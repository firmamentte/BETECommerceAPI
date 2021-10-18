using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETECommerceAPI.Data.DALClasses
{
    public class ApplicationUserDAL
    {
        //If change this function also change IsUsernameExisting function
        public async Task<ApplicationUser> GetApplicationUserByUsername(BETECommerceContext dbContext, string username)
        {
            return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                          where applicationUser.Username == username
                          select applicationUser).
                          FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetApplicationUserByUsernameAndUserPassword(BETECommerceContext dbContext, string username, string userPassword)
        {
            return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                          where applicationUser.Username == username &&
                                applicationUser.UserPassword == userPassword
                          select applicationUser).
                          FirstOrDefaultAsync();
        }

        //If change this function also change GetApplicationUserByUsername function
        public async Task<bool> IsUsernameExisting(BETECommerceContext dbContext, string username)
        {
            return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                          where applicationUser.Username == username
                          select applicationUser).
                          AnyAsync();
        }
    }
}
