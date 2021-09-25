using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BETECommerceAPI
{
    public class ApiErrorResp
    {
        public string Message { get; set; } = "Please correct the specified Errors and try again.";
        public IEnumerable<string> Errors { get; set; }
        public ApiErrorResp(string message)
        {
            Errors = new List<string>() { message };
        }

        public ApiErrorResp(ModelStateDictionary modelState)
        {
            Errors = modelState.Values.SelectMany(modelErrorCollection => modelErrorCollection.Errors).Select(modelError => modelError.ErrorMessage);
        }
    }
}
