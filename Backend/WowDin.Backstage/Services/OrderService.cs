using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Models.ViewModel.Order;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using WowDin.Backstage.Common.ModelEnum;
using WowDin.Backstage.Models.Dto.Order;
using WowDin.Backstage.Models;
using Microsoft.Extensions.Logging;
using static WowDin.Backstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Backstage.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository _repository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository repository, ILogger<OrderService> logger) 
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<GetShopsByBrandVM> GetShopsByBrand(int brandId)
        {
            var shops = _repository.GetAll<Shop>().Where(s => s.BrandId == brandId && s.State != (int)ShopEnum.StateEnum.Remove).AsNoTracking().ToList();
            var shopList = shops.Select(s => new GetShopsByBrandVM()
            {
                Value = s.ShopId,
                Text = s.Name
            });
            return shopList;
            
        }

        public IEnumerable<GetAllOrderDetailsByBrandVM> GetAllOrderDetailsByBrand(int brandId)
        {
            var brand = _repository.GetAll<Brand>().First(b => b.BrandId == brandId);
            var shops = _repository.GetAll<Shop>().Where(s => s.BrandId == brandId && s.State != (int)ShopEnum.StateEnum.Remove).AsNoTracking().ToList();
            var order = _repository.GetAll<Order>().Where(o => shops.Select(s => s.ShopId).Contains(o.ShopId)).OrderByDescending(de => de.OrderDate).AsNoTracking().ToList();
            var paymentNotComplete = order.Where(o => o.PaymentType == 1 && o.PayDate == null);
            List<Order> orderListComplete = new List<Order>();
            if (paymentNotComplete != null)
            {
                orderListComplete = order.Except(paymentNotComplete).OrderByDescending(ol => ol.OrderDate).ToList();
            }
            else 
            {
                orderListComplete = order;
            }
            var orderdetail = _repository.GetAll<OrderDetail>().Where(od => orderListComplete.Select(o => o.OrderId).Contains(od.OrderId)).AsNoTracking().ToList();
            var user = _repository.GetAll<UserAccount>().Where(u => orderdetail.Select(od => od.UserAccountId).Contains(u.UserAccountId)).AsNoTracking().ToList();
            var coupon = _repository.GetAll<Coupon>().Where(c => shops.Select(s => s.ShopId).Contains(c.ShopId)).ToList();
            var orderdetails = orderListComplete.Select(o => new GetAllOrderDetailsByBrandVM()
            {
                
                BrandName = brand.Name,
                OrderId = o.OrderId,
                ShopId = o.ShopId,
                UserName = user.FirstOrDefault(u => u.UserAccountId == o.UserAcountId).NickName,
                OrderDate = o.OrderDate.ToString("yyyy/MM/dd HH:mm:ss"),
                PickUpTime = o.PickUpTime.ToString("yyyy/MM/dd HH:mm:ss"),
                ShopName = shops.First(s => s.ShopId == o.ShopId).Name,
                TakeMethod = ((OrderEnum.TakeMethodEnum)o.TakeMethodId).GetDescription(),
                Message = o.Message,
                OrderState = ((OrderEnum.OrderStateEnum)o.OrderState).GetDescription(),
                ReceiptType = ((OrderEnum.ReceiptTypeEnum)o.ReceiptType).GetDescription(),
                City = o.City ?? " ",
                District = o.District ?? " ",
                Address = o.Address ?? "來店自取",
                UpdateDate = o.UpdateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                OrderStamp = o.OrderStamp,
                Vatnumber = o.Vatnumber,
                Coupon = o.CouponId == null ? "無折扣" : coupon.FirstOrDefault(c => c.CouponId == o.CouponId).CouponName,
                TotalPrice = o.Discount != null ? ((orderdetail.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum()) - o.Discount).ToString()
                                                : orderdetail.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum().ToString("0.#"),
                TotalQuantity = orderdetail.Where(od => od.OrderId == o.OrderId).Sum(q => q.Quantity).ToString(),
                OrderDetailsList = orderdetail.Where(ord => ord.OrderId == o.OrderId).Select(od => new GetAllOrderDetailsByBrandVM.OrderDetails()
                {
                    OrderId = o.OrderId,
                    OrderDetailId = od.OrderDetailId,
                    UserName = user.First(u => u.UserAccountId == od.UserAccountId).NickName,
                    ProductName = od.ProductName,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Note = od.Note,
                    Price = (od.UnitPrice * od.Quantity).ToString()
                }).ToList()

            }).ToList();
                        

            return orderdetails;
        }

        public IEnumerable<GetAllOrderDetailsByShopVM> GetAllOrderDetailsByShop(int shopId)
        {
            //所有未接單 && o.OrderState.ToString() == "0"
            var order = _repository.GetAll<Order>().Where(o => o.ShopId == shopId).OrderByDescending(de => de.OrderDate).AsNoTracking().ToList();
            var paymentNotComplete = order.Where(o => o.PaymentType == 1 && o.PayDate == null);
            List<Order> orderListComplete = new List<Order>();
            if (paymentNotComplete != null)
            {
                orderListComplete = order.Except(paymentNotComplete).OrderByDescending(ol => ol.OrderDate).ToList();
            }
            else
            {
                orderListComplete = order;
            }

            var orderdetail = _repository.GetAll<OrderDetail>().Where(od => orderListComplete.Select(o => o.OrderId).Contains(od.OrderId)).AsNoTracking().ToList();
            var user = _repository.GetAll<UserAccount>().Where(u => orderdetail.Select(od => od.UserAccountId).Contains(u.UserAccountId)).AsNoTracking().ToList();
            
            var shop = _repository.GetAll<Shop>().First(s => s.ShopId == shopId);
            var brand = _repository.GetAll<Brand>().First(b => shop.BrandId == b.BrandId);
            var coupon = _repository.GetAll<Coupon>().Where(c => c.ShopId == shop.ShopId).ToList();
            var discountType = coupon.Select(d => d.DiscountType);

            var orderdetails = orderListComplete.Select(o => new GetAllOrderDetailsByShopVM()
            {
                OrderId = o.OrderId,
                ShopId = o.ShopId,
                UserName = user.FirstOrDefault(u => u.UserAccountId == o.UserAcountId).NickName,
                OrderDate = o.OrderDate.ToString("yyyy/MM/dd HH:mm:ss"),
                PickUpTime = o.PickUpTime.ToString("yyyy/MM/dd HH:mm:ss"),
                ShopName = shop.Name,
                BrandId = shop.BrandId,
                BrandName = brand.Name,
                TakeMethod = ((OrderEnum.TakeMethodEnum)o.TakeMethodId).GetDescription(),
                Message = o.Message,
                OrderState = ((OrderEnum.OrderStateEnum)o.OrderState).ToString(),
                ReceiptType = ((OrderEnum.ReceiptTypeEnum)o.ReceiptType).GetDescription(),
                City = o.City ?? " ",
                District = o.District ?? " ",
                Address = o.Address ?? "來店自取",
                UpdateDate = o.UpdateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                OrderStamp = o.OrderStamp,
                Vatnumber = o.Vatnumber,
                Coupon = o.CouponId == null ? "無折扣" : coupon.FirstOrDefault(c => c.CouponId == o.CouponId).CouponName,
                Discount = o.CouponId == null ? "0" : coupon.FirstOrDefault(d => d.CouponId == o.CouponId).DiscountAmount.ToString("0.#"),
                TotalPrice = orderdetail.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum().ToString("0.#"),
                TotalQuantity = orderdetail.Where(od => od.OrderId == o.OrderId).Sum(q => q.Quantity).ToString(),
                OrderDetailsList = orderdetail.Where(ord => ord.OrderId == o.OrderId).Select(od => new GetAllOrderDetailsByShopVM.OrderDetails()
                {
                    OrderId = o.OrderId,
                    OrderDetailId = od.OrderDetailId,
                    UserName = user.First(u => u.UserAccountId == od.UserAccountId).NickName,
                    
                    ProductName = od.ProductName,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Note = od.Note,
                    Price = (od.UnitPrice * od.Quantity).ToString()
                }).ToList(),

                FinalPrice = o.Discount != null ? ((orderdetail.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum()) - o.Discount).ToString()
                                                : orderdetail.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum().ToString("0.#")
            }).ToList();
            return orderdetails;
        }

        public void UpdateOrderState_Accept(GetAllOrderDetailsByShopVM query)
        {
             
            var target = _repository.GetAll<Order>().First(o => o.OrderId == query.OrderId);
            target.OrderState = (int)OrderEnum.OrderStateEnum.OrderAccepted;

            _repository.Update(target);
            _repository.Save();
                
        }

        public void UpdateOrderState_Cancel(GetAllOrderDetailsByShopVM query)
        {
            var target = _repository.GetAll<Order>().First(o => o.OrderId == query.OrderId);

            target.OrderState = (int)OrderEnum.OrderStateEnum.OrderRejected_CancelledByShop;

            _repository.Update(target);
            _repository.Save();
        }

        public void UpdateOrderState_Complete(GetAllOrderDetailsByShopVM query)
        {
            var target = _repository.GetAll<Order>().First(o => o.OrderId == query.OrderId);

            target.OrderState = (int)OrderEnum.OrderStateEnum.OrderComplete;


            if (target.PaymentType == 0) //現金付款
            {
                //改付款成功 0
                target.PayState = (int)PaymentStateEnum.Success;

                //付款時間
                target.PayDate = DateTime.UtcNow;
            }

            _repository.Update(target);
            _repository.Save();
        }

        public APIResult UpdateOrderState_AcceptALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            using (var transaction = _repository._context.Database.BeginTransaction()) 
            {
                try
                {
                    var orderList = query;
                    foreach (var order in orderList)
                    {
                        var target = _repository.GetAll<Order>().First(o => o.OrderId == order.OrderId);
                        target.OrderState = (int)OrderEnum.OrderStateEnum.OrderAccepted;
                        _repository.Update(target);
                    }
                    _repository.Save();
                    transaction.Commit();
                    return new APIResult(APIStatus.Success, "全部接單成功", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "全部接單失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            }
            
        }

        public APIResult UpdateOrderState_CancelALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            using(var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var orderList = query;
                    foreach (var order in orderList)
                    {
                        var target = _repository.GetAll<Order>().First(o => o.OrderId == order.OrderId);
                        target.OrderState = (int)OrderEnum.OrderStateEnum.OrderRejected_CancelledByShop;
                        _repository.Update(target);
                    }
                    _repository.Save();
                    transaction.Commit();
                    return new APIResult(APIStatus.Success, "全部取消成功", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "全部取消失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            };
        }

        public APIResult UpdateOrderState_CompleteALL(IEnumerable<GetAllOrderDetailsByShopVM> query)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var orderList = query;
                    foreach (var order in orderList)
                    {
                        var target = _repository.GetAll<Order>().First(o => o.OrderId == order.OrderId);
                        target.OrderState = (int)OrderEnum.OrderStateEnum.OrderComplete;

                        if (order.PaymentType == (int)PaymentTypeEnum.Cash)
                        {
                            target.PayState = (int)PaymentStateEnum.Success;
                            
                            target.PayDate = DateTime.UtcNow;
                        }

                        _repository.Update(target);
                    }
                    _repository.Save();
                    transaction.Commit();
                    return new APIResult(APIStatus.Success, "全部完成", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "全部完成失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            };
        }
    }
}
