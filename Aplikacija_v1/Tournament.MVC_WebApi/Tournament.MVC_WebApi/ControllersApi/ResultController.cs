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

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/result")]
    public class ResultController : ApiController
    {
        protected IResultService ResultService { get; set; }

        public ResultController(IResultService service)
        {
            this.ResultService = service;
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
        public async Task<HttpResponseMessage> Add(ResultView result)
        {
            try
            {
                if (result.TeamOneGoals.ToString() == null || result.TeamTwoGoals.ToString() == null || result.MatchId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                result.Id = Guid.NewGuid();

                var response = await ResultService.Add(Mapper.Map<ResultDomain>(result));

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
    }
}
