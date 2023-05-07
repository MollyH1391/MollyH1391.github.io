// Initialize Swiper

var swiper = new Swiper(".menu_fig_swiper", {
    loop: true,

    pagination: {
        el: ".swiper-pagination",
        dynamicBullets: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});

var swiper2 = new Swiper(".menu_nav_swiper", {
    slidesPerView: 3.5,
    spaceBetween: 0,
});

// scroll監聽與nav的fixed設定
window.addEventListener('scroll', stickyNav)

const main = document.querySelector('.menu_main')
const topOfMain = main.offsetTop

const nav = document.querySelector('.menu_nav')

function stickyNav() {
    if (window.scrollY >= topOfMain) {
        nav.classList.add('menu_nav_fixed')
        main.classList.add('menu_nav_fixed_padding')
    }
    else {
        nav.classList.remove('menu_nav_fixed')
        main.classList.remove('menu_nav_fixed_padding')
    }
}

