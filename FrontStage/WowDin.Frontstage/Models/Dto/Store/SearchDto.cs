using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Models.Dto.PartialView;

namespace WowDin.Frontstage.Models.Dto.Store
{
    public class SearchDto
    {

        public List<string> Brands { get; set; }
        public List<Category> Categories { get; set; }

        public class Category
        {
            public string Name { get; set; }
            public string Img { get; set; }

        }

    }
}
