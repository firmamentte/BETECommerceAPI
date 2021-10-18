using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.BLLClasses;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    [AuthenticateAccessToken]
    public class ApplicationUserController : ControllerBase
    {
        private ApplicationUserBLL ApplicationUserBLL { get; set; }
        public ApplicationUserController()
        {
            ApplicationUserBLL = new();
        }

        [Route("V1/SignUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp()
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

            await ApplicationUserBLL.SignUp(_emailAddress.FirstOrDefault(), _userPassword.FirstOrDefault());

            return StatusCode((int)HttpStatusCode.Created);
        }

        [Route("V1/SignIn")]
        [HttpPost]
        public async Task<ActionResult> SignIn()
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

            await ApplicationUserBLL.SignIn(_emailAddress.FirstOrDefault(), _userPassword.FirstOrDefault());

            return Ok();
        }
    }
}
