using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class BrandManagementDto
    {
        public IEnumerable<Category> Categories { get; set; }
        public string Star { get; set; }

        public class Category
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Image { get; set; }
        }

    }
}
