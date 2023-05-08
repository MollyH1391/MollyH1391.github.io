//DOM
let searchzone = document.querySelector('.searchzone')
let citySelect = document.querySelector(".city_select")
let disSelect = document.querySelector(".district_select")
const address = document.querySelector(".address")
const shopcardFavoriteBtns = document.querySelectorAll('.shopcard_favorite_btn')
const recommendmore = document.querySelector(".brand_recommend_more")
const recommendcontainer = document.querySelector(".recommendcontainer")
//window onload

window.onload = function () {
    getCitySource()
    citySelect.onchange = citySelectChange
    citySelect.addEventListener('change', state)
    getNearbyShops(recommendcontainer)
}


//找品牌
const changeBtn = document.createElement('div')
changeBtn.classList.add('searchzone_icon', 'searchzone_borderleft', 'col-3', 'col-sm-3', 'col-lg-6', 'd-flex', 'align-items-center', 'flex-column')
changeBtn.setAttribute('type', 'button')
changeBtn.setAttribute('data-bs-toggle', "modal")
changeBtn.setAttribute('data-bs-target', '#searchzone_findbrand_modal')
const changeBtn_icon = document.createElement('i')
changeBtn_icon.classList.add('fas', 'fa-store', 'rounded-circle', 'text_blue', 'bg_white', 'd-flex', 'justify-content-center', 'align-items-center')
const changeBtn_p = document.createElement('p')
changeBtn_p.classList.add('mt-2', 'text_white')
changeBtn_p.innerText = "找品牌"

changeBtn.appendChild(changeBtn_icon)
changeBtn.appendChild(changeBtn_p)
searchzone.appendChild(changeBtn)

//Initialize Swiper
var swiper = new Swiper(".mySwiper", {
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

//找區域
function getCitySource() {
    fetch('/JsonData/GetCityDistrict')
        .then(response => response.json())
        .then(result => {
            cityArray = [{ City: '', District: [] }].concat(result)
            districtArray = ['']
            createOption(cityArray, districtArray)
            setOption()
        })
        .catch(ex => {
            console.log(ex)
        })
}

function createOption(cityArray, districtArray) {
    //citySelect
    cityArray.forEach(city => {
        let option = document.createElement('option')
        option.innerText = city.City == '' ? '--縣市--' : city.City
        option.value = city.City
        citySelect.append(option)
    })

    //districtSelect
    districtArray.forEach(dis => {
        let option = document.createElement('option')
        option.innerText = dis == '' ? '--區域--' : dis
        option.value = dis
        disSelect.append(option)
    })
}

function citySelectChange() {
    disSelect.length = 1;
    if (citySelect.selectedOptions[0].value != '') {
        let selectedCity = cityArray.find(x => x.City == citySelect.selectedOptions[0].value)

        selectedCity.District.forEach(dis => {
            let option = document.createElement('option')
            option.value = dis
            option.text = dis
            disSelect.add(option)
        })
    }
}

function state() {
    if (citySelect.selectedOptions[0].value != '') {
        disSelect.removeAttribute('disabled')
    }
    else {
        disSelect.setAttribute('disabled', '')
    }
}

let city = '--縣市--'
let district = '--區域--'

function setOption() {
    let cityOptions = document.querySelectorAll(".city_select>option")

    cityOptions.forEach(option => {
        if (option.value == city) {
            option.selected = true
        }
    })

    state()
    citySelectChange()

    let districtOptions = document.querySelectorAll(".district_select>option")

    districtOptions.forEach(option => {
        if (option.value == district) {
            option.selected = true
        }
    })
}
function getSearchAreaInfo() {
    let city = citySelect.value
    let district = disSelect.value
    let road = address.value

    getSearchAreaPage(city, district, road)
}
function getSearchOrderedInfo() {
    if (isLogin == 'True') {
        getSearchOrderedPage()
    } else if (isLogin == 'False') {
        window.location.href = '/Member/Login'
    }
}
function getSearchNearInfo() {
    getSearchNearbyPage()
}



