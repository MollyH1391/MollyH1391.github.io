using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Home
{
    public class GetChartDataByIdDto
    {
        public string TodayTotalAmount { get; set; }
        public string TodayOrders { get; set; }
        public string YearTotalAmount { get; set; }
        public string YearOrders { get; set; }
        public int DayCount { get; set; }
        public List<AmountMonthList> AmountMonth { get; set; }
        public List<AmountDayList> AmountDay { get; set; }
        public List<AmountWeekList> AmountWeek { get; set; }
        public List<QuantityMonthDataDto> QuantityMonthDataList { get; set; }
        public List<QuantityDayDataDto> QuantityDayDataList { get; set; }
        public List<QuantityWeekDataDto> QuantityWeekDataList { get; set; }
        public List<QuantityTodayDataDto> QuantityTodayDataList { get; set; }

        public class WeekArray
        {
            public DayOfWeek Week { get; set; }
            public int Day { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class AmountData
        {
            public int OrderId { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class AmountDataByOrderDto
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class AmountMonthDataDto
        {
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class AmountDayDataDto
        {
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class AmountMonthList
        {
            public string Month { get; set; }
            public string TotalPrice { get; set; }
        }
        public class AmountDayList
        {
            public string Day { get; set; }
            public string TotalPrice { get; set; }
        }
        public class AmountWeekList
        {
            public string Week { get; set; }
            public string TotalPrice { get; set; }
        }
        public class MonthQuantityByOrderDetail
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class QuantityMonthDataDto
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class DayQuantityByOrderDetail
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class QuantityDayDataDto
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class WeekQuantityByOrderDetail
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class QuantityWeekDataDto
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class TodayQuantityByOrderDetail
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class QuantityTodayDataDto
        {
            public string ProductName { get; set; }
            public int TotalQuantity { get; set; }
        }
    }
}
