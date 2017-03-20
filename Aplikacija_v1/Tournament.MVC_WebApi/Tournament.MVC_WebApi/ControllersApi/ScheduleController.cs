using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tournament.Model;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Service.Common;

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/schedule")]
    public class ScheduleController : ApiController
    {
        protected ITournamentService Tournamentservice { get; private set; }
        protected ITeamService TeamService { get; private set; }
        protected IMatchService MatchService { get; set; }
        protected IRefereeService RefereeService { get; set; }

        public ScheduleController (ITournamentService tournamentService, ITeamService teamService, IMatchService matchService,
            IRefereeService refereeService)
        {
            this.Tournamentservice = tournamentService;
            this.TeamService = teamService;
            this.MatchService = matchService;
            this.RefereeService = refereeService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(Guid tournamentId)
        {
            try
            {
                var tournament = Mapper.Map<TournamentView>(await Tournamentservice.Read(tournamentId));
                if (tournamentId == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid tournament id");
                }
                //check if tournament is already scheduled
                if (tournament.IsScheduled == true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tournament is already scheduled.");
                }
                //check if number of teams in table Tournament.NumberOfTeams is equal to objects in table Teams
                var teams = Mapper.Map<IEnumerable<TeamView>>(await TeamService.GetWhereTournamentId(tournamentId));
                if (teams.Count() != tournament.NumberOfTeams)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Teams aren't added.");
                }
                //check if referee or referees are added
                var referees = Mapper.Map<IEnumerable<RefereeView>>(await RefereeService.ReadRefereeByTournament(tournamentId));
                if (referees.Count() == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Referees aren't added.");
                }

                //check which type tournament is, algorith will be different for league and playoff type
                switch (tournament.Type)
                {
                    case "League":
                        //check if number of teams is even or odd - algorithm will be different
                        switch (tournament.NumberOfTeams % 2)
                        {
                            case 0:
                                //*******************************************************************************************************
                                //even number of teams
                                TeamView lastTeam = teams.Last();
                                List<TeamView> teamsWithoutLast = teams.Take(teams.Count() - 1).ToList();
                                //array of teams without last must be copied for rotating it
                                //it will be rotated in the end of this case block
                                //type list is used because Capacity property can't be used on IEnumerable
                                List<TeamView> shiftedTeams = new List<TeamView>(teams.Count() - 1);
                                int counterDown = teamsWithoutLast.Count() - 1;
                                //days difference
                                int daysDifferenceBetweenMatchesToAdd = 0;
                                for (int i = 0; i < tournament.Rounds; i++)
                                {
                                    counterDown = teamsWithoutLast.Count() - 1;
                                    MatchView match = new MatchView { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = i + 1 };
                                    int matchesPerRound = teams.Count() / 2;
                                    //pair with fixed team (last)
                                    match.TeamOneId = teamsWithoutLast.First().Id;
                                    match.TeamTwoId = lastTeam.Id;
                                    //number of referees need to be equal or greater than number of matches per round
                                    if (referees.Count() < matchesPerRound)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Not enough referees.");
                                    }
                                    //number of days of tournament need to be equal or greater than number of rounds
                                    TimeSpan daysDifference = tournament.EndTime.Subtract(tournament.StartTime); //between tournament end and start date
                                    if (daysDifference.Days < tournament.Rounds)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.UpgradeRequired, "There is not enough days between dates. " +
                                            "There need to be equal or more days than rounds.");
                                    }
                                    //set match date and time, it has to be days, beetween tournament date and time but if
                                    //there is a big number between them it dates per round has to be periodical setted
                                    int daysDifferenceBetweenMatches = Convert.ToInt32(daysDifference.Days / tournament.Rounds); //between matches
                                    if (i == 0)
                                    {
                                        match.DateTime = tournament.StartTime;
                                    }
                                    else
                                    {
                                        daysDifferenceBetweenMatchesToAdd = daysDifferenceBetweenMatchesToAdd + daysDifferenceBetweenMatches;
                                        match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesToAdd);
                                    }
                                    match.RefereeId = referees.First().Id;
                                    //this two properties will be null for initial values, it will be updated when user add result
                                    match.Penalties = null;
                                    match.Winner = null;
                                    //add match between fixed and first team
                                    await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    //add rest of teams, second team and last team, third and team before last...
                                    for (int j = 1; j < matchesPerRound; j++)
                                    {
                                        match.Id = Guid.NewGuid();
                                        match.Round = i + 1;
                                        match.TeamOneId = teamsWithoutLast.ElementAt(j).Id;
                                        match.TeamTwoId = teamsWithoutLast.ElementAt(counterDown).Id;
                                        match.RefereeId = referees.ElementAt(j).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    //shift teams 
                                    for (int k = 0; k < teamsWithoutLast.Count(); k++)
                                    {
                                        int shiftIndex = ((k + 1) % shiftedTeams.Capacity);
                                        shiftedTeams.Add(teamsWithoutLast.ElementAt(shiftIndex));
                                    }
                                    //store shifted teams in teams without last
                                    teamsWithoutLast.Clear();
                                    foreach (var team in shiftedTeams)
                                    {
                                        teamsWithoutLast.Add(team);
                                    }
                                    shiftedTeams.Clear();
                                }
                                tournament.IsScheduled = true;
                                await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                //*******************************************************************************************************
                                return Request.CreateResponse(HttpStatusCode.OK, "1");
                            default:
                                //*******************************************************************************************************
                                //odd number of teams
                                List<TeamView> oddTeams = teams.Take(teams.Count()).ToList();
                                //array of teams without last must be copied for rotating it
                                //it will be rotated in the end of this case block
                                //type list is used because Capacity property can't be used on IEnumerable
                                List<TeamView> shiftedOddTeams = new List<TeamView>(teams.Count());
                                int oddCounterDown = oddTeams.Count() - 1;
                                int daysDifferenceBetweenMatchesToAddOdd = 0;
                                for (int i = 0; i < tournament.Rounds; i++)
                                {
                                    oddCounterDown = oddTeams.Count() - 2;
                                    MatchView match = new MatchView { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = i + 1 };
                                    int matchesPerRound = teams.Count() / 2;
                                    //number of referees need to be equal or greater than number of matches per round
                                    if (referees.Count() < matchesPerRound)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not enough referees.");
                                    }
                                    //number of days of tournament need to be equal or greater than number of rounds
                                    TimeSpan daysDifference = tournament.EndTime.Subtract(tournament.StartTime); //between tournament end and start date
                                    if (daysDifference.Days < tournament.Rounds)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough days between dates. " +
                                            "There need to be equal or more days than rounds.");
                                    }
                                    //set match date and time, it has to be days, beetween tournament date and time but if
                                    //there is a big number between them it dates per round has to be periodical setted
                                    int daysDifferenceBetweenMatches = Convert.ToInt32(daysDifference.Days / tournament.Rounds); //between matches
                                    if (i == 0)
                                    {
                                        match.DateTime = tournament.StartTime;
                                    }
                                    else
                                    {
                                        match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatches);
                                    }
                                    if (i == 0)
                                    {
                                        match.DateTime = tournament.StartTime;
                                    }
                                    else
                                    {
                                        daysDifferenceBetweenMatchesToAddOdd = daysDifferenceBetweenMatchesToAddOdd + daysDifferenceBetweenMatches;
                                        match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesToAddOdd);
                                    }
                                    //add teams, first team and before the last team...
                                    for (int j = 0; j < matchesPerRound; j++)
                                    {
                                        match.Id = Guid.NewGuid();
                                        match.Round = i + 1;
                                        match.TeamOneId = oddTeams.ElementAt(j).Id;
                                        match.TeamTwoId = oddTeams.ElementAt(oddCounterDown).Id;
                                        match.RefereeId = referees.ElementAt(j).Id;
                                        oddCounterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    //shift teams 
                                    for (int k = 0; k < oddTeams.Count(); k++)
                                    {
                                        int shiftIndex = ((k + 1) % shiftedOddTeams.Capacity);
                                        shiftedOddTeams.Add(oddTeams.ElementAt(shiftIndex));
                                    }
                                    //store shifted teams in teams without last
                                    oddTeams.Clear();
                                    foreach (var team in shiftedOddTeams)
                                    {
                                        oddTeams.Add(team);
                                    }
                                    shiftedOddTeams.Clear();
                                }
                                tournament.IsScheduled = true;
                                await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                //*******************************************************************************************************
                                return Request.CreateResponse(HttpStatusCode.OK, "1");
                        }
                    case "Playoff":
                        //*******************************************************************************************************
                        TimeSpan daysDifferencePlayoff = tournament.EndTime.Subtract(tournament.StartTime); //between tournament end and start date
                        if (daysDifferencePlayoff.Days < tournament.Rounds)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough days between dates. " +
                                "There need to be more days than rounds.");
                        }
                        //set match date and time, it has to be days, beetween tournament date and time but if
                        //there is a big number between them it dates per round has to be periodical setted
                        int daysDifferenceBetweenMatchesPlayoff = Convert.ToInt32(daysDifferencePlayoff.Days / tournament.Rounds); //between matches
                        switch (tournament.Rounds)
                        {
                            //algorithm for 32 teams

                            case 5:
                                var matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches32Teams.Count() == 0)
                                {
                                    int counterDown = 31;
                                    if (referees.Count() < 16)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 16; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches32Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches32Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches32Teams.Count() == 0)
                                    {
                                        int counterDown = 15;
                                        if (referees.Count() < 8)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 8; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round two is scheduled check if results are added
                                        foreach (var item in matches32Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round three
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches32Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches32Teams.Count() == 0)
                                        {
                                            int counterDown = 7;
                                            if (referees.Count() < 4)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 4; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            //if round three is scheduled check if results are added
                                            foreach (var item in matches32Teams)
                                            {
                                                if (item.Winner == null)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                }
                                            }
                                            //if round one is scheduled, user can schedule round four
                                            //first algorithm need to get winners from previous round
                                            winners.Clear();
                                            foreach (var item in matches32Teams)
                                            {
                                                winners.Add((Guid)item.Winner);
                                            }
                                            matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 4));
                                            if (matches32Teams.Count() == 0)
                                            {
                                                int counterDown = 3;
                                                if (referees.Count() < 2)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                }
                                                for (int i = 0; i < 2; i++)
                                                {
                                                    MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 4 };
                                                    match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 3);
                                                    match.RefereeId = referees.ElementAt(i).Id;
                                                    match.TeamOneId = winners.ElementAt(i);
                                                    match.TeamTwoId = winners.ElementAt(counterDown);
                                                    counterDown--;
                                                    await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                }
                                                return Request.CreateResponse(HttpStatusCode.OK, "Round four scheduled.");
                                            }
                                            else
                                            {
                                                //if round three is scheduled check if results are added
                                                foreach (var item in matches32Teams)
                                                {
                                                    if (item.Winner == null)
                                                    {
                                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                    }
                                                }
                                                //if round one is scheduled, user can schedule round two
                                                //first algorithm need to get winners from previous round
                                                winners.Clear();
                                                foreach (var item in matches32Teams)
                                                {
                                                    winners.Add((Guid)item.Winner);
                                                }
                                                matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 5));
                                                if (matches32Teams.Count() == 0)
                                                {
                                                    int counterDown = 1;
                                                    if (referees.Count() < 1)
                                                    {
                                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                    }
                                                    for (int i = 0; i < 1; i++)
                                                    {
                                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 5 };
                                                        match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 4);
                                                        match.RefereeId = referees.ElementAt(i).Id;
                                                        match.TeamOneId = winners.ElementAt(i);
                                                        match.TeamTwoId = winners.ElementAt(counterDown);
                                                        counterDown--;
                                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                    }
                                                    tournament.IsScheduled = true;
                                                    await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                                    return Request.CreateResponse(HttpStatusCode.OK, "Round five scheduled.");
                                                }
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round five is already scheduled.");
                                            }
                                        }
                                    }
                                }
                            //algorithm for 16 teams
                            case 4:
                                var matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches16Teams.Count() == 0)
                                {
                                    int counterDown = 15;
                                    if (referees.Count() < 8)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 8; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches16Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches16Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches16Teams.Count() == 0)
                                    {
                                        int counterDown = 7;
                                        if (referees.Count() < 4)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 4; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round two is scheduled check if results are added
                                        foreach (var item in matches16Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round two
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches16Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches16Teams.Count() == 0)
                                        {
                                            int counterDown = 3;
                                            if (referees.Count() < 2)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 2; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            //if round three is scheduled check if results are added
                                            foreach (var item in matches16Teams)
                                            {
                                                if (item.Winner == null)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                }
                                            }
                                            //if round one is scheduled, user can schedule round two
                                            //first algorithm need to get winners from previous round
                                            winners.Clear();
                                            foreach (var item in matches16Teams)
                                            {
                                                winners.Add((Guid)item.Winner);
                                            }
                                            matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 4));
                                            if (matches16Teams.Count() == 0)
                                            {
                                                int counterDown = 1;
                                                if (referees.Count() < 1)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                }
                                                for (int i = 0; i < 1; i++)
                                                {
                                                    MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 4 };
                                                    match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 3);
                                                    match.RefereeId = referees.ElementAt(i).Id;
                                                    match.TeamOneId = winners.ElementAt(i);
                                                    match.TeamTwoId = winners.ElementAt(counterDown);
                                                    counterDown--;
                                                    await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                }
                                                tournament.IsScheduled = true;
                                                await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                                return Request.CreateResponse(HttpStatusCode.OK, "Round four scheduled.");
                                            }
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round three is already scheduled.");
                                        }
                                    }
                                }
                            //algorithm for 8 teams
                            case 3:
                                var matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches8Teams.Count() == 0)
                                {
                                    int counterDown = 7;
                                    if (referees.Count() < 4)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 4; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches8Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches8Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches8Teams.Count() == 0)
                                    {
                                        int counterDown = 3;
                                        if (referees.Count() < 2)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 2; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round one is scheduled check if results are added
                                        foreach (var item in matches8Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round two
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches8Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches8Teams.Count() == 0)
                                        {
                                            int counterDown = 1;
                                            if (referees.Count() < 1)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 1; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            tournament.IsScheduled = true;
                                            await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round three is already scheduled.");
                                        }
                                    }
                                }
                            //***************************************************************
                            //algorithm for 4 teams
                            case 2:
                                var matches4Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches4Teams.Count() == 0)
                                {
                                    int counterDown = 3;
                                    if (referees.Count() < 2)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 2; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches4Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from preiouse round
                                    List<Guid> winners = new List<Guid>();
                                    foreach(var item in matches4Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches4Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches4Teams.Count() == 0)
                                    {
                                        int counterDown = 1;
                                        if (referees.Count() < 1)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 1; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesPlayoff);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        tournament.IsScheduled = true;
                                        await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Round two is already scheduled.");
                                    }
                                }
                                //***************************************************************
                        }
                        break;
                    case "League cup":
                        //*******************************************************************************************************
                        TimeSpan daysDifferenceLeagueCup = tournament.EndTime.Subtract(tournament.StartTime); //between tournament end and start date
                        if (daysDifferenceLeagueCup.Days < tournament.Rounds)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough days between dates. " +
                                "There need to be more days than rounds.");
                        }
                        //set match date and time, it has to be days, beetween tournament date and time but if
                        //there is a big number between them it dates per round has to be periodical setted
                        int daysDifferenceBetweenMatchesLeagueCup = Convert.ToInt32(daysDifferenceLeagueCup.Days / tournament.Rounds); //between matches
                        switch (tournament.Rounds)
                        {
                            //algorithm for 32 teams

                            case 5:
                                var matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches32Teams.Count() == 0)
                                {
                                    int counterDown = 31;
                                    if (referees.Count() < 16)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 16; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches32Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches32Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches32Teams.Count() == 0)
                                    {
                                        int counterDown = 15;
                                        if (referees.Count() < 8)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 8; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round two is scheduled check if results are added
                                        foreach (var item in matches32Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round three
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches32Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches32Teams.Count() == 0)
                                        {
                                            int counterDown = 7;
                                            if (referees.Count() < 4)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 4; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            //if round three is scheduled check if results are added
                                            foreach (var item in matches32Teams)
                                            {
                                                if (item.Winner == null)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                }
                                            }
                                            //if round one is scheduled, user can schedule round four
                                            //first algorithm need to get winners from previous round
                                            winners.Clear();
                                            foreach (var item in matches32Teams)
                                            {
                                                winners.Add((Guid)item.Winner);
                                            }
                                            matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 4));
                                            if (matches32Teams.Count() == 0)
                                            {
                                                int counterDown = 3;
                                                if (referees.Count() < 2)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                }
                                                for (int i = 0; i < 2; i++)
                                                {
                                                    MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 4 };
                                                    match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 3);
                                                    match.RefereeId = referees.ElementAt(i).Id;
                                                    match.TeamOneId = winners.ElementAt(i);
                                                    match.TeamTwoId = winners.ElementAt(counterDown);
                                                    counterDown--;
                                                    await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                }
                                                return Request.CreateResponse(HttpStatusCode.OK, "Round four scheduled.");
                                            }
                                            else
                                            {
                                                //if round three is scheduled check if results are added
                                                foreach (var item in matches32Teams)
                                                {
                                                    if (item.Winner == null)
                                                    {
                                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                    }
                                                }
                                                //if round one is scheduled, user can schedule round two
                                                //first algorithm need to get winners from previous round
                                                winners.Clear();
                                                foreach (var item in matches32Teams)
                                                {
                                                    winners.Add((Guid)item.Winner);
                                                }
                                                matches32Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 5));
                                                if (matches32Teams.Count() == 0)
                                                {
                                                    int counterDown = 1;
                                                    if (referees.Count() < 1)
                                                    {
                                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                    }
                                                    for (int i = 0; i < 1; i++)
                                                    {
                                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 5 };
                                                        match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 4);
                                                        match.RefereeId = referees.ElementAt(i).Id;
                                                        match.TeamOneId = winners.ElementAt(i);
                                                        match.TeamTwoId = winners.ElementAt(counterDown);
                                                        counterDown--;
                                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                    }
                                                    tournament.IsScheduled = true;
                                                    await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                                    return Request.CreateResponse(HttpStatusCode.OK, "Round five scheduled.");
                                                }
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round five is already scheduled.");
                                            }
                                        }
                                    }
                                }
                            //algorithm for 16 teams
                            case 4:
                                var matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches16Teams.Count() == 0)
                                {
                                    int counterDown = 15;
                                    if (referees.Count() < 8)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 8; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches16Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches16Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches16Teams.Count() == 0)
                                    {
                                        int counterDown = 7;
                                        if (referees.Count() < 4)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 4; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round two is scheduled check if results are added
                                        foreach (var item in matches16Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round two
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches16Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches16Teams.Count() == 0)
                                        {
                                            int counterDown = 3;
                                            if (referees.Count() < 2)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 2; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            //if round three is scheduled check if results are added
                                            foreach (var item in matches16Teams)
                                            {
                                                if (item.Winner == null)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                                }
                                            }
                                            //if round one is scheduled, user can schedule round two
                                            //first algorithm need to get winners from previous round
                                            winners.Clear();
                                            foreach (var item in matches16Teams)
                                            {
                                                winners.Add((Guid)item.Winner);
                                            }
                                            matches16Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 4));
                                            if (matches16Teams.Count() == 0)
                                            {
                                                int counterDown = 1;
                                                if (referees.Count() < 1)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                                }
                                                for (int i = 0; i < 1; i++)
                                                {
                                                    MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 4 };
                                                    match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 3);
                                                    match.RefereeId = referees.ElementAt(i).Id;
                                                    match.TeamOneId = winners.ElementAt(i);
                                                    match.TeamTwoId = winners.ElementAt(counterDown);
                                                    counterDown--;
                                                    await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                                }
                                                tournament.IsScheduled = true;
                                                await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                                return Request.CreateResponse(HttpStatusCode.OK, "Round four scheduled.");
                                            }
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round three is already scheduled.");
                                        }
                                    }
                                }
                            //algorithm for 8 teams
                            case 3:
                                var matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches8Teams.Count() == 0)
                                {
                                    int counterDown = 7;
                                    if (referees.Count() < 4)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 4; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                //if round one is scheduled schedule round 2
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches8Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from previous round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches8Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches8Teams.Count() == 0)
                                    {
                                        int counterDown = 3;
                                        if (referees.Count() < 2)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 2; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        //if round one is scheduled check if results are added
                                        foreach (var item in matches8Teams)
                                        {
                                            if (item.Winner == null)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                            }
                                        }
                                        //if round one is scheduled, user can schedule round two
                                        //first algorithm need to get winners from previous round
                                        winners.Clear();
                                        foreach (var item in matches8Teams)
                                        {
                                            winners.Add((Guid)item.Winner);
                                        }
                                        matches8Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 3));
                                        if (matches8Teams.Count() == 0)
                                        {
                                            int counterDown = 1;
                                            if (referees.Count() < 1)
                                            {
                                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                            }
                                            for (int i = 0; i < 1; i++)
                                            {
                                                MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 3 };
                                                match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup * 2);
                                                match.RefereeId = referees.ElementAt(i).Id;
                                                match.TeamOneId = winners.ElementAt(i);
                                                match.TeamTwoId = winners.ElementAt(counterDown);
                                                counterDown--;
                                                await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                            }
                                            tournament.IsScheduled = true;
                                            await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                            return Request.CreateResponse(HttpStatusCode.OK, "Round three scheduled.");
                                        }
                                        else
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Round three is already scheduled.");
                                        }
                                    }
                                }
                            //***************************************************************
                            //algorithm for 4 teams
                            case 2:
                                var matches4Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 1));
                                //check if round one is scheduled
                                if (matches4Teams.Count() == 0)
                                {
                                    int counterDown = 3;
                                    if (referees.Count() < 2)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                    }
                                    for (int i = 0; i < 2; i++)
                                    {
                                        MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 1 };
                                        match.DateTime = tournament.StartTime;
                                        match.RefereeId = referees.ElementAt(i).Id;
                                        match.TeamOneId = teams.ElementAt(i).Id;
                                        match.TeamTwoId = teams.ElementAt(counterDown).Id;
                                        counterDown--;
                                        await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                    }
                                    return Request.CreateResponse(HttpStatusCode.OK, "Round one scheduled.");
                                }
                                else
                                {
                                    //if round one is scheduled check if results are added
                                    foreach (var item in matches4Teams)
                                    {
                                        if (item.Winner == null)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "To schedule new round, you need to add results from previous round.");
                                        }
                                    }
                                    //if round one is scheduled, user can schedule round two
                                    //first algorithm need to get winners from preiouse round
                                    List<Guid> winners = new List<Guid>();
                                    foreach (var item in matches4Teams)
                                    {
                                        winners.Add((Guid)item.Winner);
                                    }
                                    matches4Teams = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadByTournamentAndRound(tournament.Id, 2));
                                    if (matches4Teams.Count() == 0)
                                    {
                                        int counterDown = 1;
                                        if (referees.Count() < 1)
                                        {
                                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There is not enough referees.");
                                        }
                                        for (int i = 0; i < 1; i++)
                                        {
                                            MatchView match = new MatchView() { Id = Guid.NewGuid(), TournametId = tournament.Id, Round = 2 };
                                            match.DateTime = tournament.StartTime.AddDays(daysDifferenceBetweenMatchesLeagueCup);
                                            match.RefereeId = referees.ElementAt(i).Id;
                                            match.TeamOneId = winners.ElementAt(i);
                                            match.TeamTwoId = winners.ElementAt(counterDown);
                                            counterDown--;
                                            await MatchService.Add(Mapper.Map<MatchDomain>(match));
                                        }
                                        tournament.IsScheduled = true;
                                        await Tournamentservice.Update(Mapper.Map<TournamentDomain>(tournament));
                                        return Request.CreateResponse(HttpStatusCode.OK, "Round two scheduled.");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Round two is already scheduled.");
                                    }
                                }
                                //***************************************************************
                        }
                        break;

                }
                return Request.CreateResponse(HttpStatusCode.OK, 1);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
