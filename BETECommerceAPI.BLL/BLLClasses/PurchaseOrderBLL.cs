using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Data.DALClasses;
using BETECommerceAPI.Data.Entities;

namespace BETECommerceAPI.BLL.BLLClasses
{
    public class PurchaseOrderBLL : BETECommerceBLLHelper
    {
        public ApplicationUserDAL ApplicationUserDAL { get; set; }
        public PurchaseOrderDAL PurchaseOrderDAL { get; set; }
        public ItemDAL ItemDAL { get; set; }

        public PurchaseOrderBLL()
        {
            ApplicationUserDAL = new();
            PurchaseOrderDAL = new();
            ItemDAL = new();
        }

        public async Task<PurchaseOrderResp> CreatePurchaseOrder(string username, List<LineItemReq> lineItems)
        {
            try
            {
                using BETECommerceContext _dbContext = new();

                Guid _purchaseOrderId = Guid.NewGuid();

                PurchaseOrder _purchaseOrder = new()
                {
                    PurchaseOrderId = _purchaseOrderId,
                    ApplicationUser = await ApplicationUserDAL.GetApplicationUserByUsername(_dbContext, username),
                    PurchaseOrderNumber = await CreatePurchaseOrderNumber(),
                    PaymentStatus = GetEnumDescription(BETECommerceAPIEnum.Status.Pending),
                    ShippingStatus = GetEnumDescription(BETECommerceAPIEnum.Status.Pending),
                    CreationDate = DateTime.Now.Date
                };

                List<PurchaseOrderItemDetail> _purchaseOrderItemDetails = new();

                foreach (LineItemReq _lineItemReq in lineItems)
                {
                    ItemDetail _itemDetail = await ItemDAL.GetItemByItemDetailId(_dbContext, _lineItemReq.ItemDetailId);

                    if (_itemDetail is null)
                    {
                        RaiseServerError("Invalid Item Detail Id. The resource has been removed, had its name changed, or is unavailable.");
                    }

                    _purchaseOrderItemDetails.Add(new PurchaseOrderItemDetail()
                    {
                        PurchaseOrderItemDetailId = Guid.NewGuid(),
                        PurchaseOrderId = _purchaseOrderId,
                        ItemDetailId = _itemDetail.ItemDetailId,
                        PictureFileName = _lineItemReq.PictureFileName,
                        Quantity = _lineItemReq.Quantity,
                        PercentageDiscount = _lineItemReq.PercentageDiscount,
                        SalePrice = _lineItemReq.SalePrice,
                        PurchaseOrder = _purchaseOrder,
                        CreationDate = _purchaseOrder.CreationDate
                    });
                }

                await _dbContext.AddAsync(_purchaseOrder);
                await _dbContext.AddRangeAsync(_purchaseOrderItemDetails);
                await _dbContext.SaveChangesAsync();

                return FillPurchaseOrderResp(_purchaseOrder);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        private async Task<string> CreatePurchaseOrderNumber()
        {
            using BETECommerceContext _dbContext = new();

            string _purchaseOrderNumber;

            Random _random = new();

            _purchaseOrderNumber = _random.Next(10000, 2000000000).ToString();

            while (await PurchaseOrderDAL.IsPurchaseOrderNumberExisting(_dbContext, _purchaseOrderNumber))
            {
                _purchaseOrderNumber = _random.Next(10000, 2000000000).ToString();
            }

            return _purchaseOrderNumber;
        }

        private PurchaseOrderResp FillPurchaseOrderResp(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder != null)
            {
                return new PurchaseOrderResp()
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                    PaymentStatus = purchaseOrder.PaymentStatus,
                    ShippingStatus = purchaseOrder.ShippingStatus,
                    AmountDue = purchaseOrder.AmountDue,
                    CreationDate = purchaseOrder.CreationDate
                };
            }
            else
            {
                return null;
            }
        }
    }
}
