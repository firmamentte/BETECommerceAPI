using System;

namespace BETECommerceAPI.BLL
{
    public class BETECommerceAPIException : Exception
    {
        public BETECommerceAPIException(string errorMessage) : base(errorMessage)
        { }
    }
}
