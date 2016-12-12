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
using Tournament.Model.Common;
using Tournament.Model;

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/aspnetuser")]
    public class AspNetUserController : ApiController
    {
        protected IAspNetUserService AspNetUserService { get; set; }

        public AspNetUserController(IAspNetUserService service)
        {
            this.AspNetUserService = service;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<AspNetUserView>>(await AspNetUserService.ReadAll());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            try
            {
                var response = Mapper.Map<AspNetUserView>(await AspNetUserService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception e)
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
                var response = Mapper.Map<AspNetUserView>(await AspNetUserService.FindByUserName(username));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(AspNetUserView aspNetUser)
        {
            try
            {
                if (aspNetUser.Email == null || aspNetUser.UserName == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                aspNetUser.Id = Guid.NewGuid().ToString();
                aspNetUser.Age = 5;
                aspNetUser.EmailConfirmed = true;
                aspNetUser.TwoFactorEnabled = true;
                aspNetUser.PhoneNumberConfirmed = true;
                aspNetUser.LockoutEnabled = false;
                aspNetUser.AccessFailedCount = 2;

                var response = await AspNetUserService.Add(Mapper.Map<AspNetUserDomain>(aspNetUser));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<HttpResponseMessage> Delete(string id)
        {
            try
            {
                var aspNetUser = Mapper.Map<AspNetUserView>(await AspNetUserService.Read(id));

                //if (aspNetUser.Absence.Count != 0)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                //        "Cannot delete user. It has bound Absence.");
                //else if(aspNetUser.Absence1.Count != 0)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                //         "Cannot delete user. It has bound Absence.");
                //else if(aspNetUser.AspNetRoles.Count != 0)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                //         "Cannot delete user. It has bound user roles.");
                //else if(aspNetUser.AspNetUserClaims.Count != 0)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                //        "Cannot delete user. It has bound user claims.");
                //else if(aspNetUser.AspNetUserLogins.Count != 0)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                //       "Cannot delete user. It has bound user login.");


                if (aspNetUser == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bad id.");

                var response = await AspNetUserService.Delete(id);

                if (response == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Couldn't delete user.");

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(AspNetUserView aspNetUser)
        {
            try
            {
                var toBeUpdated = await AspNetUserService.Read(aspNetUser.Id);

                //if (toBeUpdated == null)
                //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entry not found");

                toBeUpdated.Email = aspNetUser.Email;
                toBeUpdated.UserName = aspNetUser.UserName;

                var response = await AspNetUserService.Update(Mapper.Map<IAspNetUserDomain>(toBeUpdated));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }
}