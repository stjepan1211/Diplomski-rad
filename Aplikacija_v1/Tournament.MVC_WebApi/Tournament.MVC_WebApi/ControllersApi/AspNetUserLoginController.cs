using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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

        [HttpPost]
        [Route("logintoken")]
        public async Task<HttpResponseMessage> LoginToken(UserCredentials userCredentials)
        {
            try
            {
                var userToLogin = Mapper.Map<AspNetUserView>(await AspNetUserService.FindByUserName(userCredentials.UserName));
                if (userToLogin == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User is not registered.");
                }

                else if (userCredentials.Password != userToLogin.PasswordHash)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Password is incorrect.");
                }
                else
                {
                    var tokenDuration = DateTime.UtcNow.AddMinutes(1);
                    var token = new TokenFactory(tokenDuration).GenerateToken();
                    var tokenResponse = new TokenResponse()
                    {
                        Username = userCredentials.UserName,
                        Token = token
                    };

                    var aspNetUserLoginView = new AspNetUserLoginView()
                    {
                        ProviderKey = token.TokenString,
                        UserId = userToLogin.Id,
                        LoginProvider = " "
                    };


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
