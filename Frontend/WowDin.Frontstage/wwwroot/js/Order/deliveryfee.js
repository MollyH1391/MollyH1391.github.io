//完整地址
//接外送js
let message
let isConform
let deliveryFee
const deliveryFee_dollar = document.querySelector('#deliveryFee_dollar')
const deliveryNoShow = document.querySelector('#deliveryNoShow')

function getDelivery() {
    let address = `${citySelect.value.trim()} ${disSelect.value.trim()} ${addressInput.value.trim()}`
    let totalPrice = parseFloat(summaryPrice.innerHTML, 10).toFixed(2);
    getDeliveryFee(shopId, address, totalPrice)
        .then(r => {
            message = r.Message
            isConform = r.IsConform
            deliveryFee = r.DeliveryFee
        })
        .then(r => { console.log(isConform, message, deliveryFee) })
        .then(r => {
            if (isConform == false && deliveryFee == null) {
                //alert(message)
                
                setTakeRadio()
                showWainingMsg.innerText = message
                bootstrap.Modal.getOrCreateInstance(warnDeliveryFee).show()
                return isDeliveryValid = false
            }
            else if (isConform == false && message.includes('距離過遠')) {
                //alert(message)
                setTakeRadio()
                showWainingMsg.innerText = message
                bootstrap.Modal.getOrCreateInstance(warnDeliveryFee).show()
                return isDeliveryValid = false
            }
            else if (isConform == false && message.includes('消費金額未達')) {
                //alert(message)
                setTakeRadio()
                showWainingMsg.innerText = message
                bootstrap.Modal.getOrCreateInstance(warnDeliveryFee).show()
                return isDeliveryValid = false
            }
            else if (isConform == true) {
                showCartDeliveryFee.classList.remove('d-none')
                deliveryFee_dollar.innerText = deliveryFee
                
                ResetSummaryMoneyAndAmount()
                return isDeliveryValid = true
            }
            
        })
        
}


function getDeliveryFee(shopId, address, totalAmount) {
        
    let result =fetch(`/Order/DeliveryFee/${shopId}/${address}/${totalAmount}`)
        .then(r => r.json())
        .then(r => {
            return JSON.parse(r)
        })
    return result
}

function setTakeRadio() {
    
    deliveryNoShow.classList.add('d-none')
    var radios = document.getElementsByName("flexRadioDefault-take");
    radios[0].checked = true
}
