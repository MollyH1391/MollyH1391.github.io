using System;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Home
{
    public class IndexViewModel
    {
        public string TodayTotalAmount { get; set; }
        public string TodayOrders { get; set; }
        public int DayCount { get; set; }
        public string[] Month { get; set; }
        public string[] MonthTotalPrice { get; set; }
        public string[] Day { get; set; }
        public string[] DayTotalPrice { get; set; }
        public string[] Week { get; set; }
        public string[] WeekTotalPrice { get; set; }
        public string[] MonthProductName { get; set; }
        public int[] MonthTotalQuantity { get; set; }
        public string[] DayProductName { get; set; }
        public int[] DayTotalQuantity { get; set; }
        public string[] WeekProductName { get; set; }
        public int[] WeekTotalQuantity { get; set; }

    }
}
