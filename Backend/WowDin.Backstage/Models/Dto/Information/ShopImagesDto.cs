using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class ShopImagesDto
    {
        public string UpdateTime { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public class Image
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }
            public string ImageAlt { get; set; }
            public int Sort { get; set; }

        }

    }
}
