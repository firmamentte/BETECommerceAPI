using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class PurchaseOrder
    {
        public PurchaseOrder()
        {
            PurchaseOrderItemDetails = new HashSet<PurchaseOrderItemDetail>();
        }

        public Guid PurchaseOrderId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<PurchaseOrderItemDetail> PurchaseOrderItemDetails { get; set; }
    }
}
