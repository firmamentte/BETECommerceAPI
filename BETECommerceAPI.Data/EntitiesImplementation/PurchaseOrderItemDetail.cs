namespace BETECommerceAPI.Data.Entities
{
    public partial class PurchaseOrderItemDetail
    {
        public virtual string ItemDescription
        {
            get
            {
                return ItemDetail.ItemDescription;
            }
        }

        public virtual string PaymentStatus
        {
            get
            {
                return PurchaseOrder.PaymentStatus;
            }
        }

        public virtual decimal Discount
        {
            get
            {
                return SalePrice * (PercentageDiscount / 100M);
            }
        }

        public virtual decimal CurrentSalePrice
        {
            get
            {
                return SalePrice - Discount;
            }
        }

        public virtual decimal SubTotal
        {
            get
            {
                return CurrentSalePrice * Quantity;
            }
        }
    }
}
