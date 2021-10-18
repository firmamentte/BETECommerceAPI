using System.Linq;
using BETECommerceAPI.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Controllers.ControllerHelpers
{
    public class ControllerHelper
    {
        public string GetHeaderUsername(HttpRequest request)
        {
            try
            {
                if (request.Headers.TryGetValue("EmailAddress", out StringValues _username))
                    return _username.FirstOrDefault();
                else
                    return null;
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }
    }
}
