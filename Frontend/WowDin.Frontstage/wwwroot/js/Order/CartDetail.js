
let searchzone = document.querySelector('.searchzone')
let citySelect = document.querySelector(".city_select")
let disSelect = document.querySelector(".district_select")
const messageBtn = document.querySelector("#messageBtn")
const messageInput = document.querySelector('#messageInput')
const showmessage = document.querySelector('#showmessage')
const showDeliveryAddress = document.querySelector('#showDeliveryAddress')
const addressInput = document.querySelector('#addressInput')
const addressnote = document.querySelector('#addressnote')
const searchzone_finddistrict_btn = document.querySelector('.searchzone_finddistrict_btn')
const temp = document.querySelector("#productListByUser")
const flexRadioDefault_take = document.querySelector("#flexRadioDefault_take")
const flexRadioDefaul_delivery = document.querySelector("#flexRadioDefault-delivery")
const take_map = document.querySelector("#take_map")
const delivery_address = document.querySelector("#delivery_address")
const flexRadio_card = document.querySelector("#flexRadio-card")

const showPickUpTime = document.querySelector('#showPickUpTime')
const showPickUpDate = document.querySelector('#showPickUpDate')



//calendar
//今天--一進來要看到現在的日期
const today = new Date()

//今天的 -- 年月日
let year = today.getFullYear()
let month = today.getMonth()
let date = today.getDate()

let currentTodoIndex

//DOM
const currentYear = document.querySelector('#Year')
const currentMonth = document.querySelector('#Month')
const dateArea = document.querySelector('tbody');


const checkTimePickUp = document.querySelector('#checkTimePickUp')
let storeOpenHour = parseInt(shopOpenTime.split(':')[0]) + 1
let storeCloseHour = parseInt(shopCloseTime.split(':')[0]) - 1


window.onload = function () {
    shopTakeMethodShow()
    shopPaymentTypeShow()
    getCitySource()
    citySelect.onchange = citySelectChange
    citySelect.addEventListener('change', state)
    init()
    checkAddress() //檢查外送地址
    
    //下拉時間選單
    //預設是當天取餐，所以下拉選單是當天時間之後到結束時間之前
    getHour(nowHour, storeCloseHour)
    GetMinutes(startMin, closeMin)

    partialControl() //檢查訂單第一步
    check()//檢查訂單第二步
    backToS2()
    continueAdd()
    SetCouponSelector()
    ResetContentMoneyAndAmount()
    ResetSummaryMoneyAndAmount()
    
}

//判斷店家取餐方式和付款方式
//console.log(shopPaymentType, shoptakeMethodType)
const selftakeNoShow = document.querySelector('#selftakeNoShow')
const showCash = document.querySelector('#showCash')
const showCredit = document.querySelector('#showCredit')
function shopTakeMethodShow() {
    if (shoptakeMethodType.indexOf('自') == -1) {
        //不能自取
        selftakeNoShow.classList.add('d-none')
        take_map.classList.add('d-none')
    }
    else if (shoptakeMethodType.indexOf('外') == -1) {
        //不能外送
        deliveryNoShow.classList.add('d-none')
        delivery_address.classList.add('d-none')
    }
}

function shopPaymentTypeShow() {
    if (shopPaymentType.indexOf('現') == -1) {
        //不能現金
        showCash.classList.add('d-none')
    }
    else if (shopPaymentType.indexOf('信') == -1) {
        //不能信用卡
        showCredit.classList.add('d-none')
    }
}


