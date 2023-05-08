const makeOrderBtn = document.querySelector("#makeOrderBtn")
let now = new Date()
let dateTime = new Date().getTime()
let nowyear = now.getUTCFullYear()
let nowmonth = now.getUTCMonth()+1
let nowday = now.getUTCDate()
let nowhour = now.getUTCHours()
let nowminutes = now.getUTCMinutes()
let nowseconds = now.getUTCSeconds()

//選到每個人訂的產品區塊
let request
makeOrderBtn.onclick = function () {

    let takeEnum = takeMethod.innerText
    let deliveryString
    let paymentStateEnum = payment.innerText
    let paymentString
    let receiptTypeEnum = receipt.innerText
    let receiptString
    //switchEnum(takeEnum, paymentStateEnum, receiptTypeEnum)


    //取餐enum轉換
    switch (takeEnum) {
        case "外送":
            deliveryString = "Delivery"
            break

        case "自取":
            deliveryString = "SelfPiclUp"
            break
    }

    //付款enum轉換
    switch (paymentStateEnum) {
        case "現金":
            paymentString = "Cash"
            break

        case "刷卡":
            paymentString = "CreditCard"
            break
    }

    //發票enum轉換
    switch (receiptTypeEnum) {
        case "紙本發票":
            receiptString = "Receipt"
            break

        case "統編":
            receiptString = "Receipt_VATnumber"
            break
    }

    //檢查外送地址
    if (deliveryString == "Delivery" && addressInput.value.trim() == '') {
        alert("請填寫外送地址")
    }

    else {
        let mealPickupTimeStringArr = showPickUpTime.value.split(":")
        let showPickUpHour = parseInt(mealPickupTimeStringArr[0])
        let showPickUpMin = parseInt(mealPickupTimeStringArr[1])

        let mealPickupDateStringArr = showPickUpDate.value.split("/")
        let showPickUpYear = parseInt(mealPickupDateStringArr[0])
        let showPickUpMonth = parseInt(mealPickupDateStringArr[1])
        let showPickUpDay = parseInt(mealPickupDateStringArr[2])
        let couponId
        if (couponSelector == null ) {
            couponId = null
        }
        else if (couponSelector.options[couponSelector.selectedIndex].value == 0)
        {
            couponId = null
        }
        //couponId = couponSelector.options[couponSelector.selectedIndex].value
        else {
            couponId = couponSelector.options[couponSelector.selectedIndex].value
        }
        
        console.log(couponId)
        let deliveryFeeForOrderCreate
        
        if (document.querySelector('input[name="flexRadioDefault-take"]:checked').value == '自取') {
            deliveryFeeForOrderCreate = null
        }
        else if (isDeliveryValid == true) {
            deliveryFeeForOrderCreate = deliveryFee
        }
        
        request = {
            UserAccountId: userAccountId,
            ShopId: shopId,
            CartId: cartId,
            Year: nowyear,
            Month: nowmonth,
            Day: nowday,
            Hour: nowhour,
            Minutes: nowminutes,
            Seconds: nowseconds,
            OrderStamp: OrderStamp,
            Pickupyear: showPickUpYear,
            Pickupmonth: showPickUpMonth,
            Pickupday: showPickUpDay,
            Pickuphour: showPickUpHour,
            Pickupmin: showPickUpMin,
            TakeMethod: deliveryString,
            Message: showmessage.value,
            OrderState: "OrderEstablished",
            PaymentType: paymentString,
            PayState: "Pending",
            CouponId: couponId,
            ReceiptType: receiptString,
            VATNumber: vatNum.innerText,
            City: citySelect.value,
            District: disSelect.value,
            Address: addressInput.value,
            DeliveryFee: deliveryFeeForOrderCreate,
            FinalPrice: summary_final_price.innerText
        }
        console.log(request)
        
        fetch('/Order/CreateOrder', {
            headers: {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            method: 'POST',
            body: JSON.stringify(request)
        })
            .then(response => response.json())
            .then(jsonData => {
                console.log(jsonData)
                if (payment.innerText == "刷卡") {
                    window.location.href = jsonData.url
                    makeOrderBtn.setAttribute("disabled", "")
                }
                else {
                    window.location.href = jsonData.url
                    makeOrderBtn.setAttribute("disabled", "")
                }
                
            })
            .catch(error => console.log(error))
    }
    
}



