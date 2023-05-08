using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models.Dto.Order;
using WowDin.Frontstage.Models.Dto.OrderDetails;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Repositories.Interface;
using WowDin.Frontstage.Services.Order.Interface;
using WowDin.Frontstage.Services.Store;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IRepository _repository;
        private readonly IHttpContextAccessor _httpConetextAccessor;

        public OrderService(IRepository repository, ILogger<OrderService> logger, IHttpContextAccessor httpConetextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _httpConetextAccessor = httpConetextAccessor;
        }

        #region GetOrderById
        public GetOrdersDto GetOrderById(int userid)
        {
            //傳入UserAccountId找出那一個user的所有訂單
            var orderListByUser = _repository.GetAll<Models.Entity.Order>().Where(ol => ol.UserAcountId == userid).OrderByDescending(ol => ol.OrderDate);
            List<Models.Entity.Order> orderListComplete = new List<Models.Entity.Order>();
            
            //綠界未成功付款
            var paymentNotComplete = orderListByUser.Where(o => o.PaymentType == 1 && o.PayDate == null);
            if (paymentNotComplete != null)
            {
                orderListComplete = orderListByUser.Except(paymentNotComplete).OrderByDescending(ol => ol.OrderDate).ToList();
            }
            else 
            {
                orderListComplete = orderListByUser.ToList();
            }
            //shop
            var shops = _repository.GetAll<Shop>();

            //brand
            var brands = _repository.GetAll<Brand>();

            //order detail
            var orderDetails = _repository.GetAll<OrderDetail>();

            var commentsbyOrder = _repository.GetAll<Comment>().Where(c => orderListByUser.Select(o => o.OrderId).Contains(c.OrderId));

            var result = new GetOrdersDto
            {
                IsOrderExist = orderListComplete.Count() != 0,
                OrderListByUser = orderListComplete.Select(o => new OrderDataDto
                {
                    IsCommented = commentsbyOrder.Any(c => c.OrderId == o.OrderId),
                    OrderStateEnum = (OrderStateEnum)o.OrderState,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                    OrderId = o.OrderId,
                    ShopData = new ShopDataDto
                    {
                        Brand = brands.First(b => b.BrandId == shops.First(s => s.ShopId == o.ShopId).BrandId).Name,
                        ShopName = shops.First(s => s.ShopId == o.ShopId).Name,
                        BrandImg = brands.First(b => b.BrandId == shops.First(s => s.ShopId == o.ShopId).BrandId).Logo
                    },
                    OrderDetailData = new OrderDetailDataDto
                    {
                        TotalPriceByOrder = orderDetails.Where(od => od.OrderId == o.OrderId).Select(p => p.UnitPrice * p.Quantity).Sum(),
                        TotalQuantityByOrder = orderDetails.Where(od => od.OrderId == o.OrderId).Sum(q => q.Quantity),
                        IsGroupBuy = orderDetails.Where(od => od.OrderId == o.OrderId).Select(u => u.UserAccountId).Distinct().Count() > 1
                    }
                })
            };

            return result;
        }
        #endregion

        #region GetOrderDetailDtoById
        public GetOrderDetailDto GetOrderDetailDtoById(int orderid)
        {
            //用id找到是哪一筆訂單明細
            var order = _repository.GetAll<Models.Entity.Order>().First(x => x.OrderId == orderid);
            
            //用orderid 關連到所有orderdetail(多個)
            var orderDetailList = _repository.GetAll<OrderDetail>().Where(x => x.OrderId == order.OrderId).ToList();
            //用UserAccountid 把order detail 分群
            var orderDetailByUser = orderDetailList.GroupBy(x => x.UserAccountId);

            //用order表裡面的shopid去找到是哪一家店
            var shop = _repository.GetAll<Shop>().First(x => x.ShopId == order.ShopId);
            //用shopid找到是哪一個品牌
            var brand = _repository.GetAll<Brand>().First(x => x.BrandId == shop.BrandId);
            //用brandid找到品牌圖片
            var brandImgPath = _repository.GetAll<Brand>().Where(x => x.BrandId == brand.BrandId).Select(x => x.Logo);

            //用order表裡面的UserAccountid找到訂購人
            var user = _repository.GetAll<UserAccount>().First(x => x.UserAccountId == order.UserAcountId);

            var groupBuyUser = _repository.GetAll<UserAccount>().Where(u => orderDetailList.Select(od => od.UserAccountId).Contains(u.UserAccountId)).ToList();

                                    
            var shopData = new ShopData
            {
                ShopId = shop.ShopId,
                ShopName = shop.Name,
                Phone = shop.Phone,
                City = shop.City,
                District = shop.District,
                Address = shop.Address,
                Path = string.Join("", brandImgPath),
                Brand = brand.Name
            };

            var userData = new UserData
            {
                NickName = user.NickName,
                Phone = user.Phone,
                Photo = user.Photo
            };

            var orderData = new OrderData
            {
                TakeMethod = (TakeMethodEnum)order.TakeMethodId,
                OrderDate = order.OrderDate.TransferToTaipeiTime(),
                PickUpTime = order.PickUpTime,
                Message = order.Message,
                OrderStateEnum = (OrderStateEnum)order.OrderState,
                Coupon = order.CouponId != null ? _repository.GetAll<Coupon>().First(c => c.CouponId == order.CouponId).CouponName : "無折扣",
                PaymentType = (PaymentTypeEnum)order.PaymentType,
                PayDate = order.PayDate == null ? string.Empty : order.PayDate.Value.ToString("yyyy-MM-dd"),
                ReceiptType = (ReceiptTypeEnum)order.ReceiptType,
                //外送
                City = order.City,
                District = order.District,
                Address = order.Address,
                isDelivery = order.DeliveryFee != null ? true : false,
                DeliveryFee = order.DeliveryFee != null ? (decimal)order.DeliveryFee : 0,
                VatNum = order.Vatnumber != "" ? order.Vatnumber : ""
            };

            var orderdetailData = orderDetailByUser.Select(od => new OrderDetailData
            {
                NickName = _repository.GetAll<UserAccount>().First(n => n.UserAccountId == od.Key).NickName,

                Photo = groupBuyUser.FirstOrDefault(gb => gb.UserAccountId == od.Key) == null ? string.Empty :
                groupBuyUser.FirstOrDefault(gb => gb.UserAccountId == od.Key).Photo,

                ProductDataList = od.Select(pd => new ProductData
                {
                    ProductName = pd.ProductName,
                    Note = pd.Note,
                    UnitPrice = pd.UnitPrice,
                    Quantity = pd.Quantity,
                }),

                TotalPriceByUser = od.Sum(p => p.UnitPrice * p.Quantity),
                TotalQuantityByUser = od.Sum(q => q.Quantity)
            });



            //是否已完成訂單
            var completeEnum = (int)OrderEnum.OrderStateEnum.OrderComplete;
            bool orderisCompleted =  order.OrderState.ToString() == completeEnum.ToString();
            //是否已經有評論
            var isCommented = _repository.GetAll<Comment>().Any(cm => cm.OrderId == orderid);
            if (isCommented)
            {
                var comment = _repository.GetAll<Comment>().First(cm => cm.OrderId == orderid);
                var commentData = new CommentDataDto()
                {
                    Comment1 = comment.Comment1,
                    Star = comment.Star
                };

                var result = new GetOrderDetailDto
                {
                    OrderId = order.OrderId,
                    OrderStamp = order.OrderStamp,
                    Shop = shopData,

                    User = userData,

                    Order = orderData,

                    OrderDetails = orderdetailData,

                    PriceTotal = orderDetailList.Sum(p => p.UnitPrice * p.Quantity),
                    QuantityTotal = orderDetailList.Sum(q => q.Quantity),
                    isCommented = isCommented,
                    CommentData = commentData
                };

                var discount = 0.0m; //預設折扣都是0
                if (order.CouponId != null)
                {
                    var coupon = _repository.GetAll<Coupon>().First(c => c.CouponId == order.CouponId);
                    if (coupon.DiscountType == (int)CouponEnum.CouponType.ForProduct)
                    {
                        discount = coupon.DiscountAmount;
                    }
                    else if (coupon.DiscountType == (int)CouponEnum.CouponType.Storewide)
                    {
                        discount = Math.Ceiling(result.PriceTotal * (1 - coupon.DiscountAmount));
                    }
                    else
                    {
                        discount = 0;
                    }
                }

                result.Discount = discount;
                result.FinalPrice = Math.Ceiling((result.PriceTotal - discount + result.Order.DeliveryFee)).ToString();

                return result;
            }

            else 
            {
                var result = new GetOrderDetailDto
                {
                    OrderId = order.OrderId,
                    OrderStamp = order.OrderStamp,
                    Shop = shopData,

                    User = userData,

                    Order = orderData,

                    OrderDetails = orderdetailData,

                    PriceTotal = orderDetailList.Sum(p => p.UnitPrice * p.Quantity),
                    QuantityTotal = orderDetailList.Sum(q => q.Quantity),
                    orderIsComplete = orderisCompleted,
                    isCommented = isCommented
                };

                var discount = 0.0m; //預設折扣都是0
                if (order.CouponId != null)
                {
                    var coupon = _repository.GetAll<Coupon>().First(c => c.CouponId == order.CouponId);
                    if (coupon.DiscountType == (int)CouponEnum.CouponType.ForProduct)
                    {
                        discount = coupon.DiscountAmount;
                    }
                    else if (coupon.DiscountType == (int)CouponEnum.CouponType.Storewide)
                    {
                        discount = Math.Ceiling(result.PriceTotal * (1 - coupon.DiscountAmount));
                    }
                    else
                    {
                        discount = 0;
                    }
                }

                result.Discount = discount;
                result.FinalPrice = Math.Ceiling((result.PriceTotal - discount + result.Order.DeliveryFee)).ToString();

                return result;
            }

        }

        public OperationResult AddProductToCart(AddProductToCartDto request)
        {
            var result = new OperationResult();
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.UserAccountId == request.LeaderId && c.ShopId == request.ShopId);

            if (cart == null)
            {
                using (var transaction = _repository._context.Database.BeginTransaction())
                {
                    try
                    {
                        var cartEntity = new Cart()
                        {
                            UserAccountId = request.LeaderId,
                            ShopId = request.ShopId,
                            Orderdate = request.OrderTime,
                        };

                        cart = _repository._context.Carts.Add(cartEntity).Entity;
                        _repository._context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result.Message = "購物車新增失敗";
                        //logger.Error(ex, result.Message);
                        return result;
                    }
                }
                    
            }
            //cart是帶有id的資料

            var cartDetail = request.CartDetails;
            
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var cartDetailEntity = new CartDetail()
                    {
                        //CartDetailId = 1, /*故意出錯測試*/
                        CartId = cart.CartId,
                        UserAccountId = cartDetail.OrderAccountId,
                        NickName = cartDetail.NickName,
                        ProductId = cartDetail.ProductId,
                        UnitPrice = cartDetail.UnitPrice,
                        Quantity = cartDetail.Quentity,
                        Note = cartDetail.Specs
                    };

                    _repository.Create<CartDetail>(cartDetailEntity);
                    _repository.Save();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.Message = "產品加入購物車失敗";
                    //_logger.Error(ex, result.Message);
                    return result;
                }
        }

            result.IsSuccessful = true;
            return result;
        }

        

        public OperationResult DeleteProductFromCart(DeleteCartDetailDto request)
        {
            var result = new OperationResult();
            var cartDetail = _repository.GetAll<CartDetail>().FirstOrDefault(cd => cd.CartDetailId == request.CartDetailId);
            
            if (cartDetail != null)
            {

                //using (var transaction = _repository._context.Database.BeginTransaction())
                try
                {

                    _repository._context.CartDetails.Remove(cartDetail);
                    _repository._context.SaveChanges();
                    //transaction.Commit();
                }
                catch(Exception ex) 
                {
                    //transaction.Rollback();
                    result.Message = "產品刪除失敗";
                    return result;
                }
            }
            result.IsSuccessful = true;
            return result;
        }
        public OperationResult DeleteUserCart(DeleteCartDto request) {
            var result = new OperationResult();
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.CartId == request.CartId);
            var cartDetails = _repository.GetAll<CartDetail>().Where(cd => cd.CartId == request.CartId).ToList() ?? null;

            if (cart != null) 
            {
                if (cartDetails != null)
                {
                    using (var transaction = _repository._context.Database.BeginTransaction())
                        try
                        {
                            //先刪detail
                            foreach (var cd in cartDetails)
                            {
                                _repository._context.Remove(cd);
                            }

                            //再刪cart
                            _repository._context.Remove(cart);
                            _repository._context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            result.Message = "購物車刪除失敗";
                            return result;
                        }
                }
                else 
                {
                    //只刪除cart
                    _repository._context.Remove(cart);
                    _repository._context.SaveChanges();
                }
            }
            result.IsSuccessful = true;
            return result;
        }


        public OperationResult AddCartByGroup(AddProductToCartDto request)
        {
            var result = new OperationResult();
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.UserAccountId == request.LeaderId && c.ShopId == request.ShopId);

            if (cart != null)
            {
                try
                {
                    cart.GroupCode ??= Guid.NewGuid().ToString();
                    _repository.Update(cart);

                }
                catch (Exception ex)
                {
                    result.Message = "購物車已存在，團購碼產生失敗";
                    //logger.Error(ex, result.Message);
                    return result;
                }
            }
            else
            {
                try
                {
                    cart = new Cart()
                    {
                        UserAccountId = request.LeaderId,
                        ShopId = request.ShopId,
                        Orderdate = request.OrderTime
                    };

                    cart.GroupCode = Guid.NewGuid().ToString();
                    _repository._context.Carts.Add(cart);
                }
                catch(Exception ex)
                {
                    result.Message = "團購購物車新增失敗";
                    //logger.Error(ex, result.Message);
                    return result;
                }
                
            }

            _repository._context.SaveChanges();
            result.IsSuccessful = true;
            result.Message = $"{_httpConetextAccessor.HttpContext.Request.Scheme}://{_httpConetextAccessor.HttpContext.Request.Host.Value}/Group/{cart.GroupCode}";

            return result; 
        }
        #endregion

        public GetUserAndShopFromGroupIdDto GetUserAndShopFromGroupId(string groupId)
        {
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.GroupCode == groupId);
            if(cart == null)
            {
                return null;
            }
            return new GetUserAndShopFromGroupIdDto { ShopId = cart.ShopId, UserAccountId = cart.UserAccountId};
        }

        public ProductInCartDto GetAmountInCart(int leaderId, int shopId)
        {
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.UserAccountId == leaderId && c.ShopId == shopId);

            if (cart == null)
            {
                return null;
            }

            var cartDetails = _repository.GetAll<CartDetail>().Where(d => d.CartId == cart.CartId);

            var result = new ProductInCartDto
            {
                ShopId = cart.ShopId,   
                ProductsAmount = cartDetails.Count(),
                TotalPrice = cartDetails.Sum(d => d.Quantity * d.UnitPrice)
            };

            return result;
        }

        #region GetCartByUserId
        
        public GetCartsDto GetCartByUserId(int userId) 
        {
            //brand source
            //var brand = _repository.GetAll<Brand>();
            var brand = _repository.GetAll<Brand>().Where(x => x.Verified == 1 && x.Suspension == false);

            //shop source
            //var shop = _repository.GetAll<Shop>();
            var shop = _repository.GetAll<Shop>().Where(s => brand.Select(b => b.BrandId).Contains(s.BrandId));
            //cart detail source
            var cartdetails = _repository.GetAll<CartDetail>();

            //傳入已登入的userid，找到所有的購物車
            //var getCartsListByUser = _repository.GetAll<Cart>().Where(c => c.UserAccountId == userId).ToList();
            var userCart = _repository.GetAll<Cart>().Where(c => c.UserAccountId == userId);
            var getCartsListByUser = userCart.Where(c => shop.Select(s => s.ShopId).Contains(c.ShopId)).ToList();

            var cartsByUser = new GetCartsDto()
            {
                IsCartExist = getCartsListByUser.Count() != 0,
                CartListByUser = getCartsListByUser.Select(c => new GetCartsDto.CartDataDto()
                {
                    CartId = c.CartId,
                    OrderDate = c.Orderdate.ToString("yyyy-MM-dd"),
                    ShopData = new GetCartsDto.CartDataDto.CartShopDataDto()
                    {
                       Brand = brand.First(b => b.BrandId == shop.First(s => s.ShopId == c.ShopId).BrandId).Name,
                       BrandImg = brand.First(b => b.BrandId == shop.First(s => s.ShopId == c.ShopId).BrandId).Logo,
                       ShopId = shop.First(s => s.ShopId == c.ShopId).ShopId,
                       ShopName = shop.First(s => s.ShopId == c.ShopId).Name
                    },
                    CartDetailData = new GetCartsDto.CartDataDto.CartDetailDataDto()
                    {
                        CartDetailIds = cartdetails.Where(cds => cds.CartId == c.CartId).Select(cd => new GetCartsDto.CartDataDto.CartDetailIdDto()
                        {
                            cartDetailId = cd.CartDetailId
                        }).ToList(),
                        IsGroupBuy = cartdetails.Where(cd => cd.CartId == c.CartId).Select(u => u.UserAccountId).Distinct().Count() > 1,
                        TotalQuantityByCart = cartdetails.Where(cd => cd.CartId == c.CartId).Sum(cd => cd.Quantity),
                        TotalPriceByCart = cartdetails.Where(cd => cd.CartId == c.CartId).Sum(cd => cd.Quantity * cd.UnitPrice)
                    }
                })
            };

            return cartsByUser;
        }

        //get cartdetails bu cartid

        #endregion

        #region GetCartDetailsByUserId+ShopId
        public GetCartDetailsDto GetCartDetails(int userId, int shopId)
        {
            //PK: userId+shopId
            var cart = _repository.GetAll<Cart>().Where(c => c.UserAccountId == userId);
            var shop = _repository.GetAll<Shop>().First(s => s.ShopId == shopId);
            var cartId = cart.First(c => c.UserAccountId == userId && c.ShopId == shopId).CartId;
            var cartdetails = _repository.GetAll<CartDetail>().Where(cd => cd.CartId == cartId).AsNoTracking().ToList();
            var user = _repository.GetAll<UserAccount>().First(u => u.UserAccountId == userId);

            var product = _repository.GetAll<Product>().ToList();
            var customList = _repository.GetAll<Custom>().Where(x => product.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
            var selectionList = _repository.GetAll<CustomSelection>().Where(x => customList.Select(y => y.CustomId).Contains(x.CustomId)).ToList();

            var productdetail = product.ToList().Where(p => cartdetails.ToList().Select(cd => cd.ProductId).Contains(p.ProductId)).GetProductsWithCustom(customList, selectionList).ToList();
            //brand source
            var brand = _repository.GetAll<Brand>().First(b => b.BrandId == shop.BrandId);

            var cartDetail = new GetCartDetailsDto();
                cartDetail.UserAccountId = userId;
                cartDetail.ShopId = shopId;
                cartDetail.Cartid = cartId;
                cartDetail.UserName = user.NickName;
                cartDetail.IsGroupBuy = cart.First(c => c.CartId == cartId).GroupCode != null ? true : false;
                cartDetail.Phone = user.Phone;
                cartDetail.StoreAddress = $"{shop.City}{shop.District}{shop.Address}";
                cartDetail.StoreCity = shop.City;
                cartDetail.StoreDistrict = shop.District;
                cartDetail.ShopData = new GetCartDetailsDto.CartShopDataDto()
                {
                    Brand = brand.Name,
                    BrandImg = brand.Logo,
                    ShopName = shop.Name,
                    ShopPhone = shop.Phone,
                    ShopPaymentType = string.Join("", _repository.GetAll<ShopPaymentType>().Where(p => p.ShopId == shop.ShopId).Select(pm => ((PaymentTypeEnum)pm.PaymentType).GetDescription())),
                    ShopTakeMethodType = string.Join("", _repository.GetAll<ShopMethod>().Where(m => m.ShopId == shop.ShopId).Select(sm => ((ShopMethodEnum.TakeMethodEnum)sm.TakeMethod).GetDescription()))
                };
               
                cartDetail.ProductDetailsByCart = cartdetails.GroupBy(cd => cd.UserAccountId).Select(u => new GetCartDetailsDto.ProductDetailsByCartDto()
                {
                    UserAcountId = u.Key,
                    NickName = _repository.GetAll<UserAccount>().First(ua => ua.UserAccountId == u.Key).NickName,
                    Photo = _repository.GetAll<UserAccount>().First(ua => ua.UserAccountId == u.Key).Photo,
                    ProductDetailsByUser = u.ToList().Select(d => new GetCartDetailsDto.ProductDetailsByCartDto.ProductDetailsByUserDto()
                    {
                        CartDetailId = d.CartDetailId,
                        NickName = d.NickName,
                        ProductName = product.First(p => p.ProductId == d.ProductId).Name,
                        Product = new ProductDto
                        {
                            Name = product.First(p => p.ProductId == d.ProductId).Name,
                            ProductId = product.First(p => p.ProductId == d.ProductId).ProductId,
                            Figure = product.First(p => p.ProductId == d.ProductId).Fig,
                            BasicPrice = product.First(p => p.ProductId == d.ProductId).BasicPrice,
                            SellOut = (ProductEnum.StateEnum)product.First(p => p.ProductId == d.ProductId).State == ProductEnum.StateEnum.Disable ? true : false,
                            Customs = _repository.GetAll<Models.Entity.Custom>().Where(cu => cu.ProductId == d.ProductId).Select(cu => new CustomDto
                            {
                                Name = cu.Name,
                                MaxAmount = (int)cu.MaxAmount,
                                Necessary = cu.Necessary,
                                Selections = _repository.GetAll<CustomSelection>().Where(s => s.CustomId == cu.CustomId).Select(s => new SelectionDto
                                {
                                    Name = s.Name,
                                    IsSelected = false,
                                    AddPrice = s.AddPrice
                                }).ToList()
                            }).ToList()
                        },
                        UnitPrice = d.UnitPrice,
                        Quantity = d.Quantity,
                        Note = d.Note,
                        Price = d.UnitPrice * d.Quantity
                    }).ToList()

                }).ToList();
                cartDetail.OpenTime = shop.OpenTime.TransferToTaipeiTime();
                cartDetail.CloseTime = shop.CloseTime.TransferToTaipeiTime();
                cartDetail.OpenDayList = shop.OpenDayList;
                cartDetail.HasSticker = shop.HasSticker;
                cartDetail.TotalPrice = cartdetails.Sum(p => p.UnitPrice * p.Quantity);
                cartDetail.TotalQuantity = cartdetails.Sum(q => q.Quantity);
                cartDetail.OrderStamp =  $"M{DateTimeOffset.Now.ToUnixTimeSeconds()}";

            #region cart detail backup
            //{
            //    UserAccountId = userId,
            //    ShopId = shopId,
            //    Cartid = cartId,
            //    UserName = user.NickName,
            //    Phone = user.Phone,
            //    StoreAddress = $"{shop.City}{shop.District}{shop.Address}",
            //    ShopData = new GetCartDetailsDto.CartShopDataDto()
            //    {
            //        Brand = brand.Name,
            //        BrandImg = brand.Logo,
            //        ShopName = shop.Name,
            //        ShopPhone = shop.Phone
            //    },
            //    ProductDetailsByCart = cartdetails.Select(cd => new GetCartDetailsDto.ProductDetailsByCartDto()
            //    {
            //        UserAcountId = cd.UserAccountId,
            //        NickName = cd.NickName,
            //        //Photo = 
            //        ProductDetailsByUser = cartdetails.Select(cd => new GetCartDetailsDto.ProductDetailsByCartDto.ProductDetailsByUserDto()
            //        {
            //            NickName = _repository.GetAll<UserAccount>().First(u => u.UserAccountId == cd.UserAccountId).NickName,
            //            ProductName = product.First(p => p.ProductId == cd.ProductId).Name,
            //            UnitPrice = cd.UnitPrice,
            //            //discount,
            //            Quantity = cd.Quantity,
            //            Note = cd.Note,
            //            Price = cd.UnitPrice * cd.Quantity
            //        })

            //    }),
            //    OpenTime = shop.OpenTime,
            //    CloseTime = shop.CloseTime,
            //    OpenDayList = shop.OpenDayList,
            //    //放ting的購物車資料
            //    ProductDetailsData = product.Where(p => cartdetails.Select(cd => cd.ProductId).Contains(p.ProductId)).GetProductsWithCustom(_repository).ToList(),
            //    HasSticker = shop.HasSticker,
            //    //Discount
            //    TotalPrice = cartdetails.Sum(p => p.UnitPrice * p.Quantity),
            //    TotalQuantity = cartdetails.Sum(q => q.Quantity)
            //};
            #endregion


            return cartDetail;
        }
        #endregion
        #region GetCartDetailsByCartId
        //public GetCartDetailsDto GetCartDetails(int cartId) 
        //{
        //    var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.CartId == cartId);
        //    var shop = _repository.GetAll<Shop>().FirstOrDefault(s => s.ShopId == cart.ShopId);
        //    var cartdetails = _repository.GetAll<CartDetail>().Where(cd => cd.CartId == cartId).ToList();
        //    var user = _repository.GetAll<UserAccount>().FirstOrDefault(u => u.UserAccountId == cart.UserAccountId);
        //    var product = _repository.GetAll<Product>();
        //    var brand = _repository.GetAll<Brand>().First(b => b.BrandId == shop.BrandId);

        //    var cartDetail = new GetCartDetailsDto()
        //    {
        //        UserAccountId = user.UserAccountId,
        //        ShopId = shop.ShopId,
        //        Cartid = cartId,
        //        UserName = user.NickName,
        //        Phone = user.Phone,
        //        StoreAddress = $"{shop.City}{shop.District}{shop.Address}",
        //        ShopData = new GetCartDetailsDto.CartShopDataDto()
        //        {
        //            Brand = brand.Name,
        //            BrandImg = brand.Logo,
        //            ShopName = shop.Name,
        //            ShopPhone = shop.Phone
        //        },
        //        OpenTime = shop.OpenTime,
        //        CloseTime = shop.CloseTime,
        //        OpenDayList = shop.OpenDayList,
        //        ProductDetailsData = product.Where(p => cartdetails.Select(cd => cd.ProductId).Contains(p.ProductId)).GetProductsWithCustom(_repository).ToList(),
        //        HasSticker = shop.HasSticker,
        //        //Discount
        //        TotalPrice = cartdetails.Sum(p => p.UnitPrice * p.Quantity),
        //        TotalQuantity = cartdetails.Sum(q => q.Quantity)
        //    };
        //    return cartDetail;
        //}
        #endregion

        #region comment
        public OperationResult CreateComment(CreateCommentInputDto input)
        {
            var result = new OperationResult();
            var order = _repository.GetAll<Models.Entity.Order>().FirstOrDefault(o => o.OrderId == input.OrderId);
            var shopSource = _repository.GetAll<Shop>();
            var shopId = order.ShopId;

            if (order != null)
            {
                using (var transaction = _repository._context.Database.BeginTransaction())
                    try 
                    {
                        //mapping
                        //把Dto mapping 成 Entity
                        var entity = new Comment
                        {
                            UserAccountId = order.UserAcountId,
                            Comment1 = input.Comment1,
                            Star = input.Star,
                            Date = DateTime.UtcNow,
                            BrandId = shopSource.First(s => s.ShopId == shopId).BrandId,
                            ShopId = shopId,
                            OrderId = order.OrderId
                        };

                        _repository.Create<Comment>(entity);
                        _repository.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result.Message = "評論建立失敗";
                        return result;
                    }
            }
            result.IsSuccessful = true;
            return result;
        }

      

        #endregion

        #region create order
        public OperationResult CreateOrder(AddCartDetailsInputDto input)
        {
            //create order
            var result = new OperationResult();
            var cart = _repository.GetAll<Cart>().FirstOrDefault(c => c.CartId == input.CartId);
            var cartdetails = _repository.GetAll<CartDetail>().Where(cd => cd.CartId == input.CartId).ToList();
            var coupon = _repository.GetAll<Coupon>().Where(c => c.ShopId == input.ShopId).ToList();

            if (cart != null && cartdetails != null)
            {
                using (var transaction = _repository._context.Database.BeginTransaction())
                try
                {
                        var orderEntity = new Models.Entity.Order()
                        {
                            UserAcountId = input.UserAccountId,
                            OrderDate = input.OrderDate,
                            PickUpTime = input.PickUpTime,
                            ShopId = input.ShopId,
                            TakeMethodId = (int)input.TakeMethodId,
                            Message = input.Message,
                            OrderState = (int)input.OrderState,
                            PaymentType = (int)input.PaymentType,
                            PayState = (int)input.PayState,
                            CouponId = input.CouponId,
                            City = input.City,
                            District = input.District,
                            Address = input.Address,
                            UpdateDate = input.UpdateDate,
                            OrderStamp = input.OrderStamp,
                            Vatnumber = input.VATNumber,
                            DeliveryFee = input.DeliveryFee,
                            Discount = input.CouponId == null ? 0 : coupon.First(c => c.CouponId == input.CouponId).DiscountAmount
                        };

                        //order with coupon
                        if (input.CouponId != null)
                        {
                            if (coupon.First(c => c.CouponId == input.CouponId).DiscountType == (int)CouponEnum.CouponType.Storewide)
                            {
                                orderEntity.Discount = Math.Ceiling((decimal.Parse(input.FinalPrice) - (decimal)input.DeliveryFee) * (1 - coupon.First(c => c.CouponId == input.CouponId).DiscountAmount));
                            }
                        }

                        _repository.Create<Models.Entity.Order>(orderEntity);
                        _repository.Save();

                        //update coupon status
                        if (input.CouponId != null) 
                        {
                            var targetCouponContainer = _repository.GetAll<CouponContainer>().FirstOrDefault(cc => cc.UserAccountId == input.UserAccountId && cc.CouponId == input.CouponId);
                            if (targetCouponContainer != null)
                            { 
                                targetCouponContainer.CouponState = (int)CouponContainerEnum.CouponState.Used;
                                _repository.Update(targetCouponContainer);
                                _repository.Save();
                            }
                        }

                        //create orderdetails
                        var product = _repository.GetAll<Product>();
                        var orderid = _repository.GetAll<Models.Entity.Order>().FirstOrDefault(o => o.OrderStamp == input.OrderStamp).OrderId;

                        if (orderid.ToString() != null)
                        {
                            var orderdetailsList = cartdetails.Select(cd => new OrderDetail()
                            {
                                OrderId = orderid,
                                UserAccountId = cd.UserAccountId,
                                ProductName = product.First(p => p.ProductId == cd.ProductId).Name,
                                UnitPrice = cd.UnitPrice,
                                Discount = 0,
                                Quantity = (short)cd.Quantity,
                                Note = cd.Note,

                            });


                            foreach (var entity in orderdetailsList)
                            {
                                _repository.Create<OrderDetail>(entity);
                            }
                                _repository.Save();
                        }

                        if (orderEntity.PaymentType == 0)
                        {
                            //delete cart details
                            foreach (var cd in cartdetails)
                            {
                                _repository.Delete<CartDetail>(cd);
                            }
                                _repository.Save();

                            //delete cart
                            _repository.Delete<Cart>(cart);
                            _repository.Save();
                        }

                        transaction.Commit();
                    }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.Message = "Failed to create order";
                    return result;
                }
            }

            return result;
        }

        public bool IsExistOrder(int orderid)
        {
            return _repository.GetAll<Models.Entity.Order>().Any(o => o.OrderId == orderid);
        }
        #endregion
        public GetECPayDataDto GetECPayDataById(int id)
        {
            var cartdetail = _repository.GetAll<CartDetail>().Where(x => x.CartId == id).ToList();
            var cart = _repository.GetAll<Cart>().First(x => x.CartId == id);
            var shopId = cart.ShopId;

            var cartList = new GetECPayDataDto
            {
                TotalAmount = cartdetail.Sum(p => p.Quantity * p.UnitPrice).ToString("0"),
                ShopId = shopId,
            };

            

            return cartList;
        }

        public GetAllDataDto GetAllData()
        {
            var shop = _repository.GetAll<Shop>();
            var brand = _repository.GetAll<Brand>();

            var allshopDto = new GetAllDataDto()
            {
                Shops = shop.Select(x => new ShopListDto
                {
                    BrandId = x.BrandId,
                    ShopId = x.ShopId,
                    ShopName = x.Name,
                    ShopPhone = x.Phone,
                }),
                Brands = brand.Select(x => new BrandListDto
                {
                    BrandId = x.BrandId,
                    BrandName = x.Name
                })
            };

            return allshopDto;
        }

        public GetCartCompleteDto GetCartComplete(string orderStamp)
        {
            var order = _repository.GetAll<Models.Entity.Order>().First(o => o.OrderStamp == orderStamp);
            var shop = _repository.GetAll<Shop>().FirstOrDefault(s => s.ShopId == order.ShopId);
            var brand = _repository.GetAll<Brand>().FirstOrDefault(b => b.BrandId == shop.BrandId);

            var result = new GetCartCompleteDto()
            {
                TakeMethod = ((OrderEnum.TakeMethodEnum)order.TakeMethodId).GetDescription(),
                PickUpTime = order.PickUpTime.ToString(),
                ShopName = shop.Name,
                BrandName = brand.Name,
                OrderStamp = orderStamp,
                OrderId = order.OrderId,
                ShopPhone = shop.Phone,
            };
            return result;
        }

        public GetCartCompleteDto DataUpdate(int cartId, string orderStamp, string paymentDate)
        {
            var target = _repository.GetAll<Models.Entity.Order>().First(o => o.OrderStamp == orderStamp);
            target.PayDate = DateTime.Parse(paymentDate);
            target.PayState = (int)OrderEnum.PayStateEnum.Success;

            _repository.Update(target);
            _repository.Save();

            try
            {
                var checkCart = _repository.GetAll<Cart>().First(x => x.CartId == cartId);
                var checkCartdetail = _repository.GetAll<CartDetail>().Where(x => x.CartId == cartId).ToList();
            }
            catch (Exception ex)
            {
                return new GetCartCompleteDto()
                {
                    CompleteMessage = "訂單建立成功"
                };
            }


            var cart = _repository.GetAll<Cart>().First(x => x.CartId == cartId);
            var cartdetail = _repository.GetAll<CartDetail>().Where(x => x.CartId == cartId).ToList();

            //刪除購物車明細
            foreach (var cd in cartdetail)
            {
                _repository.Delete<CartDetail>(cd);
                _repository.Save();
            }

            //刪除購物車
            _repository.Delete<Cart>(cart);
            _repository.Save();



            return new GetCartCompleteDto()
            {
                CompleteMessage = "訂單建立成功"
            };
        }

        public OperationResult OrderCancelled(CancelOrderDto request)
        {
            var result = new OperationResult();
            var orderTarget = _repository.GetAll<Models.Entity.Order>().FirstOrDefault(o => o.OrderId == request.OrderId && o.OrderState == (int)OrderStateEnum.OrderEstablished);
            if (orderTarget != null)
            {
                using (var transaction = _repository._context.Database.BeginTransaction())
                    try
                    {
                        orderTarget.OrderState = (int)OrderEnum.OrderStateEnum.OrderRejected_Cancelled;
                        _repository.Update(orderTarget);
                        _repository.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result.Message = "訂單取消失敗";
                        return result;
                    }
            }
            result.IsSuccessful = true;
            return result;
        }
    }
}
