const orderBtn = document.querySelector('#go_to_cart')

//加入購物車
orderBtn.onclick = function () {
    
    if (isAuthenticated == 'false') {
        window.location.href = '/Member/Login'
        return
    }

    let checkResult = CheckRadioSelection()

    if (!checkResult.success) {
        toastr["warning"](checkResult.message)

        return
    }

    let nickNameInput = menuModel.querySelector('#menu_item_modal_input')
    let nickName = ''
    if(nickNameInput != null){
        nickName = nickNameInput.value
    }

    let request = {
        MainAccountId: leaderId,
        ShopId: shopId,
        CartDetails: 
        {
            NickName: nickName,
            ProductId: parseInt(menuModel.querySelector('.menu_modal_header h3').dataset.productid),
            UnitPrice: currentUnitPrice,
            Quentity: quentity.value,
            Specs: GetCustomString()
        }
    }

    fetch('/Order/AddProductToCart', {
        headers: {
            'Accept': 'application/json, text/plain',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(request)
    })
        .then(response => { 
            if (response.redirected) {
                window.location.href = response.url
                return
            }
            if (response.status == 400) {
                toastr["warning"]("加入購物車失敗")
                return
            }
            return response.text()
        })
        .then(result => {
            if (result != null) {
                toastr["success"](checkResult.message)
                document.querySelector('#menu_item_modal .menu_modal_header button').click()

                //更新購物車
                document.querySelector('#cartArea').innerHTML = result
            }
        })
}
