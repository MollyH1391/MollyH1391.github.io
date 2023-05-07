// 點擊揪團產生購物車
const groupBtn = document.querySelector('#menu_group_btn')
let groupPath = document.querySelector('#menu_web_address')

groupBtn.onclick = function () {
    if (window.location.href.includes('Group')) {
        groupPath.value = window.location.href
        groupPath.setAttribute('disabled', true)
        copyToClipBoard()

        let qrcodeSrc = `https://api.qrserver.com/v1/create-qr-code/?data=${window.location.href}&size=200x200`
        document.querySelector('#group_QRcode').setAttribute('src', qrcodeSrc)

        return
    }

    let request = {
        ShopId: shopId,
        CartDetails: null
    }

    //console.log(request)

    fetch('/Order/AddCartByGroup', {
        headers: {
            'Accept': 'application/json, text/plain',
            'Content-Type': 'application/json; charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(request)
    })
        .then(response => {
            //console.log(response)
            if (response.redirected) {
                window.location.href = response.url
                return
            }

            return response.json()
        })
        .then(result => {
            //console.log(result)
            if (!result.isSuccessful) {
                toastr["waring"](result.message)
                return
                //錯誤處理尚未完善
            }

            //result為團購連結
            groupPath.value = result.message
            groupPath.setAttribute('disabled', true)
            copyToClipBoard()

            let qrcodeSrc = `https://api.qrserver.com/v1/create-qr-code/?data=${result.message}&size=200x200`
            document.querySelector('#group_QRcode').setAttribute('src', qrcodeSrc)

            //document.querySelector('#cartArea').innerHTML = result 更新購物車先不用
        })

    
}

function copyToClipBoard() {
    navigator.clipboard.writeText(groupPath.value)
    toastr["info"]("已複製連結")
}