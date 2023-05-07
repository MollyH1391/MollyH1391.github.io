using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class ShopInfoDto
    {
        public string UpdateTime { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public decimal? PriceLimit { get; set; }
        public bool HasSticker { get; set; }
        public IEnumerable<ShopPaymentTypeEnum.PaymentTypeEnum> PaymentTypes { get; set; }

    }
}
