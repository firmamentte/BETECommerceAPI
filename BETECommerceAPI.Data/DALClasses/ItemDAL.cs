using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BETECommerceAPI.Data.DALClasses
{
    public class ItemDAL
    {
        public async Task<ItemDetail> GetItemByItemDetailId(BETECommerceContext dbContext, Guid itemDetailId)
        {
            return await (from itemDetail in dbContext.ItemDetails.Cast<ItemDetail>()
                          where itemDetail.ItemDetailId == itemDetailId
                          select itemDetail).
                          FirstOrDefaultAsync();
        }

        public async Task<List<ItemDetailPicture>> GetItemPicturesByItemDetailId(BETECommerceContext dbContext, Guid itemDetailId)
        {
            return await (from itemDetailPicture in dbContext.ItemDetailPictures.Cast<ItemDetailPicture>()
                          where itemDetailPicture.ItemDetailId == itemDetailId
                          select itemDetailPicture).
                          ToListAsync();
        }

        public async Task<List<ItemDetail>> GetItemsByCriteria(BETECommerceContext dbContext, string itemDescription, int skip, int take)
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
    }
}
