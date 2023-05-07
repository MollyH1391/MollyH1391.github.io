using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Common;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models;
using WowDin.Frontstage.Models.Dto.OrderDetails;
using WowDin.Frontstage.Models.Dto.Order;
using WowDin.Frontstage.Models.ViewModel;
using WowDin.Frontstage.Models.ViewModel.Order;
using WowDin.Frontstage.Models.ViewModel.Order.Data;
using WowDin.Frontstage.Services.Order.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using WowDin.Frontstage.Models.ViewModel.Store;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using WowDin.Frontstage.Services;
using static WowDin.Frontstage.Models.ViewModel.Order.CartDetailViewModel;
using WowDin.Frontstage.Services.Interface;

namespace WowDin.Frontstage.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;
        private readonly IActivityService _activityService;

        public OrderController(IOrderService orderService, IMapper mapper, IActivityService activityService, IStoreService storeService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _storeService = storeService;
            _activityService = activityService;
        }

        #region Order
        
        [Authorize]
        public IActionResult Index()
        {
            var id = int.Parse(User.Identity.Name);
            var orderSource = _orderService.GetOrderById(id);

            var orderResult = new OrdersViewModel
            {
                IsOrderExist = orderSource.IsOrderExist,
                OrderListByUser = orderSource.OrderListByUser.Select(x => new OrdersViewModel.OrderDataVM
                {
                    IsCommented = x.IsCommented,
					OrderStateEnum = x.OrderStateEnum.GetDescription(),
					OrderDate = x.OrderDate,
                    OrderId= x.OrderId,
					ShopData = new OrdersViewModel.OrderDataVM.ShopDataVM
					{
						Brand = x.ShopData.Brand,
						ShopName = x.ShopData.ShopName,
                        BrandImg = x.ShopData.BrandImg
					},
					OrderDetailData = new OrdersViewModel.OrderDataVM.OrderDetailDataVM
					{
						TotalPriceByOrder = x.OrderDetailData.TotalPriceByOrder.ToString(),
						TotalQuantityByOrder = x.OrderDetailData.TotalQuantityByOrder.ToString(),
                        IsGroupBuy = x.OrderDetailData.IsGroupBuy
					}
				})
            };
                
            return View(orderResult);
        }
        #endregion

        #region OrderDetail
        
        [Authorize]
        public IActionResult OrderDetail(int id)
        {
            //by orderid
            var orderDetailSource = _orderService.GetOrderDetailDtoById(id);
            var order = orderDetailSource.Order;
            var orderDetailList = orderDetailSource.OrderDetails;
            var shop = orderDetailSource.Shop;
            var user = orderDetailSource.User;

            var OrderId = orderDetailSource.OrderId;
            var OrderStamp = orderDetailSource.OrderStamp;
            var Shop = new OrderDetailsViewModel.ShopData
            {
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                Phone = shop.Phone,
                City = shop.City,
                District = shop.District,
                Address = shop.Address,
                Path = shop.Path,
                Brand = shop.Brand
            };
            var Customer = new OrderDetailsViewModel.CustomerData
            {
                NickName = user.NickName,
                Phone = user.Phone
            };
            var Order = new OrderDetailsViewModel.OrderData
            {
                TakeMethod = order.TakeMethod.GetDescription(),
                OrderDate = order.OrderDate,
                PickUpTime = order.PickUpTime,
                ReceiptType = order.ReceiptType.GetDescription(),
                Message = order.Message,
                OrderStateEnum = order.OrderStateEnum.GetDescription(),
                PaymentType = order.PaymentType.GetDescription(),
                PayDate = order.PayDate,
                Coupon = order.Coupon,
                // UsePoint = order.UsePoint
                //外送
                City = order.City,
                District = order.District,
                Address = order.Address,
                isDelivery = order.isDelivery,
                DeliveryFee = order.DeliveryFee.ToString(),
                VatNum = order.VatNum
            };
            var OrderDetails = orderDetailList.Select(x => new OrderDetailsViewModel.OrderDetailData
            {
                NickName = x.NickName,
                Photo = x.Photo,
                ProductDataList = x.ProductDataList.Select(y => new OrderDetailsViewModel.ProductData
                {
                    ProductName = y.ProductName,
                    UnitPrice = y.UnitPrice.ToString(),
                    Quantity = y.Quantity.ToString(),
                    Note = y.Note
                }),
                TotalPriceByUser = x.ProductDataList.Sum(p => p.UnitPrice * p.Quantity).ToString(),
                TotalQuantityByUser = x.ProductDataList.Sum(q => q.Quantity).ToString(),

            });

            var PriceTotal = orderDetailList.Sum(p => p.TotalPriceByUser).ToString();
            var QuantityTotal = orderDetailList.Sum(q => q.TotalQuantityByUser).ToString();
            var orderisComplete = orderDetailSource.orderIsComplete;
            var isCommented = orderDetailSource.isCommented;
            if (isCommented)
            {
                var commentSource = orderDetailSource.CommentData;
                var Comments = new OrderDetailsViewModel.CommentDataVM()
                {
                    Comment1 = commentSource.Comment1,
                    Star = commentSource.Star
                };

                var orderdetaildata = new OrderDetailsViewModel
                {
                    OrderId = OrderId,
                    OrderStamp = OrderStamp,
                    Shop = Shop,
                    Customer = Customer,
                    Order = Order,
                    OrderDetails = OrderDetails,
                    PriceTotal = PriceTotal,
                    QuantityTotal = QuantityTotal,
                    isCommented = isCommented,
                    Comments = Comments,
                    Discount = orderDetailSource.Discount.ToString("0.#"),
                    FinalPrice = orderDetailSource.FinalPrice
                };
                return View(orderdetaildata);
            }
            else
            {
                var result = new OrderDetailsViewModel()
                {

                    OrderId = OrderId,
                    OrderStamp = OrderStamp,
                    Shop = Shop,
                    Customer = Customer,
                    Order = Order,
                    OrderDetails = OrderDetails,
                    PriceTotal = PriceTotal,
                    QuantityTotal = QuantityTotal,
                    orderIsComplete = orderisComplete,
                    isCommented = isCommented,
                    Discount = orderDetailSource.Discount.ToString("0.#"),
                    FinalPrice = orderDetailSource.FinalPrice
                };
                return View(result);
            }
            
            
        }
        #endregion


        #region get carts by userid
        
        [Authorize]
        public IActionResult Carts()
        {
            var id = int.Parse(User.Identity.Name);
            var cartsSource = _orderService.GetCartByUserId(id);

            var cartsResult = new CartsViewModel()
            {
                IsCartExist = cartsSource.IsCartExist,
                CartListByUserVM = cartsSource.CartListByUser.Select(c => new CartsViewModel.CartDataViewModel()
                {
                    CartId = c.CartId,
                    OrderDate = c.OrderDate,
                    ShopDataVM = new CartsViewModel.CartDataViewModel.CartShopDataViewModel()
                    {
                        Brand = c.ShopData.Brand,
                        BrandImg = c.ShopData.BrandImg,
                        ShopId = c.ShopData.ShopId,
                        ShopName = c.ShopData.ShopName
                    },
                    CartDetailDataVM = new CartsViewModel.CartDataViewModel.CartDetailDataDViewModel()
                    {
                        CartDetailIds = c.CartDetailData.CartDetailIds.Select(cd => new CartsViewModel.CartDataViewModel.CartDetailIdVM()
                        {
                            cartDetailId = cd.cartDetailId
                        }),
                        IsGroupBuy = c.CartDetailData.IsGroupBuy,
                        TotalPriceByCart = c.CartDetailData.TotalPriceByCart.ToString("0"),
                        TotalQuantityByCart = c.CartDetailData.TotalQuantityByCart.ToString("0")
                    }
                }),
                CartListByUserJson = Extensions.JsonSerialize(cartsSource.CartListByUser.ToList())
            };
            return View(cartsResult);
        }
        #endregion

        #region get cartdetails by userid and shopid
        [Authorize]
        [HttpGet]
        public IActionResult CartDetail(int id)
        {
            var userid = int.Parse(User.Identity.Name);
            
            var cartDetailSource = _orderService.GetCartDetails(userid, id);

            var cartDetail = new CartDetailViewModel();
            cartDetail.UserAccountId = userid;
            cartDetail.UserName = cartDetailSource.UserName;
            cartDetail.ShopId = cartDetailSource.ShopId;
            cartDetail.Cartid = cartDetailSource.Cartid;
            cartDetail.IsGroupBuy = cartDetailSource.IsGroupBuy;
            cartDetail.TakeMethod = "自取";
            cartDetail.StoreAddress = cartDetailSource.StoreAddress;
            cartDetail.StoreCity = cartDetailSource.StoreCity;
            cartDetail.StoreDistrict = cartDetailSource.StoreDistrict;
            cartDetail.OpenTime = cartDetailSource.OpenTime.ToString("HH:mm");
            cartDetail.CloseTime = cartDetailSource.CloseTime.ToString("HH:mm");
            cartDetail.CartShopData = new CartDetailViewModel.CartShopDataVM()
            {
                Brand = cartDetailSource.ShopData.Brand,
                BrandImg = cartDetailSource.ShopData.BrandImg,
                ShopName = cartDetailSource.ShopData.ShopName,
                ShopPhone = cartDetailSource.ShopData.ShopPhone,
                ShopPaymentType = cartDetailSource.ShopData.ShopPaymentType,
                ShopTakeMethodType = cartDetailSource.ShopData.ShopTakeMethodType
            };

            cartDetail.ProductDetailsByCart = cartDetailSource.ProductDetailsByCart.Select(cd => new CartDetailViewModel.ProductDetailsByCartVM()
            {
                UserAcountId = cd.UserAcountId,
                NickName = cd.NickName,
                Photo = cd.Photo,
                ProductDetailsByUserData = cd.ProductDetailsByUser.Select(pd => new CartDetailViewModel.ProductDetailsByCartVM.ProductDetailsByUserVM()
                {
                    NickName = pd.NickName,
                    ProductName = pd.ProductName,
                    UnitPrice = pd.UnitPrice,
                    Quantity = pd.Quantity,
                    Note = pd.Note,
                    Price = pd.Price
                }).ToList()
            });
            cartDetail.ProductDetailsByCartJson = Extensions.JsonSerialize(cartDetailSource.ProductDetailsByCart.ToList());
            //IEnumerable<ProductDetailsVM> ProductDetailsData
            //cartDetail.Products = System.Text.Json.JsonSerializer.Serialize(cartDetailSource.ProductDetailsData);
            cartDetail.ProductDetailModal = new ProductDetailModal()
            {
                BtnEnable = true,
                BtnText = "更新",
                HasSticker = cartDetailSource.HasSticker
            };                
                //Discount
                cartDetail.TotalPrice = cartDetailSource.TotalPrice.ToString("0");
                cartDetail.TotalQuantity = cartDetailSource.TotalQuantity.ToString();
                cartDetail.UserName = cartDetailSource.UserName;
                cartDetail.Phone = cartDetailSource.Phone;
                cartDetail.OpenDayList = cartDetailSource.OpenDayList;
                cartDetail.OrderStamp = cartDetailSource.OrderStamp;
            //cartDetail.IsCash = cartDetailSource.IsCash;

            cartDetail.UsableCoupons = _activityService.GetUsableCouponByUser(userid, id).Select(x => new UsableCouponByUserVM()
            {
                CouponId = x.CouponId,
                CouponTitle = x.CouponTitle
            });


            return View(cartDetail);
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateCartProductDetail([FromBody] UpdateCartProductsDataModel request)
        {
            var userid = int.Parse(User.Identity.Name);
            var cartDetailSource = _orderService.GetCartDetails(userid, request.ShopId);

            var cartDetail = new CartDetailViewModel();
            cartDetail.UserAccountId = userid;
            cartDetail.UserName = cartDetailSource.UserName;
            cartDetail.ShopId = cartDetailSource.ShopId;
            cartDetail.Cartid = cartDetailSource.Cartid;
            cartDetail.TakeMethod = "自取";
            cartDetail.StoreAddress = cartDetailSource.StoreAddress;
            cartDetail.StoreCity = cartDetailSource.StoreCity;
            cartDetail.StoreDistrict = cartDetailSource.StoreDistrict;
            cartDetail.OpenTime = cartDetailSource.OpenTime.ToString("HH:mm");
            cartDetail.CloseTime = cartDetailSource.CloseTime.ToString("HH:mm");
            cartDetail.CartShopData = new CartDetailViewModel.CartShopDataVM()
            {
                Brand = cartDetailSource.ShopData.Brand,
                BrandImg = cartDetailSource.ShopData.BrandImg,
                ShopName = cartDetailSource.ShopData.ShopName,
                ShopPhone = cartDetailSource.ShopData.ShopPhone,
                ShopPaymentType = cartDetailSource.ShopData.ShopPaymentType,
                ShopTakeMethodType = cartDetailSource.ShopData.ShopTakeMethodType
            };

            cartDetail.ProductDetailsByCart = cartDetailSource.ProductDetailsByCart.Select(cd => new CartDetailViewModel.ProductDetailsByCartVM()
            {
                UserAcountId = cd.UserAcountId,
                NickName = cd.NickName,
                Photo = cd.Photo,
                ProductDetailsByUserData = cd.ProductDetailsByUser.Select(pd => new CartDetailViewModel.ProductDetailsByCartVM.ProductDetailsByUserVM()
                {
                    NickName = pd.NickName,
                    ProductName = pd.ProductName,
                    UnitPrice = pd.UnitPrice,
                    Quantity = pd.Quantity,
                    Note = pd.Note,
                    Price = pd.Price
                }).ToList()
            });
            cartDetail.ProductDetailsByCartJson = Extensions.JsonSerialize(cartDetailSource.ProductDetailsByCart.ToList());

            cartDetail.ProductDetailModal = new ProductDetailModal()
            {
                BtnEnable = true,
                BtnText = "更新",
                HasSticker = cartDetailSource.HasSticker
            };

            cartDetail.TotalPrice = cartDetailSource.TotalPrice.ToString("0");
            cartDetail.TotalQuantity = cartDetailSource.TotalQuantity.ToString();
            cartDetail.UserName = cartDetailSource.UserName;
            cartDetail.Phone = cartDetailSource.Phone;
            cartDetail.OpenDayList = cartDetailSource.OpenDayList;
            cartDetail.OrderStamp = cartDetailSource.OrderStamp;


            cartDetail.UsableCoupons = _activityService.GetUsableCouponByUser(userid, request.ShopId).Select(x => new UsableCouponByUserVM()
            {
                CouponId = x.CouponId,
                CouponTitle = x.CouponTitle
            });
            return PartialView("_Update_CartProducts", cartDetail);
        }

        [HttpPost]
        public IActionResult CartDetail([FromBody] UpdateProductDataViewModel request)
        {
            //var inputDto = new 
            return View();
        }

        [HttpPost]
        public IActionResult CalculateCouponDiscount([FromBody] CalculateDiscountInputVM request)
        {
            var resultDto = _activityService.CalculateCouponDiscount(request.CartId, request.CouponId);

            var result = new CalculateCouponDiscountVM()
            {
                IsValid = resultDto.IsValid,
                Discount = resultDto.Discount.ToString("0.#"),
                Hint = resultDto.Hint
            };

            return Ok(result);
        }

        #endregion

        #region comment create
        //comment create
        public IActionResult Comment()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Comment([FromBody] CommentDataModel request)
        {
            var inputDto = new CreateCommentInputDto
            {
                OrderId = request.OrderId,
                Comment1 = request.Comment1,
                Star = request.Star
            };

            var createCommentResult = _orderService.CreateComment(inputDto);

            return new JsonResult(createCommentResult);
        }
        #endregion

        #region create and cancel order
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderViewModel request)
        {
            var inputDto = new AddCartDetailsInputDto()
            {
                UserAccountId = request.UserAccountId,
                CartId = request.CartId,
                //var date1 = new DateTime(2008, 5, 1, 8, 30, 52);
                // DateTime 指定 year、month、day、hour、minute 和 second 
                OrderDate = new DateTime(request.Year, request.Month, request.Day, request.Hour, request.Minutes, request.Seconds),
                PickUpTime = new DateTime(request.Pickupyear, request.Pickupmonth, request.Pickupday, request.Pickuphour, request.Pickupmin, 00),
                ShopId = request.ShopId,
                TakeMethodId = (OrderEnum.TakeMethodEnum)Enum.Parse(typeof(OrderEnum.TakeMethodEnum), request.TakeMethod),
                Message = request.Message,
                OrderState = (OrderEnum.OrderStateEnum)Enum.Parse(typeof(OrderEnum.OrderStateEnum), request.OrderState),
                PaymentType = (OrderEnum.PaymentTypeEnum)Enum.Parse(typeof(OrderEnum.PaymentTypeEnum), request.PaymentType),
                PayState = (OrderEnum.PayStateEnum)Enum.Parse(typeof(OrderEnum.PayStateEnum), request.PayState) ,
                CouponId = request.CouponId,
                City = request.City,
                District = request.District,
                Address = request.Address,
                UpdateDate = DateTime.UtcNow,
                OrderStamp = request.OrderStamp,
                VATNumber = request.VATNumber,
                FinalPrice = request.FinalPrice,
                DeliveryFee = request.DeliveryFee == null ? null : (decimal)request.DeliveryFee
            };
            var createOrderResult = _orderService.CreateOrder(inputDto);
            var random = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            if (inputDto.PaymentType == OrderEnum.PaymentTypeEnum.CreditCard)
            {
                return Json(new { Url = $"/Order/ECpay/{request.CartId}_{random}_{request.OrderStamp}" });
            }
            else 
            {
                return Json(new { Url = $"/order/cartpaybycash?orderStamp={request.OrderStamp}" });
            }

            
            
        }

        public IActionResult OrderCancelled()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderCancelled(CancelOrderDataModel request)
        {
            var inputDto = new CancelOrderDto()
            {
                OrderId = int.Parse(request.OrderId)
            };

            var orderCancel = _orderService.OrderCancelled(inputDto);

            return RedirectToAction("OrderDetail", new {id =  inputDto.OrderId});
        }


        public IActionResult CartPayByCash(string orderStamp)
        {
            var cartCompleteData = _orderService.GetCartComplete(orderStamp);

            var result = new ECpayViewModel()
            {
                OrderId = cartCompleteData.OrderId,
                PaymentDate = "現金結帳",
                TakeMethod = cartCompleteData.TakeMethod,
                OrderStamp = cartCompleteData.OrderStamp,
                PickUpTime = cartCompleteData.PickUpTime,
                ShopName = cartCompleteData.ShopName,
                ShopPhone = cartCompleteData.ShopPhone,
                BrandName = cartCompleteData.BrandName,
                isCash = cartCompleteData.isCash
            };
            return View(result);
        }

        #endregion

        #region AddProductToCart
        [HttpPost]
        [Authorize]
        public IActionResult AddProductToCart([FromBody] AddCartViewModel request)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var isGroup = request.MainAccountId != 0;
            var leaderId = request.MainAccountId;

            var inputDto = new AddProductToCartDto
            {
                LeaderId = request.MainAccountId == 0 ? currentUserId : request.MainAccountId, //若是揪團的話就會是別人的Id
                ShopId = request.ShopId,
                OrderTime = DateTime.UtcNow,
                CartDetails = new CartDetailDto
                {
                    OrderAccountId = int.Parse(User.Identity.Name),
                    NickName = request.CartDetails.NickName,
                    ProductId = request.CartDetails.ProductId,
                    UnitPrice = request.CartDetails.UnitPrice,
                    Quentity = request.CartDetails.Quentity,
                    Specs = request.CartDetails.Specs
                }
            };

            var addProductResult = _orderService.AddProductToCart(inputDto);

            ProductInCartDto updateCart = new ProductInCartDto();
            if (addProductResult.IsSuccessful == true)
            {
                updateCart = _orderService.GetAmountInCart(inputDto.LeaderId, inputDto.ShopId);
            }
            else
            {
                return StatusCode(400);
            }

            var updateCartVM = _mapper.Map<ProductInCart>(updateCart);


            if (isGroup)
            {
                if (currentUserId == leaderId)
                {
                    updateCartVM.IsLeader = true;
                    updateCartVM.MsgForGroup = "團購中";
                }
                else
                {
                    updateCartVM.IsLeader = false;
                    updateCartVM.MsgForGroup = _storeService.MsgForGroupMember(leaderId);
                }
            }

            return PartialView("_MenuCartPartial", updateCartVM);
        }
        #endregion

        #region DeleteProductFromCart
        [HttpPost]
        [Authorize]
        public IActionResult DeleteProductFromCart([FromBody] DeleteProductViewModel request)
        {
            var inputDto = new DeleteCartDetailDto()
            {
                CartDetailId = request.CartDetailId
            };

            var deleteProductResult = _orderService.DeleteProductFromCart(inputDto);
            return new JsonResult(deleteProductResult);
        }
        #endregion

        #region DeleteCart
        [HttpPost]
        [Authorize]
        public IActionResult DeleteCart([FromBody] DeleteCartVM request)
        {
            var inputDto = new DeleteCartDto()
            {
                CartId = request.CartId
            };
            var deleteCartResult = _orderService.DeleteUserCart(inputDto);
            return new JsonResult(deleteCartResult);
        }
        #endregion

        #region AddCartByGroup
        [HttpPost]
        [Authorize]
        public IActionResult AddCartByGroup([FromBody] AddCartViewModel request)
        {
            var inputDto = new AddProductToCartDto
            {
                LeaderId = int.Parse(User.Identity.Name),
                ShopId = request.ShopId,
                OrderTime = DateTime.UtcNow
            };

            var groupingResult = _orderService.AddCartByGroup(inputDto);

            return new JsonResult(groupingResult); 
        }
        #endregion

        #region ECPay
        /// <summary>
        /// 取得付款資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Order/CartS4/{orderStamp}_{cartId}")]
        [HttpPost]
        public IActionResult CartS4([FromForm] IFormCollection info, string orderStamp, int cartId)
        {
            var delete = _orderService.DataUpdate(cartId, orderStamp, info["PaymentDate"]);
            var cartCompleteData = _orderService.GetCartComplete(orderStamp);
                var result = new ECpayViewModel
                {
                    OrderId = cartCompleteData.OrderId,
                    PaymentDate = info["PaymentDate"],
                    TakeMethod = cartCompleteData.TakeMethod,
                    OrderStamp = cartCompleteData.OrderStamp,
                    PickUpTime = cartCompleteData.PickUpTime,
                    ShopName = cartCompleteData.ShopName,
                    ShopPhone = cartCompleteData.ShopPhone,
                    BrandName = cartCompleteData.BrandName,
                };
                
                return View("CartS4", result);
        }


        /// <summary>
        /// 產生訂單
        /// </summary>
        /// <returns></returns>
        [Route("/Order/ECpay/{cartId}_{random}_{orderStamp}")]
        public IActionResult ECpay(int cartId,string orderStamp)
        {
            var cartData = _orderService.GetECPayDataById(cartId);
            var AllData = _orderService.GetAllData();
            var shopData = AllData.Shops;
            var brandData = AllData.Brands;
            var shopName = shopData.FirstOrDefault(x => x.ShopId == cartData.ShopId).ShopName;
            var shopPhone = shopData.FirstOrDefault(x => x.ShopId == cartData.ShopId).ShopPhone;
            var brandId = shopData.FirstOrDefault(x => x.ShopId == cartData.ShopId).BrandId;
            var brandName = brandData.FirstOrDefault(x => x.BrandId == brandId).BrandName;
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var urlRandom = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            var website = "https://wowdin.azurewebsites.net";

            #region ECorder
            var result = new Dictionary<string, string>
                {
                //特店交易編號
                { "MerchantTradeNo",  orderId},

                //特店交易時間 yyyy/MM/dd HH:mm:ss
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

                //交易金額
                { "TotalAmount",  cartData.TotalAmount},

                //交易描述
                { "TradeDesc",  "無"},

                //商品名稱`
                { "ItemName",  $"購物車{cartId}的商品"},

                //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
                { "ExpireDate",  "3"},

                //自訂名稱欄位1
                { "CustomField1",  $"{brandName} {shopName}"},

                 //自訂名稱欄位2
                { "CustomField2",  $"{shopPhone}"},

                ////自訂名稱欄位3
                //{ "CustomField3",  "b"},

                ////自訂名稱欄位4
                //{ "CustomField4",  "c"},

                //綠界回傳付款資訊的至 此URL
                { "ReturnURL",  $"{website}/Order/CartS4/{orderStamp}_{cartId}"},

                //使用者於綠界 付款完成後，綠界將會轉址至 此URL
                { "OrderResultURL", $"{website}/Order/CartS4/{orderStamp}_{cartId}"},

                //特店編號， 2000132 測試綠界編號
                { "MerchantID",  "2000132"},

                //忽略付款方式
                { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

                //交易類型 固定填入 aio
                { "PaymentType",  "aio"},

                //選擇預設付款方式 固定填入 ALL
                { "ChoosePayment",  "Credit"},

                //CheckMacValue 加密類型 固定填入 1 (SHA256)
                { "EncryptType",  "1"},

            };

            #endregion

            result["CheckMacValue"] = GetCheckMacValue(result);
            return View(result);
        }

        /// <summary>
        /// 取得 檢查碼
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";

            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = GetSHA256(checkValue);

            return checkValue.ToUpper();
        }

        /// <summary>
        /// SHA256 編碼
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }


        #endregion

        #region GetDeliveryFee
        [HttpGet]
        [Route("/Order/DeliveryFee/{id}/{address}/{amount}")]
        public IActionResult DeliveryFee(int id,string address,decimal amount)
        {
            var feeDto = _storeService.GetDevliveryFee(id, address,amount);
            var fee=_mapper.Map<DeliveryFeeViewModel>(feeDto).JsonSerialize();
            return new JsonResult(fee);
        }
        #endregion
    }
}
