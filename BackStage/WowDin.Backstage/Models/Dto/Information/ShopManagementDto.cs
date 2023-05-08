using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class ShopManagementDto
    {
        public string BrandName { get; set; }
        public int ShopId { get; set; }
        public IEnumerable<Shop> Shops { get; set; }
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<CityDistrictDto> CityDistrict { get; set; }
        public IEnumerable<Day> DayList { get; set; }
        public IEnumerable<State> States { get; set; }
        public ShopInfoDto ShopInfo { get; set; }
        public ShopBusinessDto ShopBusiness { get; set; }
        public ShopTakeMethodDto ShopTakeMethod { get; set; }
        public ShopImagesDto ShopImages { get; set; }
        public class Shop
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class Day
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        public class State
        {
            public ShopEnum.StateEnum Id { get; set; }
            public string Name { set; get; }
        }
        public class PaymentType
        {
            public ShopPaymentTypeEnum.PaymentTypeEnum Id { get; set; }
            public string Name { set; get; }
        }
    }
}
