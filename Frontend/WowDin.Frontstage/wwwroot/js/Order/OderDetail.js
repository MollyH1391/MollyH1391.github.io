const stars = document.querySelectorAll(".fa-star")
const ratingValueTxt = document.querySelector("#ratingValue")
//ratingValueTxt.innerText = "請點擊星星評分"

let index //目前選到的星星
let ratingValue //評分

for (let i = 0; i < stars.length; i++) {
    stars[i].addEventListener("mouseover", function () {
        for (let j = 0; j < stars.length; j++) {
            //先reset star樣式
            stars[j].classList.add("star_opacity")
        }

        for (let j = 0; j <= i; j++) {
            stars[j].classList.remove("star_opacity")
        }
    })

    //click事件顯示星星數
    stars[i].addEventListener("click", function () {
        ratingValue = i + 1
        index = i
        console.log(ratingValue)
        ratingValueTxt.value = ratingValue
    })

    stars[i].addEventListener("mouseout", function () {
        for (let j = 0; j <= i; j++) {
            stars[j].classList.add("star_opacity")
        }

        for (let j = 0; j <= i; j++) {
            stars[j].classList.remove("star_opacity")
        }
    })
}

const commentEnum = {
    "服務態度很好": 0,
    "商品品質很棒": 1,
    "外送很準時": 2,
    "其他": 3
}
const commentBtn = document.querySelector("#commentBtns") //評論的文字按鈕
const submitBtn = document.querySelector('#submitBtn') //送出評論按鈕
const showCommentArea = document.querySelector('#showCommentArea') //整個評論區



//window onload
window.onload = function () {

    console.log(orderId)
    hideOrderId.value = orderId
    console.log(hideOrderId.value)
    
    if (isCommented == true) {
        showStars()
    }
    if (isCommented == false) {
        if (iscompleted == true) {
            showCommentArea.classList.remove('d-none')
            submitBtn.removeAttribute('disabled')
            createBtns()
        }
        else {
            submitBtn.setAttribute('disabled', '')
        }
    }

    
    
}

const OrderCancelBtn = document.querySelector('#OrderCancelBtn')
const checktoCancelOrder = document.querySelector('#checktoCancelOrder')
function askOrderCancel() {

}


let text
const floatingTextarea = document.querySelector("#floatingTextarea")
function createBtns() {
    Object.keys(commentEnum).forEach((txt, idx) => {

        const btn = document.createElement("span")
        btn.innerText = txt
        btn.classList.add("order_details_rateBtn", "px-2", "m-2", "rounded-1", "bg_btnGray", "btn")
        console.log(btn)
        commentBtn.appendChild(btn)

        btn.addEventListener("click", function () {
            

            if (btn.classList.contains("spanbg_blue")) {
                btn.classList.remove("spanbg_blue")
                btn.classList.add("bg_btnGray")
                selected = Array.from(document.querySelectorAll(".spanbg_blue")).map(x => x.innerText)
                floatingTextarea.innerText = selected.toString()
                
            }
            else {
                btn.classList.remove("bg_btnGray")
                btn.classList.add("spanbg_blue")
                text = btn.innerText
                console.log(text)
                selected = Array.from(document.querySelectorAll(".spanbg_blue")).map(x => x.innerText)
                floatingTextarea.innerText = selected.toString()
                return floatingTextarea.innerText
            }
           
        })
        
    })

    //送出評價的fetch
    const starvalueWarn = document.querySelector('#starvalueWarn')
    submitBtn.onclick = function () {

        //檢核有評價星星，可以不寫文字

        if (ratingValueTxt.value.toString() != null) {
            let request = {
                OrderId: orderId,
                Comment1: floatingTextarea.innerHTML,
                Star: ratingValueTxt.value
            }
            console.log(request)
            fetch('/Order/Comment', {
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
                    if (jsonData.isSuccessful) {
                        window.location.reload()
                        let checkResult = CheckRadioSelection()
                        toastr["success"]("訂單已評價")
                        return
                    }
                    else {
                        toastr["warning"]("評價失敗")
                    }
                })
        }
        else {
            starvalueWarn.classList.remove('d-none')
        }


    }

}


//顯示評價的星星

function showStars() {
    let commentStars = document.querySelector('.commentStars').innerText
    const showCommentStar = document.querySelector('.showCommentStar')
    for (let i = 0; i < commentStars; i++)
    {
        const starsIconCol = document.createElement("div")
        starsIconCol.classList.add("col-1", "text-center", "px-0", "mx-1")

        const starsIcon = document.createElement("i")
        starsIcon.classList.add("fas", "fa-star", "fz_18")

        starsIconCol.appendChild(starsIcon)
        showCommentStar.appendChild(starsIconCol)

    }
    
}




