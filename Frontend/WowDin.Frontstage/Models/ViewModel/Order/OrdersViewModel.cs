using System;
using System.Collections.Generic;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.ViewModel.Order
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderDataVM> OrderListByUser { get; set; }
        public bool IsOrderExist { get; set; }

        public class OrderDataVM
        {
            public int OrderId { get; set; }
            public bool IsCommented { get; set; }
            public string OrderStateEnum { get; set; }
            public string OrderDate { get; set; }
            public ShopDataVM ShopData { get; set; }
            public class ShopDataVM
            {
                public string Brand { get; set; }
                public string BrandImg { get; set; }
                public string ShopName { get; set; }
            }
            public OrderDetailDataVM OrderDetailData { get; set; }
            public class OrderDetailDataVM
            {
                public bool IsGroupBuy { get; set; }
                public string TotalPriceByOrder { get; set; }
                public string TotalQuantityByOrder { get; set; }
            }
        }
    }
    
    
    
}
