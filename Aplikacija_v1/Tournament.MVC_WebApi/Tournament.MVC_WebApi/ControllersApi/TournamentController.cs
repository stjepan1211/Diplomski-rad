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
                if (tournament.Type == "League")
                {
                    if (tournament.NumberOfTeams % 2 == 0)
                    {
                        tournament.Rounds = tournament.NumberOfTeams - 1;
                        tournament.NumberOfMatches = tournament.Rounds * tournament.NumberOfTeams / 2;
                    }
                }

                var responseTournament = await TournamentService.Add(Mapper.Map<TournamentDomain>(tournament));
                var responseLocation = await LocationService.Add(Mapper.Map<LocationDomain>(location));

                if(responseTournament == 1 && responseLocation == 1)
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

                toBeUpdated.StartTime = DateTime.Now;
                toBeUpdated.EndTime = DateTime.Now;
                toBeUpdated.Type = tournament.Type;
                tournament.AspNetUserId = "a4057af7-2e85-49bb-afe2-7ec14d4cfaf6";
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
