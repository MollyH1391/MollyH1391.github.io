using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class BrandImagesViewModel
    {
        public string UpdateTime { get; set; }
        public List<Image> Images { get; set; }
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
