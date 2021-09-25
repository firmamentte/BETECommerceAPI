using System.ComponentModel;

namespace BETECommerceAPI.BLL
{
    public class BETECommerceAPIEnum
    {
        public enum Status
        {
            [Description("Pending")]
            Pending,
            [Description("Paid")]
            Paid,
            [Description("Shipping")]
            Shipping,
            [Description("Delivered")]
            Delivered,
            [Description("Refund")]
            Refund,
            [Description("Refunded")]
            Refunded,
            [Description("Failed")]
            Failed
        }
    }
}
