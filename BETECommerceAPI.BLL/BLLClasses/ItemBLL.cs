using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Data.DALClasses;
using BETECommerceAPI.Data.Entities;

namespace BETECommerceAPI.BLL.BLLClasses
{
    public class ItemBLL
    {
        public ItemDAL ItemDAL { get; set; }
        public ItemBLL()
        {
            ItemDAL = new ItemDAL();
        }

        public async Task<ItemResp> GetItemByItemDetailId(Guid itemDetailId)
        {
            try
            {
                using BETECommerceContext _dbContext = new();

                ItemDetail _itemDetail = await ItemDAL.GetItemByItemDetailId(_dbContext, itemDetailId);

                return await FillItemResp(_dbContext, _itemDetail);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public async Task<ItemPaginationResp> GetItemsByCriteria(string itemDescription, int skip, int take)
        {
            try
            {
                using BETECommerceContext _dbContext = new();

                List<ItemResp> _listItemResp = new();

                foreach (ItemDetail _itemDetail in await ItemDAL.GetItemsByCriteria(_dbContext, itemDescription, skip, take))
                {
                    _listItemResp.Add(await FillItemResp(_dbContext, _itemDetail));
                }

                return FillItemPaginationResp(_listItemResp, new PaginationMeta { OrderedBy = "CreationDate Asc", NextSkip = skip + take, Taken = take });
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        private async Task<ItemResp> FillItemResp(BETECommerceContext _dbContext, ItemDetail itemDetail)
        {
            if (itemDetail != null)
            {
                List<ItemDetailPicture> _itemDetailPictures = await ItemDAL.GetItemPicturesByItemDetailId(_dbContext, itemDetail.ItemDetailId);

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

        private ItemPaginationResp FillItemPaginationResp(List<ItemResp> itemResps, PaginationMeta meta)
        {
            itemResps ??= new List<ItemResp>();

            return new ItemPaginationResp()
            {
                Meta = meta,
                Items = itemResps
            };
        }
    }
}
