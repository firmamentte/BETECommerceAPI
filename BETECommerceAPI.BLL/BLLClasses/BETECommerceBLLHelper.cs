using System;
using Microsoft.Extensions.Configuration;

namespace BETECommerceAPI.BLL.BLLClasses
{
    public abstract class BETECommerceBLLHelper
    {
        public IConfiguration Configuration { get; set; }

        public void InitializeAppSettings()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirmamentUtilities.Utilities.DatabaseHelper.ConnectionString))
                {
                    FirmamentUtilities.Utilities.DatabaseHelper.ConnectionString = Configuration["ConnectionStrings:DatabasePath"];
                }
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public void RaiseServerError(string errorMessage)
        {
            try
            {
                throw new BETECommerceAPIException(errorMessage);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public string GetEnumDescription(Enum enumValue)
        {
            return FirmamentUtilities.Utilities.GetEnumDescription(enumValue);
        }

        public bool IsEmailAddress(string emailAddress)
        {
            return FirmamentUtilities.Utilities.EmailHelper.IsEmailAddress(emailAddress);
        }
    }
}