//部分更新購物車
const reloadcartBtn = document.querySelector('#reloadcartBtn')
const updateCartProducts = document.querySelector('#updateCartProducts')
reloadcartBtn.addEventListener("click", function () {
    //location.reload()
    updateGroupCartProduct()
})
function updateGroupCartProduct() {
    let request = {
        ShopId: shopId
    }
    console.log(request)
    
    fetch('/Order/UpdateCartProductDetail', {
        headers: {
            'Accept': 'application/json, text/plain',
            'Content-Type': 'application/json;charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(request)
    })
        .then(response => {
            return response.text()
        })
        .then(result => {
            if (result != null) {
                //更新購物車
                updateCartProducts.innerHTML = result
                
                //console.log(result)
                GetDiscount()
                ResetContentMoneyAndAmount()
                ResetSummaryMoneyAndAmount()
                toastr["success"]("購物車更新成功")
            }
        })
        .catch(error => console.log(error))
}




messageBtn.addEventListener('click', function () {
    showmessage.value = messageInput.value
})

flexRadioDefault_take.addEventListener('click', function () {
    take_map.classList.remove("d-none")
    take_map.classList.add("d-block")
    delivery_address.classList.remove("d-block")
    delivery_address.classList.add("d-none")
})

flexRadioDefaul_delivery.addEventListener('click', function () {
    delivery_address.classList.remove("d-none")
    delivery_address.classList.add("d-block")
    take_map.classList.add("d-none")
    take_map.classList.remove("d-block")
})
searchzone_finddistrict_btn.addEventListener('click', function () {
    showDeliveryAddress.value = `${citySelect.value} ${disSelect.value} ${addressInput.value} ${addressnote.value}`
})



userList.forEach((u, idx) => {

    //編輯產品按鈕
    //let pencils = document.querySelectorAll(`[userid="${u.UserAcountId}"] .editproduct`)
    //u.ProductDetailsByUser.forEach((p, index) => {
    //    pencils[index].onclick = function () {
    //        SetProductDetailModal(p.Product)
    //        SetSelectedItems(p)
            
    //    }
    //})

    //刪除產品按鈕
    let deleteBtns = document.querySelectorAll(`[userid="${u.UserAcountId}"] .deleteproduct`)
    let cartDetailId
    u.ProductDetailsByUser.forEach((p, index) => {
        deleteBtns[index].onclick = function () {
            let showPerProduct = document.querySelectorAll(`[userid="${u.UserAcountId}"] .showPerProduct`)
            showPerProduct[index].classList.add("d-none")
            cartDetailId = p.CartDetailId
            console.log(cartDetailId)
            
            let request = {
                CartDetailId: cartDetailId
            }
            console.log(request)
            fetch('/Order/DeleteProductFromCart', {
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json; charset=UTF-8'
                },
                method: 'POST',
                body: JSON.stringify(request)
            })
                .then(response => response.json())
                .then(jsonData => {
                    
                    if (jsonData.isSuccessful) {
                        let checkResult = CheckRadioSelection()
                        toastr["success"]("刪除商品成功")
                        //window.location.reload()
                        GetDiscount()
                        ResetContentMoneyAndAmount()
                        ResetSummaryMoneyAndAmount()
                        if (document.querySelector('input[name="flexRadioDefault-take"]:checked').value == '外送') {
                            getDelivery()
                        }

                        location.reload()
                    }
                    else
                    {
                        toastr["warning"]("刪除商品失敗")
                    }
                })
        }
    })

})

const summaryPrice = document.querySelector('#summary_price')
const summaryAmount = document.querySelector('#summary_amount')
const summaryFinal = document.querySelector('#summary_final_price')
let discountDollar
let totalDollar
let totalAmount

function ResetContentMoneyAndAmount(){
    
    totalDollar = 0
    totalAmount = 0

    let users = document.querySelectorAll('.ordersbyUser')
    users.forEach(x => {
        let totalDollarOfUser = 0
        let totalAmountOfUser = 0
        
        const contentSumDollar = x.querySelector('#buying_content_sum_dollar')
        const contentSumAmount = x.querySelector('#buying_content_sum_amount')
        let currentProducts = x.querySelectorAll('.showPerProduct:not(.d-none)')
        currentProducts.forEach(x => {
            
            let note = x.querySelector('.product_note').innerText
            let startIdx = note.lastIndexOf('$') + 1
            let endIdx = note.indexOf('份') 
            let dollarAndAmount = note.substring(startIdx, endIdx).split('/')
            
            let dollar = dollarAndAmount[0].trim()
            let amount = dollarAndAmount[1].trim()
            
            totalDollarOfUser += parseInt(dollar) * parseInt(amount)
            totalAmountOfUser += parseInt(amount)

        })
        totalDollar += totalDollarOfUser
        totalAmount += totalAmountOfUser
        
        contentSumDollar.innerText = totalDollarOfUser.toLocaleString('en-US')
        contentSumAmount.innerText = totalAmountOfUser
    })
}
    

function ResetSummaryMoneyAndAmount(){
    discountDollar = document.querySelector('#discount_dollar').innerText
    summaryPrice.innerText = totalDollar.toLocaleString('en-US')
    summaryAmount.innerText = totalAmount
    if (deliveryFee == null || deliveryFee == undefined) {
        summaryFinal.innerText = (parseInt(totalDollar) - parseInt(discountDollar)).toLocaleString('en-US')
    }
    else if (deliveryFee != null) {
        summaryFinal.innerText = (parseInt(totalDollar) - parseInt(discountDollar) + parseInt(deliveryFee)).toLocaleString('en-US')
    }
    
}

