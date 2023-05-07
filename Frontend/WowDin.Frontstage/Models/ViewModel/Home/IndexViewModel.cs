using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.ViewModel.PartialView;
using WowDin.Frontstage.Models.ViewModel.Store;

namespace WowDin.Frontstage.Models.ViewModel.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Picture> BigPictures { get; set; }
        public IEnumerable<Picture> Smallpictures { get; set; }
        public IEnumerable<ShopCardViewModel> Shops { get; set; }
        public IEnumerable<BrandList> BrandLists { get; set; }
    }

    public class BrandList
    {
        public int BrandId { get; set; }
        public string Logo { get; set; }
    }
    public class Picture
    {
        public string PictureFile { get; set; }
        public int BrandId { get; set; }
        public string Code { get; set; }
    }
}
