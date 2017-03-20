using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tournament.Service.Common;
using AutoMapper;
using System.Threading.Tasks;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/result")]
    public class ResultController : ApiController
    {
        protected IResultService ResultService { get; set; }
        protected ITeamService TeamService { get; set; }
        protected IMatchService Matchservice { get; set; }
        protected IPlayerService PlayerService { get; set; }
        protected ITournamentService TournamentService { get; set; }

        public ResultController(IResultService service, ITeamService teamService, IMatchService matchService, IPlayerService playerService,
            ITournamentService tournamentService)
        {
            this.ResultService = service;
            this.TeamService = teamService;
            this.Matchservice = matchService;
            this.PlayerService = playerService;
            this.TournamentService = tournamentService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ResultView>>(await ResultService.ReadAll());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            try
            {
                var response = Mapper.Map<ResultView>(await ResultService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getresultbymatch")]
        public async Task<HttpResponseMessage> GetResultByMatch(Guid matchId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<ResultView>>(await ResultService.ReadResultByMatchId(matchId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(JObject data)
        {
            //JSON parsing example 
            //https://weblog.west-wind.com/posts/2012/aug/30/using-jsonnet-for-dynamic-json-parsing
            //JSON is send from edit result controller
            //data is one object consist of 2 objects and 4 arrays of objects
            //object1 result
            //object2 penalties
            //array object1 teamOneScorers
            //array object2 teamTwoScorers
            //array object3 playersYellowCards
            //array object4 playersRedCards
            try
            {

                //get result object from Json
                ResultView result = data["result"].ToObject<ResultView>();

                //check if penalties was selected and who is winner
                dynamic penalty = data["penalties"];
                bool wasPenalties = Convert.ToBoolean(penalty.Were);


                //sums for add goals add by scorers, it has to match team goals
                int goalsTeamOneSum = 0;
                int goalsTeamTwoSum = 0;

                //read scorers for team1 and team2 from Json
                //add them to list 
                List<PlayerView> ScorersTeamOne = new List<PlayerView>();
                List<PlayerView> ScorersTeamTwo = new List<PlayerView>();
                foreach (dynamic item in data["teamOneScorers"])
                {
                    ScorersTeamOne.Add(new PlayerView {
                        Id = item.Player.Id,
                        Name = item.Player.Name,
                        Surname = item.Player.Surname,
                        Goals = item.Player.Goals
                    });
                    goalsTeamOneSum = goalsTeamOneSum + Convert.ToInt32(item.Player.Goals);
                }
                foreach (dynamic item in data["teamTwoScorers"])
                {
                    ScorersTeamTwo.Add(new PlayerView
                    {
                        Id = item.Player.Id,
                        Name = item.Player.Name,
                        Surname = item.Player.Surname,
                        Goals = item.Player.Goals
                    });
                    goalsTeamTwoSum = goalsTeamTwoSum + Convert.ToInt32(item.Player.Goals);
                }

                //check result form
                if (result.TeamOneGoals.ToString() == null || result.TeamTwoGoals.ToString() == null || result.MatchId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid result input.");
                result.Id = Guid.NewGuid();

                dynamic tournamentId = new Guid(data["tournamentId"].ToString());

                //check if result is already added 
                var results = Mapper.Map<IEnumerable<ResultView>>(await ResultService.ReadResultByMatchId(result.MatchId)); ;

                foreach(var item in results)
                {
                    if(result.MatchId == item.MatchId)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You already added result for this match.");
                    }
                }

                //check if goals by scorers match team goals
                if (goalsTeamOneSum != result.TeamOneGoals || goalsTeamTwoSum != result.TeamTwoGoals)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Team goals and goals added by scorers" +
                       " don't correspond each other.");
                }

                //check who is winner, if score is even penalties must be checked
                if(result.TeamOneGoals > result.TeamTwoGoals)
                {
                    if(wasPenalties)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To add penalties score must be even.");

                    var playedMatch = Mapper.Map<MatchView>(await Matchservice.Read(result.MatchId));
                    playedMatch.Winner = playedMatch.Team.Id;
                    playedMatch.Penalties = false;

                    var matchResponse = await Matchservice.Update(Mapper.Map<MatchDomain>(playedMatch));
                    if (matchResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update match.");

                    var teamOne = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team.Id));
                    teamOne.MatchesPlayed += 1;
                    teamOne.Won += 1;
                    teamOne.Points += 3;
                    teamOne.GoalsScored = teamOne.GoalsScored + result.TeamOneGoals;
                    teamOne.GoalsRecieved = teamOne.GoalsRecieved + result.TeamTwoGoals;
                    var teamOneResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamOne));
                    if(teamOneResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team one.");

                    var teamTwo = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team1.Id));
                    teamTwo.MatchesPlayed += 1;
                    teamTwo.Lost += 1;
                    teamTwo.GoalsScored = teamTwo.GoalsScored + result.TeamTwoGoals;
                    teamTwo.GoalsRecieved = teamTwo.GoalsRecieved + result.TeamOneGoals;
                    var teamTwoResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamTwo));
                    if (teamTwoResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team two.");

                }
                else if(result.TeamOneGoals < result.TeamTwoGoals)
                {
                    if (wasPenalties)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To add penalties score must be even.");

                    var playedMatch = Mapper.Map<MatchView>(await Matchservice.Read(result.MatchId));
                    playedMatch.Winner = playedMatch.Team1.Id;
                    playedMatch.Penalties = false;

                    var matchResponse = await Matchservice.Update(Mapper.Map<MatchDomain>(playedMatch));
                    if (matchResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update match.");

                    var teamOne = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team.Id));
                    teamOne.MatchesPlayed += 1;
                    teamOne.Lost += 1;
                    teamOne.GoalsScored = teamOne.GoalsScored + result.TeamOneGoals;
                    teamOne.GoalsRecieved = teamOne.GoalsRecieved + result.TeamTwoGoals;

                    var teamOneResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamOne));
                    if (teamOneResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team one.");

                    var teamTwo = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team1.Id));
                    teamTwo.MatchesPlayed += 1;
                    teamTwo.Won += 1;
                    teamTwo.Points += 3;
                    teamTwo.GoalsScored = teamTwo.GoalsScored + result.TeamTwoGoals;
                    teamTwo.GoalsRecieved = teamTwo.GoalsRecieved + result.TeamOneGoals;

                    var teamTwoResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamTwo));
                    if (teamTwoResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team two.");
                }
                else if(result.TeamOneGoals == result.TeamTwoGoals)
                {
                    Guid penaltyWinnerId;
                    if (penalty.Winner != null)
                    {
                        penaltyWinnerId = penalty.Winner;

                        var playedMatch = Mapper.Map<MatchView>(await Matchservice.Read(result.MatchId));
                        playedMatch.Winner = penaltyWinnerId;
                        playedMatch.Penalties = true;

                        var matchResponse = await Matchservice.Update(Mapper.Map<MatchDomain>(playedMatch));
                        if (matchResponse == 0)
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update match.");

                        var teamOne = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team.Id));
                        teamOne.MatchesPlayed += 1;
                        teamOne.Draw +=1;
                        teamOne.Points += 1;
                        teamOne.GoalsScored = teamOne.GoalsScored + result.TeamOneGoals;
                        teamOne.GoalsRecieved = teamOne.GoalsRecieved + result.TeamTwoGoals;

                        var teamOneResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamOne));
                        if (teamOneResponse == 0)
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team one.");

                        var teamTwo = Mapper.Map<TeamView>(await TeamService.Read(playedMatch.Team1.Id));
                        teamTwo.MatchesPlayed += 1;
                        teamTwo.Draw +=1;
                        teamTwo.Points += 1;
                        teamTwo.GoalsScored = teamTwo.GoalsScored + result.TeamTwoGoals;

                        teamTwo.GoalsRecieved = teamTwo.GoalsRecieved + result.TeamOneGoals;
                        var teamTwoResponse = await TeamService.Update(Mapper.Map<TeamDomain>(teamTwo));
                        if (teamTwoResponse == 0)
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't update team two.");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You need to select penalty winner.");
                    }    
                    if (!wasPenalties)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "If score is even, you need to check" +
                            "penalties and select winner");
                    //add,result, winner, points, penalty
                }

                //read players with red and yellow cards from Json
                List<PlayerView> PlayersWithYellowCard = new List<PlayerView>();
                List<PlayerView> PlayersWithRedCard = new List<PlayerView>();
                //check and add playersYellowCards
                foreach (dynamic item in data["playersYellowCards"])
                {
                    if(Convert.ToBoolean(item.Value))
                    {
                        PlayersWithYellowCard.Add(new PlayerView
                        {
                            Id = item.Id
                        });
                    }
                }
                //check and add playersRedCards
                foreach (dynamic item in data["playersRedCards"])
                {
                    if (Convert.ToBoolean(item.Value))
                    {
                        PlayersWithRedCard.Add(new PlayerView
                        {
                            Id = item.Id
                        });
                    }
                }

                //update scorers
                foreach(var player in ScorersTeamOne)
                {
                    var playerToAddGoals = Mapper.Map<PlayerView>(await PlayerService.Read(player.Id));
                    playerToAddGoals.Goals = player.Goals;
                    var updateResponse = await PlayerService.Update(Mapper.Map<PlayerDomain>(playerToAddGoals));
                    if (updateResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Couldn't add team one scorers");
                }
                foreach(var player in ScorersTeamTwo)
                {
                    var playerToAddGoals = Mapper.Map<PlayerView>(await PlayerService.Read(player.Id));
                    playerToAddGoals.Goals = player.Goals;
                    var updateResponse = await PlayerService.Update(Mapper.Map<PlayerDomain>(playerToAddGoals));
                    if(updateResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Couldn't add team two scorers");
                }

                //update yellow cards
                foreach(var player in PlayersWithYellowCard)
                {
                    var playerToAddYellowCard = Mapper.Map<PlayerView>(await PlayerService.Read(player.Id));
                    playerToAddYellowCard.YellowCards += 1;
                    var updateResponse = await PlayerService.Update(Mapper.Map<PlayerDomain>(playerToAddYellowCard));
                    if (updateResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Couldn't add players with yellow cards");
                }

                //updates red cards
                foreach(var player in PlayersWithRedCard)
                {
                    var playerToAddRedCard = Mapper.Map<PlayerView>(await PlayerService.Read(player.Id));
                    playerToAddRedCard.RedCards += 1;
                    var updateResponse = await PlayerService.Update(Mapper.Map<PlayerDomain>(playerToAddRedCard));
                    if (updateResponse == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Couldn't add players with red cards");
                }

                //add result
                var resultResponse = await ResultService.Add(Mapper.Map<ResultDomain>(result));

                return Request.CreateResponse(HttpStatusCode.OK, 1);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                var result = Mapper.Map<ResultView>(await ResultService.Read(id));


                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await ResultService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete result.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(ResultView result)
        {
            try
            {
                ResultView toBeUpdated = Mapper.Map<ResultView>(await ResultService.Read(result.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                toBeUpdated.TeamOneGoals = result.TeamOneGoals;
                toBeUpdated.TeamTwoGoals = result.TeamTwoGoals;
                toBeUpdated.Id = toBeUpdated.Id;
                toBeUpdated.MatchId = toBeUpdated.MatchId;
                var response = await ResultService.Update(Mapper.Map<ResultDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        //this method check is dependencies such as teams and matches added
        //if they are method will response with true and then it will be possible call other web api methods
        [HttpGet]
        [Route("matchesteamsadded")]
        public async Task<HttpResponseMessage> CheckDependencies(Guid tournamentId)
        {
            try
            {
                //check  if matches are added
                var matches = Mapper.Map<IEnumerable<MatchView>>(await Matchservice.ReadMatchesByTournament(tournamentId));
                var tournament = Mapper.Map<TournamentView>(await TournamentService.Read(tournamentId));
                if(tournament.Type == "League")
                {
                    if (matches.Count() != tournament.NumberOfMatches)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Matches aren't added.");
                    }
                }
                //check  if teams are added
                var teams = Mapper.Map<IEnumerable<TeamView>>(await TeamService.GetWhereTournamentId(tournamentId));
                if(teams.Count() != tournament.NumberOfTeams)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Teams aren't added.");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Teams and matches added.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        //check  if players are added
        [HttpGet]
        [Route("playersadded")]
        public async Task<HttpResponseMessage> CheckPlayersDependencies(Guid teamId)
        {
            try
            {
                //check  if matches are added
                var players = Mapper.Map<IEnumerable<PlayerView>>(await PlayerService.ReadPlayersByTeam(teamId));
                var team = Mapper.Map<TeamView>(await TeamService.Read(teamId));
                if (players.Count() != team.NumberOfPlayers)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Players aren't added for " + team.Name);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Players added.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
