using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class ItemDetail
    {
        public ItemDetail()
        {
            ItemDetailPictures = new HashSet<ItemDetailPicture>();
            PurchaseOrderItemDetails = new HashSet<PurchaseOrderItemDetail>();
        }

        public Guid ItemDetailId { get; set; }
        public string ItemDescription { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PercentageDiscount { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<ItemDetailPicture> ItemDetailPictures { get; set; }
        public virtual ICollection<PurchaseOrderItemDetail> PurchaseOrderItemDetails { get; set; }
    }
}
