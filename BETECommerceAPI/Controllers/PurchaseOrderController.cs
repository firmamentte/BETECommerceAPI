using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.BLL.BLLClasses;
using BETECommerceAPI.BLL.DataContract;
using BETECommerceAPI.Controllers.ControllerHelpers;
using BETECommerceAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BETECommerceAPI.Controllers
{
    [Route("api/PurchaseOrder")]
    [ApiController]
    [AuthenticateAccessToken]
    public class PurchaseOrderController : ControllerBase
    {
        private PurchaseOrderBLL PurchaseOrderBLL { get; set; }
        private ControllerHelper ControllerHelper { get; set; }

        public PurchaseOrderController() 
        {
            PurchaseOrderBLL = new();
            ControllerHelper = new();
        }

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

            return Created(string.Empty, await PurchaseOrderBLL.CreatePurchaseOrder(ControllerHelper.GetHeaderUsername(Request), lineItems));
        }
    }
}
