//console.log(cartList)
cartList.forEach((c, idx) => {
    let deleteBtn = document.querySelector(`.deleteCartBtn[cartid="${c.CartId}"] `)
    console.log(deleteBtn)

    deleteBtn.onclick = (event) => {
        //console.log(c.CartId)
        
        let cartId = c.CartId
        console.log(cartId)

        let request = {
            CartId: cartId
        }
        //console.log(request)
        fetch('/Order/DeleteCart', {
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
                    toastr["success"]("刪除購物車成功")
                    window.location.reload()
                }
                else {
                    toastr["warning"]("刪除購物車失敗")
                }
            })

        event.preventDefault() //阻擋冒泡事件
    }
})