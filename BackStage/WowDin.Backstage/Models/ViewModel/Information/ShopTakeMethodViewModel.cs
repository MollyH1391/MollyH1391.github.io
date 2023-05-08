using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class ShopTakeMethodViewModel
    {
        public string UpdateTime { get; set; }
        public bool IsTakeOut { get; set; }
        public bool IsDelivery { get; set; }
        public string TakeOutTime { get; set; }
        public bool PreOrder { get; set; }

        public List<DeliveryCondition> DeliveryConditions { get; set; }
        public class DeliveryCondition
        {
            public int Id { get; set; }
            public string DeliveryTime { get; set; }
            public string PriceThreshold { get; set; }
            public string LowerDistance { get; set; }
            public string HigherDistance { get; set; }
            public string DeliveryFee { get; set; }
            public bool _showDetails { get; set; }

        }

    }
}
