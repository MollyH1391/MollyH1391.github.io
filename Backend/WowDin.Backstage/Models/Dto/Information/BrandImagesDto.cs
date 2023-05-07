using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class BrandImagesDto
    {
        public string UpdateTime { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public class Image
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }
            public string ImageAlt { get; set; }
            public int Sort { get; set; }
            //public bool TurnHome { get; set; }

        }
    }
}
