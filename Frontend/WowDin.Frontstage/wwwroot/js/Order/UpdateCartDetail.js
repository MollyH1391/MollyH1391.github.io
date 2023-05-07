
const updateBtn = document.querySelector('#go_to_cart')
updateBtn.onclick = function () {

    let checkResult = CheckRadioSelection()

    if (!checkResult.success) {
        toastr["warning"](checkResult.message)

        return
    }

    let request = {
        CartDetails:
        {
            NickName: menuModel.querySelector('#menu_item_modal_input').value,
            ProductId: parseInt(menuModel.querySelector('.menu_modal_header h3').dataset.productid),
            UnitPrice: parseInt(price.innerText) / parseInt(quentity.value),
            Quentity: quentity.value,
            Specs: GetCustomString()
        }
    }
    //Action名稱待修改
    fetch('/Order/{CartDetail}', {
        headers: {
            'Accept': 'application/json, text/plain',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(request)
    })
        //以下以Action回傳的內容而定
        .then(response => {
            if (response.redirected) {
                window.location.href = response.url
                return
            }
            if (response.status == 400) {
                toastr["warning"]("更新購物車失敗")
                return
            }
            return response.text()
        })
        .then(result => {
            if (result != null) {
                toastr["success"]("更新購物車成功")
                document.querySelector('#menu_item_modal .menu_modal_header button').click()
            }
        })
}