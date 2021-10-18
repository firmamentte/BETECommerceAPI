using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETECommerceAPI.Data.DALClasses
{
    public class PurchaseOrderDAL
    {
        public static async Task<bool> IsPurchaseOrderNumberExisting(BETECommerceContext dbContext, string purchaseOrderNumber)
        {
            return await (from purchaseOrder in dbContext.PurchaseOrders.Cast<PurchaseOrder>()
                          where purchaseOrder.PurchaseOrderNumber == purchaseOrderNumber
                          select purchaseOrder).
                          AnyAsync();
        }
    }
}
