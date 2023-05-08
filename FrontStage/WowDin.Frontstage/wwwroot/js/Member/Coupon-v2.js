const brandSelector = document.querySelector('#brand_selector')
const statusSelector = document.querySelector('#status_selector')
const discountContainer = document.querySelector('#discount_container')
const voucherContainer = document.querySelector('#voucher_container')
const discounts = document.querySelectorAll('.discount_coupon')
const vouchers = document.querySelectorAll('.voucher_coupon')

let couponBtn = document.querySelector('#coupon_btn')
let couponModalContainer = document.querySelector('#coupon_modal')

let couponModal = new bootstrap.Modal(document.getElementById('coupon_modal'), {
    keyboard: false
})

brandSelector.addEventListener('change', function(){
    CouponFilter()
})

statusSelector.addEventListener('change', function(){
    CouponFilter()
})

couponModalContainer.addEventListener('hidden.bs.modal', function (event){
    window.location.reload()
})

function CouponFilter(){
    let selectedBrand = brandSelector.selectedOptions[0].innerText
    let selectedStatus = statusSelector.selectedOptions[0].innerText
    let currentDiscounts = [... discounts]
    let currentVouchers = [... vouchers]
    if(selectedBrand != '全部'){
        currentDiscounts = currentDiscounts.filter(x => 
            x.querySelector('.shop_name').innerText.includes(selectedBrand)
        )
        currentVouchers = currentVouchers.filter(x => 
            x.querySelector('.shop_name').innerText.includes(selectedBrand)
        )
    }
    if(selectedStatus == '可使用'){
        currentDiscounts = currentDiscounts.filter(x => 
            x.querySelector('.coupon_status').innerText.includes('可使用')
        )
        currentVouchers = currentVouchers.filter(x => 
            x.querySelector('.coupon_status').innerText.includes('可使用')
        )
    }
    if (selectedStatus == '已使用') {
        currentDiscounts = currentDiscounts.filter(x =>
            x.querySelector('.coupon_status').innerText.includes('已使用')
        )
        currentVouchers = currentVouchers.filter(x =>
            x.querySelector('.coupon_status').innerText.includes('已使用')
        )
    }
    if(selectedStatus == '已過期'){
        currentDiscounts = currentDiscounts.filter(x => 
            x.querySelector('.coupon_status').innerText.includes('已過期')
        )
        currentVouchers = currentVouchers.filter(x => 
            x.querySelector('.coupon_status').innerText.includes('已過期')
        )
    }
    //debugger
    discountContainer.innerHTML = ''
    voucherContainer.innerHTML = ''
    currentDiscounts.forEach(x => {
        discountContainer.append(x)
    })
    currentVouchers.forEach(x => {
        voucherContainer.append(x)
    })
}

function GetCoupon() {
    let request = document.querySelector('#coupon_input').value.toString()
    
    if(request == ""){
        toastr.info('請輸入折扣碼')
        return
    }
    
    fetch(`/RequestForCoupon/${request}`)
    .then((res) => {
        console.log(res)

        return res.json()
    })
    .then((res) => {
        if (res.isSuccess == false) {
            toastr.warning(res.message)
        }
        else{
            fetch(`/GetCoupon/${request}`)
            .then(res => res.text())
            .then(res => {
                couponModalContainer.innerHTML = res
                toastr.success('成功兌換! 請至「我的券夾」查看')
                couponModal.show()
            })
        }
    })
}