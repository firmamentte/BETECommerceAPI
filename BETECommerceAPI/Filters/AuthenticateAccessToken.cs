using System;
using System.Linq;
using System.Net;
using BETECommerceAPI.BLL.BLLClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthenticateAccessToken : TypeFilterAttribute
    {
        public AuthenticateAccessToken() : base(typeof(AuthenticateAccessTokenImplementation)) { }
        private class AuthenticateAccessTokenImplementation : IAuthorizationFilter
        {
            private IConfiguration Configuration { get; set; }
            private ApplicationUserBLL ApplicationUserBLL { get; set; }

            public AuthenticateAccessTokenImplementation(IConfiguration configuration)
            {
                Configuration = configuration;
                ApplicationUserBLL = new();
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.Request.Headers.TryGetValue("AccessToken", out StringValues _accessToken))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(new ApiErrorResp("Access Token required"));
                }
                else
                {
                    if (!ApplicationUserBLL.IsAccessTokenValid(Configuration, _accessToken.FirstOrDefault()))
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Result = new JsonResult(new ApiErrorResp("Request Forbidden"));
                    }
                }
            }
        }
    }
}
