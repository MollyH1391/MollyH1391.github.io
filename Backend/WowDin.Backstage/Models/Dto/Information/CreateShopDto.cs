using System;
using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class CreateShopDto
    {
        public int ShopId { get; set; }
        public int BrandId { get; set; }
        public int VerificationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OpenDayList { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal? PriceLimit { get; set; }
        public bool PreOrder { get; set; }
        public int State { get; set; }
        public bool Verified { get; set; }
        public bool HasSticker { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Star { get; set; }
        public int StarAmount { get; set; }
        public IEnumerable<ShopPaymentTypeEnum.PaymentTypeEnum> PaymentTypes { get; set; }

    }
}
