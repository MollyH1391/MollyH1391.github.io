# WowDin: A Complete Online Food Ordering Platform Developed with ASP.NET Core MVC

## Table of Content

## Summary of WowDin's Development and Achievements
WowDin is an **online food ordering platform project that replicates a real-life website**, which was completed within three-month in a seven-month microsoft funded bootcamp. The project includes a platform administrator, store management, and ordering web pages, providing a complete shopping process. This article will summarize the important technical details of the **Order module** and showcase the achievements.

The project was developed using **ASP.NET Core MVC**, with a layered architecture of **Repository Pattern + Service Layer**. The front-end utilizes JavaScript and Vue, with data retrieved through API requests using Fetch/Axios, and the back-end utilizes LINQ queries for CRUD operations. Notion was used as a project management tool to enhance schedule management and team collaboration efficiency. Additionally, GitHub and Azure DevOps were used for version control and CI/CD pipeline to ensure application quality.

**Selenium** was used for web testing, with test methods written to simulate user behavior, increasing testing efficiency and reducing the likelihood of human error. Finally, Azure DevOps **blue-green deployment** was utilized to enhance rollback capability and reduce risk when adding or modifying the application.

## The design logic of Software Layered Architecture Pattern

This project adopts a **layered architecture pattern** that follows the principles of single responsibility and separation of concerns, which increases code reusability. and improves team collaboration efficiency. It consists of four layers: 
  - Presentation Layer (Controller): Handles client requests, calls the Service Layer, and returns ViewModels.
  - Business Layer (Service): Focuses on processing business logic, calls the Repository Layer, and returns Data Transfer Objects (DTOs) to the Controller.
  - Data Layer (Repository): Specializes in handling database operations.
  - Common Layer: Includes shared Enums and Exception Filters across modules
This architecture greatly facilitates the separation of concerns in the development process, minimizes conflicts in collaboration, and demonstrates significant benefits in multi-person projects.

![Software Layered Architecture](https://github.com/MollyH1391/MollyH1391.github.io/blob/14e647e9f14598559f6cddb898e06eba6d07f434/GUI/layered_architecture.PNG)

## Order Placement Process
The order process includes obtaining items from the shopping cart, filling out ordering information, choosing payment method (credit card or cash), completing the order, and customer feedback.

![Order Placement Process](https://github.com/MollyH1391/MollyH1391.github.io/blob/14e647e9f14598559f6cddb898e06eba6d07f434/GUI/order_process.PNG)

## Maintaining Data Integrity with Service Transactions and Rollback Mechanisms

The methods inside the Service use **transactions with try-catch blocks** to ensure data Atomicity and Consistency. In case of operation failure, it performs a rollback. For example, in the OrderService, the CreateOrder method needs to perform create, update, and delete actions on multiple tables to add the order and order details table, update the coupon table, and delete the shopping cart table. Each action must complete to ensure data integrity. Below is the code for the CreateOrder method:

```c#
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
```

## Using Redis to Improve Website Data Access Performance

## Using partial views to reduce duplicate code and enhance user experience
Using partial views reduces code duplication and increases flexibility, making the code more manageable, readable, and improving the user experience. For example, if an order page has many duplicate cards with only some differing data, a partial view can render and pass in different parameters for each card. Here's an example of the order card partial view code:

```html
@model WowDin.Frontstage.Models.ViewModel.Order.OrdersViewModel

@{
    ViewData["Title"] = "­q³æ";
}

	<link href="~/css//Order/Order.css" rel="stylesheet">



<div class="row">

	@if(Model.IsOrderExist)
	{
		foreach (var od in Model.OrderListByUser)
		{
			<div class="col-12 col-sm-6 col-lg-4 d-flex justify-content-center ">
				<div id="cart_card cart_order_complete" class="card mb-3 w-100">
					<a href="/Order/OrderDetail/@od.OrderId" class="text-decoration-none">
						@await Html.PartialAsync("_Order_Card_Partial", od)
					</a>
				</div>
			</div>
		}
	}
	else
	{
		<div class="col-12 d-flex">
            <img class="w-25 m-auto" src="~/img/Order/µL­q³æ.png" alt="noorder">
        </div>
	}

</div>
```

In the shopping cart page, only a portion of the page needs updating. Using partial views, users can obtain the products ordered by other members of the group without having to refresh the entire page, improving the shopping experience. Here's an example of the cart details update code:

```html
<div class="row justify-content-center mb-2">
    <div class="col-12 col-sm-10 col-lg-8">
        <div class="card rounded-1">
            <div class="card-body">
                <div class="row py-2 px-3">
                    <div class="col-12">
                        <h4 class="fz_16 fw-bolder">Cart Details</h4>
                    </div>
                </div>         
                <div id="updateCartProducts">
                    @await Html.PartialAsync("_Update_CartProducts")
                </div>
                <div class="row mt-2">
                    <div class="col-3 m-auto">
                        <button id="reloadcartBtn" type="button" class=" btn btn-outline-secondary w-100 d-flex justify-content-evenly">
                            <p class="mb-0 d-inline-block"><i class="fas fa-redo-alt text_gray"></i></p>
                            <p class="mb-0 d-inline-block">cart details update</p>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```
## Using refactoring and the AsNoTracking method to improve web page performance

## Streamlining Website Testing with Automated Selenium Testing

## Ensuring Collaborative Efficiency and Application Quality with Azure DevOps



