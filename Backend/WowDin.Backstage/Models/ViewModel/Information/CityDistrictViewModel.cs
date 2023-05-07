using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class CityDistrictViewModel
    {
        public string City { get; set; }
        public IEnumerable<string> District { get; set; }

    }
}
