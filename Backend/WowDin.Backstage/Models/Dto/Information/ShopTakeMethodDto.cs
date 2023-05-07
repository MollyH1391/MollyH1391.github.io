using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class ShopTakeMethodDto
    {
        public string UpdateTime { get; set; }
        public bool IsTakeOut { get; set; }
        public bool IsDelivery { get; set; }
        public double TakeOutTime { get; set; }
        public bool PreOrder { get; set; }

        public IEnumerable<DeliveryCondition> DeliveryConditions { get; set; }
        public class DeliveryCondition
        {
            public int Id { get; set; }
            public double DeliveryTime { get; set; }
            public decimal PriceThreshold { get; set; }
            public double LowerDistance { get; set; }
            public double? HigherDistance { get; set; }
            public decimal DeliveryFee { get; set; }

        }
    }
}
