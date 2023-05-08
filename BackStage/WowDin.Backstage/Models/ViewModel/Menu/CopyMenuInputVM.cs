using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Menu
{
    public class CopyMenuInputVM
    {
        public IEnumerable<MenuClassVM> Source { get; set; }
        public IEnumerable<int> Targets { get; set; }
    }
}
