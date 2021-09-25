using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BETECommerceAPI.BLL;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    [AuthenticateAccessToken]
    public class ApplicationUser : ControllerBase
    {
        [Route("V1/SignUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp()
        {
            try
            {
                #region RequestValidation

                ModelState.Clear();

                if (!Request.Headers.TryGetValue("EmailAddress", out StringValues _emailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Email Address required");
                }

                if (!Request.Headers.TryGetValue("UserPassword", out StringValues _userPassword))
                {
                    ModelState.AddModelError("UserPassword", "User Password required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiErrorResp(ModelState));
                }

                #endregion

                await BETECommerceAPIBLL.ApplicationUserHelper.SignUp(_emailAddress.FirstOrDefault(), _userPassword.FirstOrDefault());

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        [Route("V1/SignIn")]
        [HttpPost]
        public async Task<ActionResult> SignIn()
        {
            try
            {
                #region RequestValidation

                ModelState.Clear();

                if (!Request.Headers.TryGetValue("EmailAddress", out StringValues _emailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Email Address required");
                }

                if (!Request.Headers.TryGetValue("UserPassword", out StringValues _userPassword))
                {
                    ModelState.AddModelError("UserPassword", "User Password required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiErrorResp(ModelState));
                }

                #endregion

                await BETECommerceAPIBLL.ApplicationUserHelper.SignIn(_emailAddress.FirstOrDefault(), _userPassword.FirstOrDefault());

                return Ok();
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }
    }
}
