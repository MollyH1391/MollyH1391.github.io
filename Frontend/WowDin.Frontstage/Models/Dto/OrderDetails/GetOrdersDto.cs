using System;
using System.Collections.Generic;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class GetOrdersDto
    {
        public IEnumerable<OrderDataDto> OrderListByUser { get; set; }
        public bool IsOrderExist { get; set; }

    }
    public class OrderDataDto
    {
        public int OrderId { get; set; }
        public bool IsCommented { get; set; }
        public OrderStateEnum OrderStateEnum { get; set; }
        public string OrderDate { get; set; }
        public ShopDataDto ShopData { get; set; }
        public OrderDetailDataDto OrderDetailData { get; set;}
    }

    public class ShopDataDto
    {
        public string Brand { get; set; }
        public string BrandImg { get; set; }
        public string ShopName { get; set; }
    }

    public class OrderDetailDataDto
    {
        public bool IsGroupBuy { get; set; }
        public decimal TotalPriceByOrder { get; set; }
        public int TotalQuantityByOrder { get; set; }
    }
}
