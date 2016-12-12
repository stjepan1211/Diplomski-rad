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

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(MatchView match)
        {
            try
            {
                Guid TournamentId;
                Guid RefereeId;
                Guid TeamOneId;
                Guid TeamTwoId;

                //if (true)
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                match.Id = Guid.NewGuid();

                Guid.TryParse("8888D36F-6B44-421C-BFCC-A64C698E3B09", out TournamentId);
                Guid.TryParse("F12CB751-0B2E-496E-A9C2-45EE53EE9B32", out RefereeId);
                Guid.TryParse("1D4B5874-9C48-4D2A-9745-B9724996A25A", out TeamOneId);
                Guid.TryParse("ec7e1b39-4878-4a84-8636-281546402369", out TeamTwoId);

                match.TournametId = TournamentId;
                match.RefereeId = RefereeId;
                match.TeamOneId = TeamOneId;
                match.TeamTwoId = TeamTwoId;
                match.DateTime = DateTime.Now;

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

                Guid teamTwoId;

                Guid.TryParse("ec7e1b39-4878-4a84-8636-281546402369", out teamTwoId);
                toBeUpdated.TeamOneId = teamTwoId;

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
