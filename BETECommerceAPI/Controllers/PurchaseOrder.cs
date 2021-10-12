using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.BLL;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BETECommerceAPI.Controllers
{
    [Route("api/PurchaseOrder")]
    [ApiController]
    [AuthenticateAccessToken]
    public class PurchaseOrder : ControllerBase
    {
        [Route("V1/CreatePurchaseOrder")]
        [HttpPost]
        public async Task<ActionResult> CreatePurchaseOrder([FromBody] List<LineItemReq> lineItems)
        {

            #region RequestValidation

            ModelState.Clear();

            if (lineItems is null)
            {
                ModelState.AddModelError("LineItems", "Line Items request can not be null");
            }
            else
            {
                if (!lineItems.Any())
                {
                    ModelState.AddModelError("EmptyLineItems", "At least one Line Item must be supplied");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResp(ModelState));
            }

            #endregion

            return Created(string.Empty, await BETECommerceAPIBLL.PurchaseOrderHelper.CreatePurchaseOrder(ControllerHelper.GetEmailAddress(Request), lineItems));
        }
    }
}
