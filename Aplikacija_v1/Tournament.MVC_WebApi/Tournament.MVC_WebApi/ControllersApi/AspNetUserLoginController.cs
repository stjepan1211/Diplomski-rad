using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Tournament.Model;
using Tournament.Model.Common;
using Tournament.MVC_WebApi.ViewModels;
using Tournament.Service.Common;
using Tournament.Token;

namespace Tournament.MVC_WebApi.ControllersApi
{
    [RoutePrefix("api/aspnetuserlogin")]
    public class AspNetUserLoginController : ApiController
    {
        protected IAspNetUserService AspNetUserService { get; set; }
        protected IAspNetUserLoginService AspNetUserLoginService { get; set; }

        public AspNetUserLoginController(IAspNetUserLoginService userLoginService, IAspNetUserService userService)
        {
            this.AspNetUserLoginService = userLoginService;
            this.AspNetUserService = userService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var response = Mapper.Map<IEnumerable<AspNetUserLoginView>>(await AspNetUserLoginService.ReadAll());
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
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
                var response = Mapper.Map<AspNetUserLoginView>(await AspNetUserLoginService.Read(id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Add(AspNetUserLoginView aspNetUserLogin)
        {
            try
            {
                if (aspNetUserLogin == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UserLogin is null.");
                if (aspNetUserLogin.ProviderKey == null || aspNetUserLogin.LoginProvider == null ||
                        aspNetUserLogin.UserId == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input.");

                var response = await AspNetUserLoginService.Add(Mapper.Map<AspNetUserLoginDomain>(aspNetUserLogin));

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("logintoken")]
        public async Task<HttpResponseMessage> LoginToken(UserCredentials userCredentials)
        {
            try
            {
                var userToLogin = Mapper.Map<AspNetUserView>(await AspNetUserService.FindByUserName(userCredentials.UserName));
                if (userToLogin == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User is not registered.");
                }
                else if (userCredentials.Password != userToLogin.PasswordHash)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Password is incorrect.");
                }
                else
                {
                    //generate token and send it to user
                    var tokenDuration = DateTime.UtcNow.AddMinutes(30);
                    var token = new TokenFactory(tokenDuration).GenerateToken();
                    var tokenResponse = new TokenResponse()
                    {
                        Id = userToLogin.Id,
                        UserName = userCredentials.UserName,
                        Token = token
                    };

                    var aspNetUserLoginView = new AspNetUserLoginView()
                    {
                        ProviderKey = token.TokenString,
                        UserId = userToLogin.Id,
                        LoginProvider = "Microsoft Sql Server"
                    };
                    //store user login to database
                    await AspNetUserLoginService.Add(Mapper.Map<IAspNetUserLoginDomain>(aspNetUserLoginView));

                    return Request.CreateResponse(HttpStatusCode.OK, tokenResponse);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