//找區域
function getCitySource() {
    fetch('/JsonData/GetCityDistrict')
        .then(response => response.json())
        .then(result => {
            cityArray = [{ City: '', District: [] }].concat(result)
            districtArray = ['']
            createOption(cityArray, districtArray)
            setOption()
        })
        .catch(ex => {
            console.log(ex)
        })
}

function createOption(cityArray, districtArray) {
    //citySelect
    cityArray.forEach(city => {
        let option = document.createElement('option')
        option.innerText = city.City == '' ? '--縣市--' : city.City
        option.value = city.City
        citySelect.append(option)
    })

    //districtSelect
    districtArray.forEach(dis => {
        let option = document.createElement('option')
        option.innerText = dis == '' ? '--區域--' : dis
        option.value = dis
        disSelect.append(option)
    })
}

function citySelectChange() {
    disSelect.length = 1;
    if (citySelect.selectedOptions[0].value != '') {
        let selectedCity = cityArray.find(x => x.City == citySelect.selectedOptions[0].value)

        selectedCity.District.forEach(dis => {
            let option = document.createElement('option')
            option.value = dis
            option.text = dis
            disSelect.add(option)
        })
    }
}

function state() {
    if (citySelect.selectedOptions[0].value != '') {
        disSelect.removeAttribute('disabled')
    }
    else {
        disSelect.setAttribute('disabled', '')
    }
}

function setOption() {
    let cityOptions = document.querySelectorAll(".city_select>option")

    cityOptions.forEach(option => {
        if (option.value == city) {
            option.selected = true
        }
        else {
            //option.setAttribute('disabled', '')
        }

    })

    state()
    citySelectChange()

    let districtOptions = document.querySelectorAll(".district_select>option")

    districtOptions.forEach(option => {
        if (option.value == district) {
            option.selected = true
        }
        else {
            //option.setAttribute('disabled', '')
        }
    })
}

const cartS1 = document.querySelector('#CartS1')
const cartS2 = document.querySelector('#CartS2')
const num2 = document.querySelector('#num2')
const partialControlBtn = document.querySelector('#partialControlBtn')
//partialControl()
let takemethodval
let msg

//購物車第一頁的驗證
function checkAddress() {
    
    checkAddress.onclick = function () {
        //選外送
        let hasError = false
        let values = {} //存最後結果
        const elementAddress = document.querySelectorAll('.addressrequired')
        for (element of elementAddress) {
            if (addressInput) {
                values[addressInput.name] = addressInput.value
                if (!addressInput.value) {
                    // 只是控制他顯示不顯示
                    element.classList.remove('d-none')
                    hasError = true
                    alert("請填寫詳細地址")
                } else {
                    element.classList.add('d-none')
                    element.classList.remove('addressrequired')
                }
            }
        }
       
    }
}

let checkAddressBtn = document.querySelector('#checkAddress')
let showCartDeliveryFee = document.querySelector('#showCartDeliveryFee')
checkAddressBtn.addEventListener("click", function () {
    checkAddressCorrectly()
})
function checkAddressCorrectly() {
    let hasError = false
    let values = {} //存最後結果
    const elementAddress = document.querySelectorAll('.addressrequired')
    for (element of elementAddress) {
        if (addressInput) {
            values[addressInput.name] = addressInput.value
            if (!addressInput.value) {
                // 只是控制他顯示不顯示
                element.classList.remove('d-none')
                hasError = true
                alert("請填寫詳細地址")
            } else {
                element.classList.add('d-none')
                element.classList.remove('addressrequired')
                
                getDelivery()
            }
        }
    }
}



