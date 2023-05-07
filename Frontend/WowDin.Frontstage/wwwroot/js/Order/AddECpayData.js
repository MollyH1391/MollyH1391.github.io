//放View
let request = {
    CartId: '@Model.Cartid',
    TotalPrice: '@Model.TotalPrice'
}


//放js的最後
function Pay() {
    if (flexRadio_card.checked == true) {
        getECpayData()
        //window.location.href = "https://localhost:5001/Order/CartS3"
    }
}

function getECpayData() {
    fetch('/Order/ECpay', {
        headers: {
            'Accept': 'application/json,text/plain',
            'Content-Type': 'application/json;charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(request)
    })
        .then(response => {
            console.log(response)

            window.location.href = response.url
            return
        })
        .then(result => {
            console.log(result)
        })
}