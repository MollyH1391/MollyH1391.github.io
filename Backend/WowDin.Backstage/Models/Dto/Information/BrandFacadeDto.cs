using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class BrandFacadeDto
    {
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Slogan { get; set; }
        public string CardImgUrl { get; set; }
        public IEnumerable<BrandManagementDto.Category> BrandCategories { get; set; }

    }
}
