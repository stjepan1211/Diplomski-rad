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
using Tournament.Model.Common;

namespace Tournament.MVC_WebApi.ControllersApi
{
    //methods are tested
    [RoutePrefix("api/team")]
    public class TeamController : ApiController
    {
        protected ITeamService TeamService { get; set; }

        public TeamController(ITeamService service)
        {
            this.TeamService = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<TeamView>>(await TeamService.ReadAll());
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
                var response = Mapper.Map<TeamView>(await TeamService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getallwheretournamentid")]
        public async Task<HttpResponseMessage> GetWhereTournamentId(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<TeamView>>(await TeamService.GetWhereTournamentId(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(TeamView team)
        {
            try
            {
                if (team.Name == null || team.NumberOfPlayers == null || team.TournamentId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                team.Id = Guid.NewGuid();
                team.MatchesPlayed = 0;
                team.Won = 0;
                team.Lost = 0;

                var response = await TeamService.Add(Mapper.Map<TeamDomain>(team));

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
                var team = Mapper.Map<TeamView>(await TeamService.Read(id));


                if (team == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await TeamService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete team.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(TeamView team)
        {
            try
            {
                TeamView toBeUpdated = Mapper.Map<TeamView>(await TeamService.Read(team.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                if(team.Name == null)
                {
                    toBeUpdated.Name = toBeUpdated.Name;
                    toBeUpdated.Lost = toBeUpdated.Lost;
                    toBeUpdated.Won = toBeUpdated.Won;
                    toBeUpdated.MatchesPlayed = toBeUpdated.MatchesPlayed;
                    toBeUpdated.NumberOfPlayers = team.NumberOfPlayers;
                }
                else if(team.NumberOfPlayers == null)
                {
                    toBeUpdated.Name = team.Name;
                    toBeUpdated.Lost = toBeUpdated.Lost;
                    toBeUpdated.Won = toBeUpdated.Won;
                    toBeUpdated.MatchesPlayed = toBeUpdated.MatchesPlayed;
                    toBeUpdated.NumberOfPlayers = toBeUpdated.NumberOfPlayers;
                }
                else
                {
                    toBeUpdated.Name = team.Name;
                    toBeUpdated.Lost = toBeUpdated.Lost;
                    toBeUpdated.Won = toBeUpdated.Won;
                    toBeUpdated.MatchesPlayed = toBeUpdated.MatchesPlayed;
                    toBeUpdated.NumberOfPlayers = team.NumberOfPlayers;
                }
                

                var response = await TeamService.Update(Mapper.Map<TeamDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


    }
}
