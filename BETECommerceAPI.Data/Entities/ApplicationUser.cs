using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public Guid ApplicationUserId { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
