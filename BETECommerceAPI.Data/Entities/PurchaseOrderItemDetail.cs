using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class PurchaseOrderItemDetail
    {
        public Guid PurchaseOrderItemDetailId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ItemDetailId { get; set; }
        public string PictureFileName { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public decimal PercentageDiscount { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ItemDetail ItemDetail { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}