//檢查訂單第一步
let isDeliveryValid
const warnDeliveryFee = document.querySelector('#warnDeliveryFee')
const showWainingMsg = document.querySelector('#showWainingMsg')
function partialControl() {
    
    partialControlBtn.addEventListener("click", function () {
        
            //前端驗證
            let hasError = false
            let values = {} //存最後結果

            const elements = document.querySelectorAll('.required')
            for (element of elements) {
                const radios = document.querySelectorAll('input[name="flexRadioDefault-take"]')
                if (!radios.length) continue
                let hasValue = [...radios].some(radio => radio.checked) //hasValue 檢視欄位是否驗證不過
                if (!hasValue) { //沒選
                    element.classList.remove('d-none')
                    alert("請選擇取餐方式")
                    hasError = true
                } else { //有選

                    element.classList.add('d-none')
                    element.classList.remove('required')

                }

            }


            //時間驗證
            const timeelement = document.querySelector('.timerequired')
            if (showPickUpDate.value.trim() == '' || showPickUpTime.value == '') {
                timeelement.classList.remove('d-none')
                alert("請選擇取餐時間")

            }
            else {
                showFinalQuantityAndPrice()
                timeelement.classList.add('d-none')
                timeelement.classList.remove('required')
                
                //檢查購物車內是否有商品
                if (summaryAmount.innerText == '0') {
                    toastr.warning("購物車內沒有商品")
                }
                else {
                    if (document.querySelector('input[name="flexRadioDefault-take"]:checked').value == '外送') {
                        getDelivery()
                        if (isDeliveryValid == true) {
                            cartS1.classList.add('d-none')
                            cartS2.classList.remove('d-none')
                            cartS2.classList.add('d-block')
                            num2.classList.remove('bg_stepNum')
                            num2.classList.add('bg_blue')
                        }
                        else if (isDeliveryValid == false) {
                            showWainingMsg.innerText = message
                            bootstrap.Modal.getOrCreateInstance(warnDeliveryFee).show()
                        }

                    }
                    cartS1.classList.add('d-none')
                    cartS2.classList.remove('d-none')
                    cartS2.classList.add('d-block')
                    num2.classList.remove('bg_stepNum')
                    num2.classList.add('bg_blue')
                }
            }
    })
    
}

//繼續加購1
const continueBtn = document.querySelector('#continueBtn')
function continueAdd() {

    continueBtn.addEventListener("click", function () {
        cartS1.classList.remove('d-none')
        cartS2.classList.add('d-none')
        cartS2.classList.remove('d-block')
        num2.classList.remove('bg_blue')
        num2.classList.add('bg_stepNum')
        partialControl() //再執行一次驗證
        //location.reload()
    })
}

//繼續加購2
const backS2 = document.querySelector('#backS2')
function backToS2() {
    backS2.addEventListener("click", function () {
        cartS3.classList.remove('d-block')
        cartS3.classList.add('d-none')
        cartS2.classList.remove('d-none')
        cartS2.classList.add('d-block')
        num3.classList.remove('bg_blue')
        num3.classList.add('bg_stepNum')
        check()
    })
    
    
}

//判斷是否顯示統編欄位
var receiptTypeArr = document.getElementsByName("flexRadio-receipt");
receiptTypeArr[0].onclick = function () {

    //vatRow.classList.remove('d-block')
    vatRow.classList.add('d-none')
};
receiptTypeArr[1].onclick = function () {
    
    vatRow.classList.remove('d-none')
    //vatRow.classList.add('d-block')
};




const checkDetailBtn = document.querySelector('#checkDetail')
const cartS3 = document.querySelector('#CartS3')
const num3 = document.querySelector('#num3')
const userName = document.querySelector('#userName')
const name = document.querySelector('#name')
const pickUpPhone = document.querySelector('#pickUpPhone')
const phone = document.querySelector('#phone')
const takeMethod = document.querySelector('#takeMethod')
const deliveryTime = document.querySelector('#deliveryTime')
const payment = document.querySelector('#payment')
const showAddress = document.querySelector('#showAddress')
const vatNum = document.querySelector('#vatNum')
const VAT = document.querySelector('#VAT')
const receipt = document.querySelector('#receipt')

