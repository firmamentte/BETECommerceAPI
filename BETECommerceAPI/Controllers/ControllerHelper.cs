using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BETECommerceAPI.BLL;
using BETECommerceAPI.BLL.DataContract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace BETECommerceAPI.Controllers
{
    public static class ControllerHelper
    {
        public static string GetEmailAddress(HttpRequest request)
        {
            try
            {
                if (request.Headers.TryGetValue("EmailAddress", out StringValues _username))
                    return _username.FirstOrDefault();
                else
                    return null;
            }
            catch (BETECommerceAPIException)
            {
                throw;
            }
        }

        public static class ItemHelper
        {
            public static async Task<string> GetItemPictureBase64String(IWebHostEnvironment webHostEnvironment, string pictureName)
            {
                try
                {
                    string _path = Path.Combine(webHostEnvironment.ContentRootPath, "Content", "ItemPictures", pictureName);

                    return File.Exists(_path) ? Convert.ToBase64String(await File.ReadAllBytesAsync(_path)) : null;
                }
                catch (BETECommerceAPIException)
                {
                    throw;
                }
            }

            public static async Task GetItemPicturesBase64String(IWebHostEnvironment webHostEnvironment, ItemResp itemResp)
            {
                try
                {
                    if (itemResp != null)
                    {
                        itemResp.ItemPicture.PictureBase64String = await GetItemPictureBase64String(webHostEnvironment, itemResp.ItemPicture.PictureFileName);
                    }
                }
                catch (BETECommerceAPIException)
                {
                    throw;
                }
            }

            public static async Task GetItemPicturesBase64String(IWebHostEnvironment webHostEnvironment, List<ItemResp> itemResps)
            {
                try
                {
                    foreach (var itemResp in itemResps)
                    {
                        await GetItemPicturesBase64String(webHostEnvironment, itemResp);
                    }
                }
                catch (BETECommerceAPIException)
                {
                    throw;
                }
            }
        }
    }
}
