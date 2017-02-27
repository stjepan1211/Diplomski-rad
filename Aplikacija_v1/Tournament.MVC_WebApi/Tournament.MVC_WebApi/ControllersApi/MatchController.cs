﻿using System;
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
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {
        protected IMatchService MatchService { get; set; }

        public MatchController(IMatchService service)
        {
            this.MatchService = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadAll());
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
                var response = Mapper.Map<MatchView>(await MatchService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getmatchesbytournament")]
        public async Task<HttpResponseMessage> GetMatchesByTournament(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<MatchView>>(await MatchService.ReadMatchesByTournament(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(MatchView match)
        {
            try
            {

                if (match.TournametId == null || match.TeamOneId == null || match.TeamTwoId == null || match.DateTime == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                match.Id = Guid.NewGuid();
                match.Round = 0;
                var response = await MatchService.Add(Mapper.Map<MatchDomain>(match));

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
                var match = Mapper.Map<MatchView>(await MatchService.Read(id));


                if (match == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await MatchService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete match.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(MatchView match)
        {
            try
            {
                MatchView toBeUpdated = Mapper.Map<MatchView>(await MatchService.Read(match.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                if (match.TournametId == null || match.RefereeId == null || match.TeamOneId == null
                    || match.TeamTwoId == null || match.DateTime == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input");

                toBeUpdated.TournametId = match.TournametId;
                toBeUpdated.RefereeId = match.RefereeId;
                toBeUpdated.TeamOneId = match.TeamOneId;
                toBeUpdated.TeamTwoId = match.TeamTwoId;
                toBeUpdated.DateTime = match.DateTime;

                var response = await MatchService.Update(Mapper.Map<MatchDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
