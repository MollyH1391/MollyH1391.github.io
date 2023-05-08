using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class ShopInfoViewModel
    {
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string PriceLimit { get; set; }
        public string HasSticker { get; set; }
        public List<ShopPaymentTypeEnum.PaymentTypeEnum> PaymentTypes { get; set; }

    }
}
