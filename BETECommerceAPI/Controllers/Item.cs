using System;
using System.Threading.Tasks;
using BETECommerceAPI.BLL;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BETECommerceAPI.Controllers
{
    [Route("api/Item")]
    [ApiController]
    [AuthenticateAccessToken]
    public class Item : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Item(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("V1/GetItemByItemDetailId")]
        [HttpGet]
        public async Task<ActionResult> GetItemByItemDetailId([FromQuery] Guid itemDetailId)
        {
            try
            {
                #region RequestValidation

                ModelState.Clear();

                if (itemDetailId == Guid.Empty)
                {
                    ModelState.AddModelError("ItemDetailId", "Item Detail Id must be a globally unique identifier and not empty");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiErrorResp(ModelState));
                }

                #endregion

                ItemResp _itemResp = await BETECommerceAPIBLL.ItemHelper.GetItemByItemDetailId(itemDetailId);

                await ControllerHelper.ItemHelper.GetItemPicturesBase64String(_webHostEnvironment, _itemResp);

                return Ok(_itemResp);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        [Route("V1/GetItemsByCriteria")]
        [HttpGet]
        public async Task<ActionResult> GetItemsByCriteria([FromQuery] string itemDescription, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            try
            {
                #region RequestValidation

                #endregion

                ItemPaginationResp _itemPaginationResp = await BETECommerceAPIBLL.ItemHelper.GetItemsByCriteria(itemDescription, skip, take);

                await ControllerHelper.ItemHelper.GetItemPicturesBase64String(_webHostEnvironment, _itemPaginationResp.Items);

                return Ok(_itemPaginationResp);
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }
    }
}
