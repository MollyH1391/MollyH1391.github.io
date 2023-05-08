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

```
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


