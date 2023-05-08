using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class BrandManagementViewModel
    {
        public BrandFacadeViewModel BrandFacade { get; set; }
        public BrandIntroduceViewModel BrandIntroduce { get; set; }
        public BrandWebViewModel BrandWeb { get; set; }
        public BrandImagesViewModel BrandImages { get; set; }
        public ShopImagesViewModel ShopImages { get; set; }

        public string Star { get; set; }
        public List<CategoryOp> Categories { get; set; }

        public class CategoryOp
        {
            public string text { get; set; }
            public Category value { get; set; }
            public bool disabled { get; set; }
        }
        public class Category
        {
            public int Id { get; set; }
            public string Image { get; set; }
        }

    }
}
