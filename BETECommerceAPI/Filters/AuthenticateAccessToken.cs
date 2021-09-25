using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BETECommerceAPI.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthenticateAccessToken : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("AccessToken", out StringValues _accessToken))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ApiErrorResp("Access Token required"));
            }
            else
            {
                if (!BETECommerceAPIBLL.ApplicationUserHelper.IsAccessTokenValid(_accessToken.FirstOrDefault()))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Result = new JsonResult(new ApiErrorResp("Request Forbidden"));
                }
            }
        }
    }
}
