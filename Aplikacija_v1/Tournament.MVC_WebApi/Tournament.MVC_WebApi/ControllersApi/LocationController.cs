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
using Tournament.Model.Common;

namespace Tournament.MVC_WebApi.ControllersApi
{
    //methods are tested
    [RoutePrefix("api/location")]
    public class LocationController : ApiController
    {
        protected ILocationService LocationService { get; set; }

        public LocationController(ILocationService service)
        {
            this.LocationService = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<LocationView>>(await LocationService.ReadAll());
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
                var response = Mapper.Map<LocationView>(await LocationService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("getbytournament")]
        public async Task<HttpResponseMessage> GetByTournament(Guid tournamentId)
        {
            try
            {
                var response = Mapper.Map<IEnumerable<LocationView>>(await LocationService.ReadByTournament(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(LocationView location)
        {
            try
            {
                Guid guid;

                if (location.Latitude == null || location.Longitude == null || location.Description == null || location.Place == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                location.Id = Guid.NewGuid();

                Guid.TryParse("8888D36F-6B44-421C-BFCC-A64C698E3B09", out guid);

                location.TournamentId = guid;

                var response = await LocationService.Add(Mapper.Map<LocationDomain>(location));

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
                var location = Mapper.Map<LocationView>(await LocationService.Read(id));


                if (location == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await LocationService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete location.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(LocationView location)
        {
            try
            {
                LocationView toBeUpdated = Mapper.Map<LocationView>(await LocationService.Read(location.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                toBeUpdated.Longitude = location.Longitude;
                toBeUpdated.Latitude = location.Latitude;
                toBeUpdated.Place = location.Place;
                toBeUpdated.Description = location.Description;

                var response = await LocationService.Update(Mapper.Map<LocationDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