//檢查訂單第二步
const userinfo = document.querySelector('.userinforequired')
function check() {
    checkDetailBtn.addEventListener("click", function () {

        //檢查付款方式
        const elements = document.querySelectorAll('.payrequired')
        for (element of elements) {

            const radios = document.querySelectorAll('input[name="flexRadio-payment"]')
            if (!radios.length) continue
            let hasValue = [...radios].some(radio => radio.checked) //hasValue 檢視欄位是否驗證不過
            if (!hasValue) {
                element.classList.remove('d-none')
                alert("請選擇付款方式")
                hasError = true
            } else {
                element.classList.add('d-none')
                element.classList.remove('payrequired')
            }

        }

        //檢查發票方式
        const receiptelements = document.querySelectorAll('.receiptrequired')
        for (element of receiptelements) {

            const radios = document.querySelectorAll('input[name="flexRadio-receipt"]')
            if (!radios.length) continue
            let hasValue = [...radios].some(radio => radio.checked) //hasValue 檢視欄位是否驗證不過
            if (!hasValue) {
                element.classList.remove('d-none')
                alert("請選擇發票方式")
                hasError = true
            } else {
                element.classList.add('d-none')
                element.classList.remove('payrequired')
            }

        }
        //檢查取餐人姓名電話
        if (name.value.trim() == '' || phone.value.trim() == '') {
            alert("請填寫取餐人資料")
            userinfo.classList.remove('d-none')
        }
        else {
            userinfo.classList.add('d-none')
            userinfo.classList.remove('userinforequired')

            cartS2.classList.add('d-none')
            cartS3.classList.remove('d-none')
            cartS3.classList.add('d-block')
            num3.classList.remove('bg_stepNum')
            num3.classList.add('bg_blue')

            userName.innerText = name.value
            pickUpPhone.innerText = phone.value
            takeMethod.innerText = document.querySelector('input[name="flexRadioDefault-take"]:checked').value

            deliveryTime.innerText = `${showPickUpDate.value.toString()} ${showPickUpTime.value.toString()}`
            payment.innerText = document.querySelector('input[name="flexRadio-payment"]:checked').value
            receipt.innerText = document.querySelector('input[name="flexRadio-receipt"]:checked').value
            if (takeMethod.innerText == "外送") {
                showAddress.innerText = showDeliveryAddress.value
            }
            else {
                showAddress.innerText = "店家自取"
            }

            vatNum.innerText = VAT.value
        }
    })
    
}



let checkprorderDate 
//calendar
function init() {
    //每次開始渲染前要把原本內容清空
    dateArea.innerHTML = ''
    currentYear.innerText = year
    currentMonth.innerText = month + 1
    //一看到行事曆就要顯示幾年幾月

    //這個月第一天是禮拜幾
    let firstDay = new Date(year, month, 1).getDay()

    //這個月有幾天(找到這個月的最後一天，就知道這個月一共有幾天)
    //month + 1 下個月的第一天
    //用 new Date(2022, 0+1, 0)去console測試
    let dayOfMonth = new Date(year, month + 1, 0).getDate()

    let day = 1 //每個月從1號開始長出來
    let rows = Math.ceil((dayOfMonth + firstDay) / 7)
    //總天數+第一天是禮拜幾，除以7，就知道要長幾列
    //Math.ceil無條件進位 / floor 是無條件捨去

    for (let row = 0; row < rows; row++) {
        let tr = document.createElement('tr')
        for (let col = 0; col < 7; col++) {
            let td = document.createElement('td')
            if (row == 0 && col < firstDay) {
                //上個月
                //td.innerText = 'A'
            }
            else {
                if (day <= dayOfMonth) {
                    //這個月
                    let btn = document.createElement('button')
                    btn.classList.add('py-1', 'px-2')
                    btn.innerText = day
                    btn.setAttribute('data-bs-dismiss', 'modal')
                    btn.setAttribute('aria-label', 'Close')
                    
                    td.append(btn)
                    if (btn.innerText == date && parseInt(currentYear.innerText) == today.getFullYear() && parseInt(currentMonth.innerText) == today.getMonth() + 1
                        || btn.innerText == date + 1 && parseInt(currentYear.innerText) == today.getFullYear() && parseInt(currentMonth.innerText) == today.getMonth() + 1
                        || btn.innerText == date + 2 && parseInt(currentYear.innerText) == today.getFullYear() && parseInt(currentMonth.innerText) == today.getMonth() + 1) {
                        btn.classList.add('btn', 'bg_blue', 'rounded-5', 'text_white', 'fw-bloder')
                        // 判斷當日是否有營業，且在營業時間內，顯示今日往後+3日亮起來
                        
                        if (now.getHours() > parseInt(shopCloseTime.split(':')[0]) || now.getHours() < parseInt(shopOpenTime.split(':')[0])) {
                            if (btn.innerText == date && parseInt(currentYear.innerText) == today.getFullYear() && parseInt(currentMonth.innerText) == today.getMonth() + 1) {
                                btn.setAttribute('disabled', 'disabled')
                                btn.classList.remove('btn', 'bg_blue', 'rounded-5', 'text_white', 'fw-bloder')
                            }
                        }

                        btn.addEventListener('click', function () {
                            showPickUpDate.value = `${currentYear.innerText}/${currentMonth.innerText}/${btn.innerText}`
                            checkprorderDate = showPickUpDate.value.split("/")[2]
                            
                            if (checkprorderDate != today.getDate()) {
                                getHour(storeOpenHour - 1, storeCloseHour)
                                GetMinutes(startMin, closeMin)
                            }
                            else {
                                showPickUpTime.value =''
                                getHour(nowHour, storeCloseHour)
                                GetMinutes(startMin, closeMin)
                                
                            }
                        })
                        // 當日沒有營業/當日營養時間已過，則把最近三個營業日日期亮起來，時間只能選擇有營業的區間
                    }
                    else {
                        btn.setAttribute('disabled', 'disabled')
                    }
                }
                else {
                    //下個月
                    //td.innerText = 'B'
                }
                day++
            }
            tr.appendChild(td)
        }
        dateArea.appendChild(tr)
    }
}
//month+1或-1就可以控制上個月和下個月 
function previousMonth() {
    month--
    //0 - -1 = -1 是去年，所以年份要-1
    if (month == -1) {
        month = 11
        year--
    }
    init()
}

