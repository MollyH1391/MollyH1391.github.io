let cityArray = []
//107、118行，要改成自己的選擇器名稱

//DOM
let citySelect = document.querySelector(".city_select")
let disSelect = document.querySelector(".district_select")

window.onload = function () {
    getLoginType()
    getSexType()
    getCitySource()
    citySelect.onchange = citySelectChange
    citySelect.addEventListener('change', state)
}

function getLoginType() {
    switch (loginType) {
        case 0:
            document.querySelector("#member_icon_bg").classList.add("bg-transparent")
            document.querySelector("#member_icon").style.display = "none"
            break;
        case 1:
            document.querySelector("#member_icon_bg").classList.add("fb_bg")
            document.querySelector("#member_icon").src = "/img/Member/login_fb_icon.svg"
            break;
        case 2:
            document.querySelector("#member_icon_bg").classList.add("bg-white")
            document.querySelector("#member_icon").src = "/img/Member/login_google_icon.svg"

            break;
        case 3:
            document.querySelector("#member_icon_bg").classList.add("line_bg")
            document.querySelector("#member_icon").src = "/img/Member/login_line_icon.svg"
            break;
    }
}

function getSexType() {
    switch (sexType) {
        case 0:
            document.querySelector("#male").checked = true;
            break;
        case 1:
            document.querySelector("#female").checked = true;
            break;
        default:
            document.querySelector("#none").checked = true;
    }
}

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
        option.innerText = city.City == '' ? '--請選擇縣市--' : city.City
        option.value = city.City
        citySelect.append(option)
    })

    //districtSelect
    districtArray.forEach(dis => {
        let option = document.createElement('option')
        option.innerText = dis == '' ? '--請選擇行政區--' : dis
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