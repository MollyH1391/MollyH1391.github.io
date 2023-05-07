using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Backstage.Models.Dto.Home;
using WowDin.Backstage.Models.Dto.Member;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using static WowDin.Backstage.Models.Dto.Home.GetChartDataByIdDto;

namespace WowDin.Backstage.Services
{
    public class HomeService : IHomeService
    {
        private readonly IRepository _repository;
        public HomeService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<GetCommentDataDto> GetCommentData(int brandId)
        {
            var result = _repository.GetAll<Comment>().Select(x => new GetCommentDataDto()
            {
                BrandName = _repository._context.Brands.FirstOrDefault(y => y.BrandId == x.BrandId).Name,
                //ShopName = x.Shop.Name,
                ShopName = _repository._context.Shops.FirstOrDefault(y => y.BrandId == x.BrandId).Name,
                Star = x.Star,
                UserName = _repository._context.UserAccounts.FirstOrDefault(y => y.UserAccountId == x.UserAccountId).RealName,
                Comment = x.Comment1,
                Date = x.Date
            }) ;
            return result;
        }

        public GetChartDataByIdDto GetChartDataById(int brandId)
        {
            var dateNow = DateTime.UtcNow.TransferToTaipeiTime();
            var year = dateNow.Year;
            var month = dateNow.Month;
            var day = dateNow.Day;

            DateTime firstday;
            if (dateNow.DayOfWeek == 0)
            {
                firstday = dateNow.AddDays(1 - (int)dateNow.DayOfWeek - 7);  //ex:5月:15(1-0-7) = 9號
            }
            else
            {
                firstday = dateNow.AddDays(1 - (int)dateNow.DayOfWeek);
            }
            var dayCount = DateTime.DaysInMonth(year, month);
            List<WeekArray> weekList = new List<WeekArray>() {
                new WeekArray() { Day = firstday.AddDays(0).Day, Week = firstday.AddDays(0).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(1).Day, Week = firstday.AddDays(1).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(2).Day, Week = firstday.AddDays(2).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(3).Day, Week = firstday.AddDays(3).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(4).Day, Week = firstday.AddDays(4).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(5).Day, Week = firstday.AddDays(5).DayOfWeek, TotalPrice = 0},
                new WeekArray() { Day = firstday.AddDays(6).Day, Week = firstday.AddDays(6).DayOfWeek, TotalPrice = 0},
            };

            var brand = _repository.GetAll<Brand>().FirstOrDefault(x => x.BrandId == brandId);
            var shop = _repository.GetAll<Shop>().Where(x => x.BrandId == brandId);

            var order = _repository.GetAll<Order>().Where(x => x.OrderState == 7 && shop.Select(s => s.ShopId).Contains(x.ShopId)).ToList();

            var orderdetail = _repository.GetAll<OrderDetail>().Where(x => order.Select(o => o.OrderId).Contains(x.OrderId)).ToList();

            #region For TotalQuantity
            var monthOrder = order.Where(x => x.OrderDate.Year == year);
            var monthOrderdetail = _repository.GetAll<OrderDetail>().Where(x => monthOrder.Select(o => o.OrderId).Contains(x.OrderId)).ToList();

            var dayOrder = monthOrder.Where(x => x.OrderDate.Month == month);
            var dayOrderdetail = _repository.GetAll<OrderDetail>().Where(x => dayOrder.Select(o => o.OrderId).Contains(x.OrderId)).ToList();

            var weekOrder = dayOrder.Where(x => weekList.Select(od => od.Day).Contains(x.OrderDate.Day)).ToList();
            var weekOrderdetail = _repository.GetAll<OrderDetail>().Where(x => weekOrder.Select(o => o.OrderId).Contains(x.OrderId)).ToList();

            var todayOrder = dayOrder.Where(x => x.OrderDate.Day == day);
            var todayOrderdetail = _repository.GetAll<OrderDetail>().Where(x => todayOrder.Select(o => o.OrderId).Contains(x.OrderId)).ToList();
            #endregion

            #region Quantity: orderdetail的總數量
            //每月orderdetail的總數量
            var monthQuantityDataByOrderDetail = monthOrderdetail.Select(x => new GetChartDataByIdDto.MonthQuantityByOrderDetail
            {
                ProductName = x.ProductName,
                TotalQuantity = x.Quantity
            });

            //每日orderdetail的總數量
            var dayQuantityDataByOrderDetail = dayOrderdetail.Select(x => new GetChartDataByIdDto.DayQuantityByOrderDetail
            {
                ProductName = x.ProductName,
                TotalQuantity = x.Quantity
            });

            //每週orderdetail的總數量
            var weekQuantityDataByOrderDetail = weekOrderdetail.Select(x => new GetChartDataByIdDto.WeekQuantityByOrderDetail
            {
                ProductName = x.ProductName,
                TotalQuantity = x.Quantity
            });

            //今天orderdetail的總數量
            var todayQuantityDataByOrderDetail = todayOrderdetail.Select(x => new GetChartDataByIdDto.TodayQuantityByOrderDetail
            {
                ProductName= x.ProductName,
                TotalQuantity= x.Quantity
            });

            #endregion

            #region Quantity: 用商品名稱分類，取總數量

            var monthOrderdetailByProductName = monthQuantityDataByOrderDetail.GroupBy(x => x.ProductName);
            var dayOrderdetailByProductName = dayQuantityDataByOrderDetail.GroupBy(x => x.ProductName);
            var weekOrderdetailByProductName = weekQuantityDataByOrderDetail.GroupBy(x => x.ProductName);
            var todayOrderdetailByProductName = todayQuantityDataByOrderDetail.GroupBy(x => x.ProductName);

            //用商品名稱分類，取同一年/每月總數量
            var quantityMonthList = monthOrderdetailByProductName.Select(x => new GetChartDataByIdDto.QuantityMonthDataDto
            {
                ProductName = x.Key,
                TotalQuantity = x.Sum(x => x.TotalQuantity),
            });

            //用商品名稱分類，取同一月/每日總數量
            var quantityDayList = dayOrderdetailByProductName.Select(x => new GetChartDataByIdDto.QuantityDayDataDto
            {
                ProductName = x.Key,
                TotalQuantity = x.Sum(x => x.TotalQuantity),
            });

            //用商品名稱分類，取同一週/每日總數量
            var quantityWeekList = weekOrderdetailByProductName.Select(x => new GetChartDataByIdDto.QuantityWeekDataDto
            {
                ProductName = x.Key,
                TotalQuantity = x.Sum(x => x.TotalQuantity),
            });

            //用商品名稱分類，取今天總數量
            var quantityTodayList = todayOrderdetailByProductName.Select(x => new GetChartDataByIdDto.QuantityTodayDataDto
            {
                ProductName = x.Key,
                TotalQuantity = x.Sum(x => x.TotalQuantity),
            });

            #endregion

            #region Amount: 先取得orderdetail的總價，再以orderId分群
            //每一筆orderdetail的總價
            var chartDataByOrderDetail = orderdetail.Select(x => new GetChartDataByIdDto.AmountData
            {
                OrderId = x.OrderId,
                TotalPrice = x.UnitPrice * x.Quantity,
            });
           

            var orderdetailByOrderId = chartDataByOrderDetail.GroupBy(x => x.OrderId);
            var getOrderDateForAmount = _repository.GetAll<Order>().Where(x => orderdetailByOrderId.Select(od => od.Key).Contains(x.OrderId)).ToList();
           
            //把屬於同一筆orderId的orderdetail總價加總，取每一筆orderId的總價
            var oddataByOrderId = orderdetailByOrderId.Select(x => new GetChartDataByIdDto.AmountDataByOrderDto
            {
                OrderId = x.Key,
                OrderDate = getOrderDateForAmount.FirstOrDefault(gd => gd.OrderId == x.Key).OrderDate.TransferToTaipeiTime(),
                TotalPrice = x.Sum(x => x.TotalPrice),
            });
            #endregion

            #region Amount: 取orderId的總價，並GroupBy
            //取同一年orderId的總價，並以Month分群
            var amountMonthList = oddataByOrderId.Where(x => x.OrderDate.Year == year).Select(x => new GetChartDataByIdDto.AmountMonthDataDto
            {
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
            });

            var amountDataByMonth = amountMonthList.GroupBy(x => x.OrderDate.Month);
            var amountMonthData = amountDataByMonth.Select(x => new GetChartDataByIdDto.AmountMonthList
            {
                Month = $"{x.Key}月",
                TotalPrice = x.Sum(x => x.TotalPrice).ToString()
            });

            //取同一月orderId的總價，並以Day分群
            var amountDayList = amountMonthList.Where(x => x.OrderDate.Month == month).Select(x => new GetChartDataByIdDto.AmountDayDataDto
            {
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
            });

            var amountDataByDay = amountDayList.GroupBy(x => x.OrderDate.Day);
            var amountDayData = amountDataByDay.Select(x => new GetChartDataByIdDto.AmountDayList
            {
                Day = x.Key.ToString(),
                TotalPrice = x.Sum(x => x.TotalPrice).ToString(),
            });

            //取同一月orderId的總價，並以Week分群
            var getWeekDataByDay = amountDayList.Where(x => weekList.Select(w => w.Day).Contains(x.OrderDate.Day)).ToList();

            var amountDataByWeek = getWeekDataByDay.GroupBy(x => x.OrderDate.DayOfWeek);
            var amountWeekData = amountDataByWeek.Select(x => new GetChartDataByIdDto.AmountWeekList
            {
                Week = x.Key.ToString(),
                TotalPrice = x.Sum(x => x.TotalPrice).ToString()
            });
            #endregion


            var getChartDto = new GetChartDataByIdDto()
            {
                TodayTotalAmount = amountDayData.FirstOrDefault(x => x.Day == day.ToString()) == null ? "0" : string.Format("{0:#,##0;0}", amountDayData.FirstOrDefault(x => x.Day == day.ToString()).TotalPrice),
                YearTotalAmount = amountMonthList.Count() == 0 ? "0" : string.Format("{0:#,##0;0}", amountMonthList.Sum(x => x.TotalPrice)),
                TodayOrders = order.Where(x => x.OrderDate.Day == day).Count().ToString() == "null" ? "0" : order.Where(x => x.OrderDate.Day == day).Count().ToString(),
                YearOrders = order.Where(x => x.OrderDate.Year == year).Count().ToString() == "null" ? "0" : order.Where(x => x.OrderDate.Year == year).Count().ToString(),
                QuantityTodayDataList = quantityTodayList.OrderByDescending(x => x.TotalQuantity).Take(1).ToList(),
                DayCount = dayCount,
                AmountMonth = amountMonthData.ToList(),
                AmountDay = amountDayData.ToList(),
                AmountWeek = amountWeekData.ToList(),
                QuantityMonthDataList = quantityMonthList.OrderByDescending(x => x.TotalQuantity).Take(7).ToList(),
                QuantityDayDataList = quantityDayList.OrderByDescending(x => x.TotalQuantity).Take(7).ToList(),
                QuantityWeekDataList = quantityWeekList.OrderByDescending(x => x.TotalQuantity).Take(7).ToList(),
            };

            return getChartDto;
        }
    }
}
