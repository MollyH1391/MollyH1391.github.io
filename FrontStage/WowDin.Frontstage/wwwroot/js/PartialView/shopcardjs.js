window.onload = function () {
    const shopcardFavoriteBtns = document.querySelectorAll('.shopcard_favorite_btn')
    shopcardFavoriteBtns.forEach(btn => {
        let heart = btn.querySelector('i')
        btn.addEventListener('click', function (event) {
            if (btn.din_favorite) {
                btn.din_favorite = false
                heart.classList.replace('fas', 'far')
                heart.classList.replace('text-danger', 'text_gray')
            }
            else {
                btn.din_favorite = true
                heart.classList.replace('far', 'fas')
                heart.classList.replace('text_gray', 'text-danger')
            }
            event.preventDefault()
        })
    })
}
