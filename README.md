# WowDin: A Complete Online Food Ordering Platform Developed with ASP.NET Core MVC

## Table of Content

1. [Summary of WowDin's Development and Achievements](https://github.com/MollyH1391/MollyH1391.github.io#summary-of-wowdins-development-and-achievements)
2. [The design logic of Software Layered Architecture Pattern](https://github.com/MollyH1391/MollyH1391.github.io#the-design-logic-of-software-layered-architecture-pattern)
3. [Order Placement Process](https://github.com/MollyH1391/MollyH1391.github.io#order-placement-process)
4. [Maintaining Data Integrity with Service Transactions and Rollback Mechanisms](https://github.com/MollyH1391/MollyH1391.github.io#maintaining-data-integrity-with-service-transactions-and-rollback-mechanisms)
5. [Using partial views to reduce duplicate code and enhance user experience](https://github.com/MollyH1391/MollyH1391.github.io#using-partial-views-to-reduce-duplicate-code-and-enhance-user-experience)
6. [Using refactoring and the AsNoTracking method to improve web page performance](https://github.com/MollyH1391/MollyH1391.github.io#using-refactoring-and-the-asnotracking-method-to-improve-web-page-performance)
7. [Streamlining Website Testing with Automated Selenium Testing](https://github.com/MollyH1391/MollyH1391.github.io#streamlining-website-testing-with-automated-selenium-testing)
8. [Ensuring Collaborative Efficiency and Application Quality with Azure DevOps](https://github.com/MollyH1391/MollyH1391.github.io#ensuring-collaborative-efficiency-and-application-quality-with-azure-devops)
9. [WowDin Food Ordering Platform Website](https://github.com/MollyH1391/MollyH1391.github.io#wowdin-food-ordering-platform-website)

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

## Using partial views to reduce duplicate code and enhance user experience
Using partial views reduces code duplication and increases flexibility, making the code more manageable, readable, and improving the user experience. For example, if an order page has many duplicate cards with only some differing data, a partial view can render and pass in different parameters for each card. Here's an example of the order card partial view code:

[Orders View](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/FrontStage/WowDin.Frontstage/Views/Order/Index.cshtml)

![Orders](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/GUI/order_index.PNG)

[Order Cards Partial View](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/FrontStage/WowDin.Frontstage/Views/Shared/_Order_Card_Partial.cshtml)

In the shopping cart page, only a portion of the page needs updating. Using partial views, users can obtain the products ordered by other members of the group without having to refresh the entire page, improving the shopping experience. Here's an example of the cart details update code:

[Cart Product List View](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/FrontStage/WowDin.Frontstage/Views/Shared/_Add_Cart_S1.cshtml)

![Cart Product List](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/GUI/update_productlist.PNG)

[Card Products Partial View](https://github.com/MollyH1391/MollyH1391.github.io/blob/5616c60922cc86523f497cca4cd13f00bbab26c8/FrontStage/WowDin.Frontstage/Views/Shared/_CartDetail_GroupBuyList.cshtml)

## Using refactoring and the AsNoTracking method to improve web page performance
One of the challenges encountered in this project was the slow loading speed of a brand's order page in the backstage system. After researching, it was found that the slow performance was due to slow data retrieval. To address this, the code was refactored using **select.contains()** to retrieve the necessary order data first and then using the **AsNoTracking** method to disconnect Entity Framework tracking, resulting in improved performance. As a result, the page loading speed was improved from the original 2.9 minutes to 2.6 seconds.

```
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
                ...
            }).ToList();     

            return orderdetails;
        }
```

## Streamlining Website Testing with Automated Selenium Testing
The frontstage project utilizes Selenium for implementing automated website testing, saving testing time and reducing human errors. The main testing focuses on the following three items:
    - Login is required before ordering: if the user is not logged in, they will be redirected to the login page.
    - Multiple group-buying items can be added to the cart correctly.
    - The shopping cart amount calculation is correct.

[Selenium project code](https://github.com/MollyH1391/MollyH1391.github.io/blob/e6e4a072dab6a3415b21a647fec6dc1260fe271d/Selenium/Frontstage_Order_Process_UnitTest.cs)

**Selenium video recording**
[![WowDin Selenium Test](https://i.ytimg.com/vi/3jGH0BnLkFg/maxresdefault.jpg)](https://youtu.be/3jGH0BnLkFg "WowDin Selenium Test")

## Ensuring Collaborative Efficiency and Application Quality with Azure DevOps

This project has implemented Azure DevOps pull requests, automated builds, and blue-green deployment, which effectively improves workflow coordination and enables smooth deployment of new versions of the application to production environments. Code management and version control reduce code conflicts and simplify team development processes, thereby improving collaboration efficiency. 

In addition, blue-green deployment can smoothly deploy new versions of the application to production environments without interrupting existing services and has convenient rollback features, achieving high application availability and automating the deployment process, thereby improving development and deployment efficiency.

The following is the Azure DevOps process:
![Azure DevOps](https://github.com/MollyH1391/MollyH1391.github.io/blob/bd4c253ae0e637c2c5870607db07adaba0915c9c/GUI/pipeline_demo.gif)

The following is the pull request process:
![pull request](https://github.com/MollyH1391/MollyH1391.github.io/blob/bd4c253ae0e637c2c5870607db07adaba0915c9c/GUI/PR_demo.gif)

## WowDin Food Ordering Platform Website
Take a look at the website and see what you and your friends want to order for a meal! (It may take a bit longer the first time you visit.)

[WowDin Food Ordering Platform](https://wowdin.azurewebsites.net)