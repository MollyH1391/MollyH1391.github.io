let showBtn = document.querySelectorAll('.showBtn')
let hideBtn = document.querySelectorAll('.hideBtn')
let input = document.querySelectorAll('input[type="password"]')

window.onload = function () {
    init()
    showBtn.forEach((btn, index) => {
        btn.onclick = function () {
            clickShowBtn(index)
        }
    })

    hideBtn.forEach((btn, index) => {
        btn.onclick = function () {
            clickHideBtn(index)
        }
    })
    
}

function init() {
    hideBtn.forEach(btn => {
        btn.style.display = 'none'
    })
}

function clickShowBtn(index) {
    showBtn[index].style.display = 'none'
    hideBtn[index].style.display = ''
    input[index].setAttribute('type', 'text')
}

function clickHideBtn(index) {
    hideBtn[index].style.display = 'none'
    showBtn[index].style.display = ''
    input[index].setAttribute('type', 'password')
}

