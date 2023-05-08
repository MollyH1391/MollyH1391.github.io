let t = document.querySelector("#nav-discount-btn")

function Buttontoggle() {
    if (t.innerText == "折價券轉贈") {
        t.innerText = "退款申請";
    }
    else {
        t.innerText = "折價券轉贈";
    }
}

const couponSelectDropdownitem = document.getElementById('coupon_selet_dropdownitem')
const couponBrand = document.getElementById('coupon_brand')
const couponBrandItems = document.querySelectorAll('.brand.form-select option')
const couponCategory = document.getElementById('coupon_status')
const couponCategoryItems = document.querySelectorAll('.status.form-select option')
// let couponModalSubmitBtn
let requestData = { Brand: '全部', Status: '全部' }

window.onload = function () {
    couponBrandItems.forEach(item => {
        item.onclick = function () {
            couponBrand.value = item.innerText
            if (window.innerWidth >= 992) {
                requestData.Filter = true
                requestData.Brand = couponBrand.value
                getShopCards(requestData)
            } else {
                couponModalSubmitBtn.disabled = false
            }
        }
    })
    couponCategoryItems.forEach(item => {
        item.onclick = function () {
            couponCategory.innerText = item.innerText
            if (window.innerWidth >= 992) {
                requestData.Filter = true
                requestData.Status = couponStatus.innerText
                getShopCards(requestData)
            } else {
                couponModalSubmitBtn.disabled = false
            }
        }
    })
}

function createSubmitButton(btntext, btnclickfunc) {
    couponModalSubmitBtn = document.createElement('button')
    couponModalSubmitBtn.type = 'submit'
    couponModalSubmitBtn.classList.add('p-4', 'w-50', 'rounded-1', 'bg_pink', 'text_white', 'fw-bold')
    couponModalSubmitBtn.id = 'search_modal_submitbtn'
    couponModalSubmitBtn.disabled = true
    couponModalSubmitBtn.innerText = btntext
    couponModalSubmitBtn.onclick = btnclickfunc
}
function openFilter() {
    requestData.Filter = false
    requestData.Brand = '全部'
    requestData.Category = '全部'
    requestData.Evaluate = '全部'
    couponBrand.disabled = false
    couponCategory.disabled = false
    couponBrand.value = ''
    couponCategory.innerText = '全部'
}

const checkbtn = document.querySelector('#couponCheck')
checkbtn.onclick = function () {
    var t = document.getElementById("couponText").value
    var err = document.getElementById("getCouponModal")
    var tur = document.getElementById("getCouponModal")
    var dis = document.getElementById("discount_container")
    var cou = document.getElementById("voucher_container")
    if (!isNaN(t) == true) {
        err.innerHTML = '<div class="modal-header border-bottom-0"><strong class="me-auto">錯誤</strong><button type="button" data-bs-dismiss="modal" aria-label="Close"><i class="fas fa-times-circle text_pink fz_18"></i></button></div><div class="modal-body"><p>折扣碼輸入錯誤，請重新輸入!</p>'
    }
    else {
        tur.innerHTML = "@Html.Partial("_CouponToastPartial")"
        if (isNaN(t) == false) {
            dis.innerHTML = "@Html.Partial("_Add_Coupon_Discount")"
        }
        else {
            cou.innerHTML = "@Html.Partial("_Add_Coupon_Voucher")"
        }

    }
}


