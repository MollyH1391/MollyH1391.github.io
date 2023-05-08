using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class ShopManagementViewModel
    {
        public string BrandName { get; set; }
        public int ShopId { get; set; }
        public List<Shop> Shops { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<CityDistrictViewModel> CityDistrict { get; set; }
        public List<Day> DayList { get; set; }
        public List<State> States { get; set; }
        public ShopInfoViewModel ShopInfo { get; set; }
        public ShopBusinessViewModel ShopBusiness { get; set; }
        public ShopTakeMethodViewModel ShopTakeMethod { get; set; }
        public class Shop
        {
            public int value { get; set; }
            public string text { get; set; }
        }
        public class Day
        {
            public string value { get; set; }
            public string text { get; set; }
        }

        public class State
        {
            public ShopEnum.StateEnum value { get; set; }
            public string text { set; get; }
        }
        public class PaymentType
        {
            public ShopPaymentTypeEnum.PaymentTypeEnum value { get; set; }
            public string text { set; get; }
        }
    }
}
