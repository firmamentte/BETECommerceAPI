using System;
using System.Threading.Tasks;
using BETECommerceAPI.Data.DALClasses;
using BETECommerceAPI.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace BETECommerceAPI.BLL.BLLClasses
{
    public class ApplicationUserBLL : BETECommerceBLLHelper
    {
        private ApplicationUserDAL ApplicationUserDAL { get; set; }

        public ApplicationUserBLL()
        {
            ApplicationUserDAL = new();
        }

        public void AuthenticateAccessToken(IConfiguration configuration, string accessToken)
        {
            try
            {
                if (configuration["ApiKeys:AccessToken"] != accessToken)
                {
                    RaiseServerError("Request Forbidden");
                }
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public bool IsAccessTokenValid(IConfiguration configuration, string accessToken)
        {
            try
            {
                return configuration["ApiKeys:AccessToken"] == accessToken;
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public async Task SignUp(string emailAddress, string userPassword)
        {
            try
            {
                using BETECommerceContext _dbContext = new();

                if (await ApplicationUserDAL.IsUsernameExisting(_dbContext, emailAddress))
                {
                    RaiseServerError("Email Address already existing");
                }

                ApplicationUser _applicationUser = new()
                {
                    ApplicationUserId = Guid.NewGuid(),
                    Username = emailAddress,
                    UserPassword = userPassword
                };

                await _dbContext.AddAsync(_applicationUser);
                await _dbContext.SaveChangesAsync();
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public async Task SignIn(string emailAddress, string userPassword)
        {
            try
            {
                using BETECommerceContext _dbContext = new();

                ApplicationUser _applicationUser = await ApplicationUserDAL.GetApplicationUserByUsernameAndUserPassword(_dbContext, emailAddress, userPassword);

                if (_applicationUser is null)
                {
                    RaiseServerError("Invalid Username or Password");
                }

                if (!string.Equals(_applicationUser.Username, emailAddress, StringComparison.CurrentCulture))
                {
                    RaiseServerError("Invalid Username or Password");
                }

                if (!string.Equals(_applicationUser.UserPassword, userPassword, StringComparison.CurrentCulture))
                {
                    RaiseServerError("Invalid Username or Password");
                }
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }
    }
}
