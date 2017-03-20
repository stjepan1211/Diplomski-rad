using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tournament.Service.Common;
using AutoMapper;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Model;
using Newtonsoft.Json.Linq;

namespace Tournament.MVC_WebApi.ControllersApi
{
    //methods are tested
    [RoutePrefix("api/tournament")]
    public class TournamentController : ApiController
    {
        protected ITournamentService TournamentService { get; set; }
        protected ILocationService LocationService { get; set; }
        public TournamentController(ITournamentService tournamentService, ILocationService locationService)
        {
            this.TournamentService = tournamentService;
            this.LocationService = locationService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<TournamentView>>(await TournamentService.ReadAll());
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
                var response = Mapper.Map<TournamentView>(await TournamentService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getleaguetournaments")]
        public async Task<HttpResponseMessage> GetLeagueTournaments()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<TournamentView>>(await TournamentService.ReadLeagueTournaments());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getbyusername")]
        public async Task<HttpResponseMessage> GetByUsername(string username)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<TournamentView>>(await TournamentService.ReadByUsername(username));
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
            try
            {
                TournamentView tournament = data["tournament"].ToObject<TournamentView>();
                LocationView location = data["location"].ToObject<LocationView>();

                if (tournament.StartTime == null || tournament.EndTime == null || tournament.Type == null
                    || tournament.AspNetUserId == null || tournament.NumberOfTeams == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                tournament.Id = Guid.NewGuid();
                location.Id = Guid.NewGuid();
                location.TournamentId = tournament.Id;
                //Algorithm for generate number of matches and rounds
                switch(tournament.Type)
                {
                    case "League":
                        if(tournament.NumberOfTeams > 20 || tournament.NumberOfTeams < 2)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Number of teams in league format can't be greater than 20.");
                        }
                        if (tournament.NumberOfTeams % 2 == 0)
                        {
                            tournament.Rounds = tournament.NumberOfTeams - 1;
                        }
                        else
                        {
                            tournament.Rounds = tournament.NumberOfTeams;
                        }
                        tournament.NumberOfMatches = tournament.Rounds * (tournament.NumberOfTeams / 2);
                        break;
                    case "Playoff":
                        if(tournament.NumberOfTeams != 4 && tournament.NumberOfTeams != 8 && tournament.NumberOfTeams != 16 && tournament.NumberOfTeams != 32)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't add that number of matches. Number of matches for"
                                + " playoff type must be 4, 8, 16 or 32.");
                        }
                        else
                        {
                            switch(tournament.NumberOfTeams)
                            {
                                case 4:
                                    tournament.Rounds = 2;
                                    tournament.NumberOfMatches = 3;
                                    break;
                                case 8:
                                    tournament.Rounds = 3;
                                    tournament.NumberOfMatches = 7;
                                    break;
                                case 16:
                                    tournament.Rounds = 4;
                                    tournament.NumberOfMatches = 15;
                                    break;
                                case 32:
                                    tournament.Rounds = 5;
                                    tournament.NumberOfMatches = 31;
                                    break;
                            }
                        }
                        break;
                    case "League cup":
                        if (tournament.NumberOfTeams != 4 && tournament.NumberOfTeams != 8 && tournament.NumberOfTeams != 16 && tournament.NumberOfTeams != 32)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't add that number of matches. Number of matches for"
                                + " playoff type must be 4, 8, 16 or 32.");
                        }
                        else
                        {
                            switch (tournament.NumberOfTeams)
                            {
                                case 4:
                                    tournament.Rounds = 2;
                                    tournament.NumberOfMatches = 3;
                                    break;
                                case 8:
                                    tournament.Rounds = 3;
                                    tournament.NumberOfMatches = 7;
                                    break;
                                case 16:
                                    tournament.Rounds = 4;
                                    tournament.NumberOfMatches = 15;
                                    break;
                                case 32:
                                    tournament.Rounds = 5;
                                    tournament.NumberOfMatches = 31;
                                    break;
                            }
                        }
                        break;
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't add that type of tournament.");

                }
                //check if number of rounds can be played in interval between start and end time of tournament
                TimeSpan daysDifference = tournament.EndTime.Subtract(tournament.StartTime);
                if(daysDifference.Days < tournament.Rounds)
                {
                    if(daysDifference.Days == 0 && tournament.NumberOfTeams == 2)
                    {
                        
                    }
                    else if(daysDifference.Days == 1 && tournament.NumberOfTeams == 3)
                    {

                    }
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Date interval should be greater. Cosider that one round" +
                        " will be played one day at time.");
                }
                tournament.IsScheduled = false;

                var responseTournament = await TournamentService.Add(Mapper.Map<TournamentDomain>(tournament));
                var responseLocation = await LocationService.Add(Mapper.Map<LocationDomain>(location));

                if (responseTournament == 1 && responseLocation == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 1);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }

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
                var tournament = Mapper.Map<TournamentView>(await TournamentService.Read(id));


                if (tournament == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await TournamentService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete tournament.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(TournamentView tournament)
        {
            try
            {
                TournamentView toBeUpdated = Mapper.Map<TournamentView>(await TournamentService.Read(tournament.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                toBeUpdated.Name = tournament.Name;
                toBeUpdated.StartTime = tournament.StartTime;
                toBeUpdated.EndTime = tournament.EndTime;
                toBeUpdated.Type = tournament.Type;
                toBeUpdated.NumberOfTeams = tournament.NumberOfTeams;
                //Algorithm for generate number of matches and rounds
                if (tournament.Type == "League")
                {
                    if (tournament.NumberOfTeams % 2 == 0)
                    {
                        tournament.Rounds = tournament.NumberOfTeams - 1;
                        tournament.NumberOfMatches = tournament.Rounds * tournament.NumberOfTeams / 2;
                    }
                }
                var response = await TournamentService.Update(Mapper.Map<TournamentDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
