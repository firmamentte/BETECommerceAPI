using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class PurchaseOrder
    {
        public virtual decimal SubTotal
        {
            get
            {
                decimal _total = decimal.Zero;

                foreach (PurchaseOrderItemDetail _lineItem in PurchaseOrderItemDetails)
                {
                    _total += _lineItem.SubTotal;
                }

                return _total;
            }
        }

        public virtual decimal AmountDue
        {
            get
            {
                return SubTotal;
            }
        }
    }
}
