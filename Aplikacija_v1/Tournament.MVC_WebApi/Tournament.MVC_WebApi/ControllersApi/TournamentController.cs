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

namespace Tournament.MVC_WebApi.ControllersApi
{
    //methods are tested
    [RoutePrefix("api/tournament")]
    public class TournamentController : ApiController
    {
        protected ITournamentService TournamentService { get; set; }

        public TournamentController(ITournamentService service)
        {
            this.TournamentService = service;
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

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(TournamentView tournament, LocationView location)
        {
            try
            {
                tournament.StartTime = DateTime.Now;
                tournament.EndTime = DateTime.Now;
                tournament.AspNetUserId = "035bd3bd-f696-4679-bff3-f291ad6f9fa5";

                if (tournament.StartTime == null || tournament.EndTime == null || tournament.Type == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                tournament.Id = Guid.NewGuid();

                var response = await TournamentService.Add(Mapper.Map<TournamentDomain>(tournament));

                return Request.CreateResponse(HttpStatusCode.OK, response);
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
