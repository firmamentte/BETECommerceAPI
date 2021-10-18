using System;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.BLLClasses;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Controllers.ControllerHelpers;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BETECommerceAPI.Controllers
{
    [Route("api/Item")]
    [ApiController]
    [AuthenticateAccessToken]
    public class ItemController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ItemControllerHelper ItemControllerHelper { get; set; }
        private ItemBLL ItemBLL { get; set; }

        public ItemController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            ItemBLL = new();
            ItemControllerHelper = new();
        }

        [Route("V1/GetItemByItemDetailId")]
        [HttpGet]
        public async Task<ActionResult> GetItemByItemDetailId([FromQuery] Guid itemDetailId)
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

            ItemResp _itemResp = await ItemBLL.GetItemByItemDetailId(itemDetailId);

            await ItemControllerHelper.GetItemPicturesBase64String(_webHostEnvironment, _itemResp);

            return Ok(_itemResp);
        }

        [Route("V1/GetItemsByCriteria")]
        [HttpGet]
        public async Task<ActionResult> GetItemsByCriteria([FromQuery] string itemDescription, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            #region RequestValidation

            #endregion

            ItemPaginationResp _itemPaginationResp = await ItemBLL.GetItemsByCriteria(itemDescription, skip, take);

            await ItemControllerHelper.GetItemPicturesBase64String(_webHostEnvironment, _itemPaginationResp.Items);

            return Ok(_itemPaginationResp);
        }
    }
}
