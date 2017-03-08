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
    [RoutePrefix("api/player")]
    public class PlayerController : ApiController
    {
        protected IPlayerService PlayerService { get; set; }
        protected ITeamService TeamService { get; set; }
        public PlayerController(IPlayerService service, ITeamService teamService)
        {
            this.PlayerService = service;
            this.TeamService = teamService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<PlayerView>>(await PlayerService.ReadAll());
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
                var response = Mapper.Map<PlayerView>(await PlayerService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getplayersbyteam")]
        public async Task<HttpResponseMessage> GetPlayersByTeam(Guid teamId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<PlayerView>>(await PlayerService.ReadPlayersByTeam(teamId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getplayersbytournament")]
        public async Task<HttpResponseMessage> GetPlayersByTournament(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<PlayerView>>(await PlayerService.ReadPlayersByTournament(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(PlayerView player)
        {
            try
            {
                if (player.Name == null || player.Surname == null || player.TeamId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");
                var team = Mapper.Map<TeamView>(await TeamService.Read(player.TeamId));
                var playersByTeam = Mapper.Map<IEnumerable<PlayerView>>(await PlayerService.ReadPlayersByTeam(player.TeamId));

                if(playersByTeam.Count() == team.NumberOfPlayers)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You already added players for that team.");
                }

                player.Id = Guid.NewGuid();
                player.Goals = 0;
                player.RedCards = 0;
                player.YellowCards = 0;
                player.GamesPlayed = 0;

                var response = await PlayerService.Add(Mapper.Map<PlayerDomain>(player));

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
                var player = Mapper.Map<PlayerView>(await PlayerService.Read(id));


                if (player == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await PlayerService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete player.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(PlayerView player)
        {
            try
            {
                PlayerView toBeUpdated = Mapper.Map<PlayerView>(await PlayerService.Read(player.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                toBeUpdated.Name = player.Name;
                toBeUpdated.Surname = player.Surname;
                toBeUpdated.Goals = player.Goals;
                toBeUpdated.YellowCards = player.YellowCards;
                toBeUpdated.RedCards = player.RedCards;
                toBeUpdated.GamesPlayed = player.GamesPlayed;
                toBeUpdated.TeamId = player.TeamId;
                var response = await PlayerService.Update(Mapper.Map<PlayerDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
