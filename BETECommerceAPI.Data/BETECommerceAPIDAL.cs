using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETECommerceAPI.Data
{
    public static class BETECommerceAPIDAL
    {
        #region ApplicationUserHelper

        //If change this function also change IsUsernameExisting function
        public static async Task<ApplicationUser> GetApplicationUserByUsername(BETECommerceContext dbContext, string username)
        {
            try
            {
                return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                              where applicationUser.Username == username
                              select applicationUser).
                              FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<ApplicationUser> GetApplicationUserByUsernameAndUserPassword(BETECommerceContext dbContext, string username, string userPassword)
        {
            try
            {
                return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                              where applicationUser.Username == username &&
                                    applicationUser.UserPassword == userPassword
                              select applicationUser).
                              FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //If change this function also change GetApplicationUserByUsername function
        public static async Task<bool> IsUsernameExisting(BETECommerceContext dbContext, string username)
        {
            try
            {
                return await (from applicationUser in dbContext.ApplicationUsers.Cast<ApplicationUser>()
                              where applicationUser.Username == username
                              select applicationUser).
                              AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ItemHelper

        public static async Task<ItemDetail> GetItemByItemDetailId(BETECommerceContext dbContext, Guid itemDetailId)
        {
            try
            {
                return await (from itemDetail in dbContext.ItemDetails.Cast<ItemDetail>()
                              where itemDetail.ItemDetailId == itemDetailId
                              select itemDetail).
                              FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<ItemDetailPicture>> GetItemPicturesByItemDetailId(BETECommerceContext dbContext, Guid itemDetailId)
        {
            try
            {
                return await (from itemDetailPicture in dbContext.ItemDetailPictures.Cast<ItemDetailPicture>()
                              where itemDetailPicture.ItemDetailId == itemDetailId
                              select itemDetailPicture).
                              ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<ItemDetail>> GetItemsByCriteria(BETECommerceContext dbContext, string itemDescription, int skip, int take)
        {
            try
            {
                itemDescription ??= string.Empty;

                return await (from itemDetail in dbContext.ItemDetails.Cast<ItemDetail>()
                              where itemDetail.ItemDescription.Contains(itemDescription)
                              select itemDetail).
                              OrderBy(itemDetail => itemDetail.CreationDate).
                              Skip(skip).
                              Take(take).
                              ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region PurchaseOrderHelper

        public static async Task<bool> IsPurchaseOrderNumberExisting(BETECommerceContext dbContext, string purchaseOrderNumber)
        {
            try
            {
                return await (from purchaseOrder in dbContext.PurchaseOrders.Cast<PurchaseOrder>()
                              where purchaseOrder.PurchaseOrderNumber == purchaseOrderNumber
                              select purchaseOrder).
                              AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
