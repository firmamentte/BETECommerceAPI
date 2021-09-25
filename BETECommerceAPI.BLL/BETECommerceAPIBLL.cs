using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Data;
using BETECommerceAPI.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace BETECommerceAPI.BLL
{
    public static class BETECommerceAPIBLL
    {
        private static string AccessToken { get; set; }

        private static void RaiseServerError(string errorMessage)
        {
            try
            {
                throw new BETECommerceAPIException(errorMessage);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public static void InitializeAppSettings(IConfiguration configuration)
        {
            try
            {
                if (configuration != null)
                {
                    if (string.IsNullOrWhiteSpace(FirmamentUtilities.Utilities.DatabaseHelper.ConnectionString))
                    {
                        FirmamentUtilities.Utilities.DatabaseHelper.ConnectionString = configuration["ConnectionStrings:DatabasePath"];
                        AccessToken = configuration["ApiKeys:AccessToken"];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            return FirmamentUtilities.Utilities.GetEnumDescription(enumValue);
        }

        public static bool IsEmailAddress(string emailAddress)
        {
            try
            {
                return FirmamentUtilities.Utilities.EmailHelper.IsEmailAddress(emailAddress);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public static class ApplicationUserHelper
        {
            public static void AuthenticateAccessToken(string accessToken)
            {
                try
                {
                    if (AccessToken != accessToken)
                    {
                        RaiseServerError("Request Forbidden");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static bool IsAccessTokenValid(string accessToken)
            {
                try
                {
                    return AccessToken == accessToken;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static async Task SignUp(string emailAddress, string userPassword)
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    if (await BETECommerceAPIDAL.IsUsernameExisting(_dbContext, emailAddress))
                    {
                        RaiseServerError("Email Address already existing");
                    }

                    ApplicationUser _applicationUser = new()
                    {
                        ApplicationUserId = Guid.NewGuid(),
                        Username = emailAddress,
                        UserPassword = userPassword
                    };

                    await _dbContext.AddAsync(_applicationUser);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static async Task SignIn(string emailAddress, string userPassword)
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    ApplicationUser _applicationUser = await BETECommerceAPIDAL.GetApplicationUserByUsernameAndUserPassword(_dbContext, emailAddress, userPassword);

                    if (_applicationUser is null)
                    {
                        RaiseServerError("Invalid Username or Password");
                    }

                    if (!string.Equals(_applicationUser.Username, emailAddress, StringComparison.CurrentCulture))
                    {
                        RaiseServerError("Invalid Username or Password");
                    }

                    if (!string.Equals(_applicationUser.UserPassword, userPassword, StringComparison.CurrentCulture))
                    {
                        RaiseServerError("Invalid Username or Password");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        public static class ItemHelper
        {
            public static async Task<ItemResp> GetItemByItemDetailId(Guid itemDetailId)
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    ItemDetail _itemDetail = await BETECommerceAPIDAL.GetItemByItemDetailId(_dbContext, itemDetailId);

                    return await FillItemResp(_dbContext, _itemDetail);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public static async Task<ItemPaginationResp> GetItemsByCriteria(string itemDescription, int skip, int take)
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    List<ItemResp> _listItemResp = new();

                    foreach (ItemDetail _itemDetail in await BETECommerceAPIDAL.GetItemsByCriteria(_dbContext, itemDescription, skip, take))
                    {
                        _listItemResp.Add(await FillItemResp(_dbContext, _itemDetail));
                    }

                    return FillItemPaginationResp(_listItemResp, new PaginationMeta { OrderedBy = "CreationDate Asc", NextSkip = skip + take, Taken = take });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private static async Task<ItemResp> FillItemResp(BETECommerceContext _dbContext, ItemDetail itemDetail)
            {
                try
                {
                    if (itemDetail != null)
                    {
                        List<ItemDetailPicture> _itemDetailPictures = await BETECommerceAPIDAL.GetItemPicturesByItemDetailId(_dbContext, itemDetail.ItemDetailId);

                        ItemResp _itemResp = new()
                        {
                            ItemDetailId = itemDetail.ItemDetailId,
                            ItemDescription = itemDetail.ItemDescription,
                            ItemPicture = new ItemPictureResp() { PictureFileName = _itemDetailPictures.FirstOrDefault()?.PictureFileName },
                            SalePrice = itemDetail.SalePrice,
                            CurrentSalePrice = itemDetail.CurrentSalePrice,
                            PercentageDiscount = itemDetail.PercentageDiscount,
                            IsOnSale = itemDetail.IsOnSale,
                            CreationDate = itemDetail.CreationDate
                        };

                        return _itemResp;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private static ItemPaginationResp FillItemPaginationResp(List<ItemResp> itemResps, PaginationMeta meta)
            {
                try
                {
                    itemResps ??= new List<ItemResp>();

                    return new ItemPaginationResp()
                    {
                        Meta = meta,
                        Items = itemResps
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static class PurchaseOrderHelper
        {
            public static async Task<PurchaseOrderResp> CreatePurchaseOrder(string username, List<LineItemReq> lineItems)
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    Guid _purchaseOrderId = Guid.NewGuid();

                    PurchaseOrder _purchaseOrder = new()
                    {
                        PurchaseOrderId = _purchaseOrderId,
                        ApplicationUser = await BETECommerceAPIDAL.GetApplicationUserByUsername(_dbContext, username),
                        PurchaseOrderNumber = await CreatePurchaseOrderNumber(),
                        PaymentStatus = GetEnumDescription(BETECommerceAPIEnum.Status.Pending),
                        ShippingStatus = GetEnumDescription(BETECommerceAPIEnum.Status.Pending),
                        CreationDate = DateTime.Now.Date
                    };

                    List<PurchaseOrderItemDetail> _purchaseOrderItemDetails = new();

                    foreach (LineItemReq _lineItemReq in lineItems)
                    {
                        ItemDetail _itemDetail = await BETECommerceAPIDAL.GetItemByItemDetailId(_dbContext, _lineItemReq.ItemDetailId);

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
                catch (Exception)
                {
                    throw;
                }
            }

            private static async Task<string> CreatePurchaseOrderNumber()
            {
                try
                {
                    using BETECommerceContext _dbContext = new();

                    string _purchaseOrderNumber;

                    Random _random = new();

                    _purchaseOrderNumber = _random.Next(10000, 2000000000).ToString();

                    while (await BETECommerceAPIDAL.IsPurchaseOrderNumberExisting(_dbContext, _purchaseOrderNumber))
                    {
                        _purchaseOrderNumber = _random.Next(10000, 2000000000).ToString();
                    }

                    return _purchaseOrderNumber;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private static PurchaseOrderResp FillPurchaseOrderResp(PurchaseOrder purchaseOrder)
            {
                try
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
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