function nextMonth() {
    month++
    //month ==12 表示是下一年的1月 所以month=0 年份要+1
    if (month == 12) {
        month = 0
        year++
    }
    init()
}




//取餐時間選擇

let startMin = 0
let closeMin = 59
let nowHour = today.getHours() + 1

const timeSelect = document.querySelector('#timeSelect')
function getHour(openHour, closeHour) {
    let numArray = []
    let hourArray = []
    for (let i = openHour; i <= closeHour; i++) {
        numArray.push(i)
    }
    console.log(numArray)
    hourArray = numArray.map(numberToOption)
    console.log(hourArray)

    if (hourArray == []) {
        alert("營業時間已過")
    }

    timeSelect.innerHTML = `
            <select class="form-select w-100" id="hourSelect">
                ${hourArray.join("")}
            </select>
            `
}

const minSelect = document.querySelector('#minSelect')
function GetMinutes(startMin, closeMin) {
    let tempminArray = []
    let minArray = []
    for (let i = startMin; i <= closeMin; i += 30) {

        tempminArray.push(i)
    }
    //console.log(tempminArray)
    minArray = tempminArray.map(numberToOption)
    minArray = ["<option value=\"30\">30</option>", "<option value=\"00\">00</option>"]
    console.log(minArray)

    minSelect.innerHTML = `
            <select class="form-select w-100" id="minsSelect">
                ${minArray.join("")}
            </select>
            `
}

function numberToOption(number) {
    const padded = number.toString().padStart(2, "0");

    return `<option value="${padded}">${padded}</option>`;
}


checkTimePickUp.addEventListener("click", function () {
    showPickUpTime.value = `${hourSelect.value} : ${minsSelect.value}`
    if (hourSelect.value == '') {
        toastr.warning('未選擇取餐時間或營業時間已過')
        partialControlBtn.setAttribute("disabled", "")
    }
    else {
        partialControlBtn.removeAttribute("disabled", "")
    }
})


const couponSelector = document.querySelector('#coupon_selector')
function SetCouponSelector() {
    
    if (couponSelector != null) {
        couponSelector.addEventListener('change', function () {
            GetDiscount()
        })
    }
}

function GetDiscount() {
    let request = {
        CartId: cartId,
        CouponId: parseInt(couponSelector.selectedOptions[0].value)
    }
    fetch('/Order/CalculateCouponDiscount', {
        headers: {
            'Accept': 'application/json, text/plain',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        method: 'POST',
        method: 'POST',
        body: JSON.stringify(request)
    })
        .then(res => {
            if (res.status != 200) {
                toastr.warning('折價券讀取失敗')
                return
            }
            return res.json()
        })
        .then(res => {
            console.log(res)
            let discountDollar = document.querySelector('#discount_dollar')
            discountDollar.innerText = res.discount

            ResetSummaryMoneyAndAmount()
            if (couponSelector.selectedOptions[0].value == "0") {
                return
            }
            if (res.isValid) {
                toastr.success('成功套用折價券')
            }
            else {
                toastr.info('不符合折價條件')
                couponSelector.selectedIndex = 0
            }
        })
}


const showFinalPrice = document.querySelector('#showFinalPrice')
const showFinalQuantity = document.querySelector('#showFinalQuantity')
const step3FinalPrice = document.querySelector('#step3FinalPrice')
function showFinalQuantityAndPrice() {

    //前端計算的數量
    showFinalQuantity.innerText = `${summaryAmount.innerText} 份`
    //前端計算的價格
    showFinalPrice.innerText = `$ ${summaryFinal.innerText} 元`
    step3FinalPrice.innerText = `$ ${summaryFinal.innerText} 元`

    //前端顯示的折扣
}