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
    //methods are tested
    [RoutePrefix("api/referee")]
    public class RefereeController : ApiController
    {
        protected IRefereeService RefereeService { get; set; }

        public RefereeController(IRefereeService service)
        {
            this.RefereeService = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<RefereeView>>(await RefereeService.ReadAll());
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
                var response = Mapper.Map<RefereeView>(await RefereeService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getrefereesbytournament")]
        public async Task<HttpResponseMessage> GetRefereesByTournament(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<RefereeView>>(await RefereeService.ReadRefereeByTournament(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(RefereeView referee)
        {
            try
            {
                if (referee.Name == null || referee.Surname == null ||referee.TournamentId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                referee.Id = Guid.NewGuid();

                var response = await RefereeService.Add(Mapper.Map<RefereeDomain>(referee));

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
                var referee = Mapper.Map<RefereeView>(await RefereeService.Read(id));


                if (referee == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await RefereeService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete referee.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(RefereeView referee)
        {
            try
            {
                RefereeView toBeUpdated = Mapper.Map<RefereeView>(await RefereeService.Read(referee.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                if(referee.Name == null)
                {
                    toBeUpdated.Name = toBeUpdated.Name;
                    toBeUpdated.Surname = referee.Surname;
                    toBeUpdated.Id = toBeUpdated.Id;
                    toBeUpdated.TournamentId = toBeUpdated.TournamentId;

                }
                else if(referee.Surname == null)
                {
                    toBeUpdated.Name = referee.Name;
                    toBeUpdated.Surname = toBeUpdated.Surname;
                    toBeUpdated.Id = toBeUpdated.Id;
                    toBeUpdated.TournamentId = toBeUpdated.TournamentId;
                }
                else
                {
                    toBeUpdated.Id = toBeUpdated.Id;
                    toBeUpdated.TournamentId = toBeUpdated.TournamentId;
                    toBeUpdated.Name = referee.Name;
                    toBeUpdated.Surname = referee.Surname;
                }
                

                var response = await RefereeService.Update(Mapper.Map<RefereeDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
