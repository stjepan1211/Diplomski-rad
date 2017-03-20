using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tournament.Model;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Service.Common;

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/gallery")]
    public class GalleryController : ApiController
    {
        protected IGalleryService GalleryService { get; set; }

        public GalleryController(IGalleryService gallery)
        {
            this.GalleryService = gallery;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<GalleryView>>(await GalleryService.ReadAll());
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
                var response = Mapper.Map<GalleryView>(await GalleryService.Read(id));
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
                var response = Mapper.Map<IEnumerable<GalleryView>>(await GalleryService.GetWhereTournamentId(tournamentId));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(GalleryView gallery)
        {
            try
            {
                if (gallery.Url == null  || gallery.TournamentId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                gallery.Id = Guid.NewGuid();

                Uri uriResult;
                bool result = Uri.TryCreate(gallery.Url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

               if(!result)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid url.");
                }

                var response = await GalleryService.Add(Mapper.Map<GalleryDomain>(gallery));

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
                var gallery = Mapper.Map<GalleryView>(await GalleryService.Read(id));


                if (gallery == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await GalleryService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete gallery.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(GalleryView gallery)
        {
            try
            {
                GalleryView toBeUpdated = Mapper.Map<GalleryView>(await GalleryService.Read(gallery.Id));

                if (toBeUpdated == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");


                var response = await GalleryService.Update(Mapper.Map<GalleryDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
