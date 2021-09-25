using System;
using System.Collections.Generic;

#nullable disable

namespace BETECommerceAPI.Data.Entities
{
    public partial class ItemDetailPicture
    {
        public Guid ItemDetailPictureId { get; set; }
        public Guid ItemDetailId { get; set; }
        public string PictureFileName { get; set; }

        public virtual ItemDetail ItemDetail { get; set; }
    }
}
