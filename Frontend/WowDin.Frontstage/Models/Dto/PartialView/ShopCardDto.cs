using System.Collections.Generic;

namespace WowDin.Frontstage.Models.Dto.PartialView
{
    public class ShopCardDto
    {
        public int ShopId { get; set; }
        public string ShopFig { get; set; }
        public string ShopName { get; set; }
        public int BrandId { get; set; }
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public double Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Star { get; set; }
        public string Address { get; set; }
        public List<Category> Categories { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsFavorite { get; set; }

        public class Category
        {
            public int CategoryId { get; set; }
            public string CategoryFig { get; set; }
            public string CategoryName { get; set; }
        }

    }
}
