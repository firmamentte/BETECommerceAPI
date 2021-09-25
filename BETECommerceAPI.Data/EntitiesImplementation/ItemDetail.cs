
namespace BETECommerceAPI.Data.Entities
{
    public partial class ItemDetail
    {
        public virtual decimal Discount
        {
            get
            {
                return SalePrice * (PercentageDiscount / 100);
            }
        }

        public virtual decimal CurrentSalePrice
        {
            get
            {
                return SalePrice - Discount;
            }
        }

        public virtual bool IsOnSale
        {
            get
            {
                return PercentageDiscount > 0;
            }
        }
    }
}
