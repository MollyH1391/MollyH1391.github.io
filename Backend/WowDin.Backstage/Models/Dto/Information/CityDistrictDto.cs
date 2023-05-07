using System.Collections.Generic;
namespace WowDin.Backstage.Models.Dto.Information
{
    public class CityDistrictDto
    {
        public string City { get; set; }
        public IEnumerable<string> District { get; set; }
    }
}
