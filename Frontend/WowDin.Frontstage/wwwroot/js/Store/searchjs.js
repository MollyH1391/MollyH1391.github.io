const searchBreadPopupmenuBtn = document.getElementById('search_bread_popupmenubtn')
const searchBreadPopupmenu = document.getElementById('search_bread_popupmenu')
const searchPopupmenuNearby = document.getElementById('search_popupmenu_nearby')
const searchPopupmenuArea = document.getElementById('search_popupmenu_area')
const searchPopupmenuOrdered = document.getElementById('search_popupmenu_ordered')
const searchAreaform = document.getElementById('search_areaform')
const searchSearchform = document.getElementById('search_searchform')
const searchSearchbar = document.getElementById('search_searchbar')
const searchSelectCity = document.getElementById('search_select_city')
const searchSelectDistrict = document.getElementById('search_select_district')
const searchSelectCitylist = document.getElementById('search_select_citylist')
const searchInputAddress = document.getElementById('search_input_address')
const searchSelectDistrictlist = document.getElementById('search_select_districtlist')
const searchSelectDropdownitem = document.getElementById('search_select_dropdownitem')
const searchBrandList = document.getElementById('search_brandlist')
const cityDistrictJsonUrl = '/JsonData/GetCityDistrict'

window.onload = function () {
    setFavorite()
    setBrandItem()
    searchBreadPopupmenuBtn.onclick = function () {
        if (!searchBreadPopupmenuBtn.din_show) {
            searchBreadPopupmenuBtn.din_show = true
            searchBreadPopupmenu.classList.remove('d-none')
        }
        else {
            searchBreadPopupmenuBtn.din_show = false
            searchBreadPopupmenu.classList.add('d-none')
        }
    }
    searchPopupmenuNearby.onclick = function () {
        searchBreadPopupmenuBtn.click()
        transToNearby()
    }
    searchPopupmenuOrdered.onclick = function () {
        searchBreadPopupmenuBtn.click()
        if (isLogin == 'True') {
            transToOrdered()
        } else if (isLogin == 'False') {
            window.location.href = '/Member/Login'
        }
    }

    searchPopupmenuArea.onclick = function () {
        searchBreadPopupmenuBtn.click()
        modalLabel.innerText = '找區域'
        modalHeader.classList.remove('border-bottom-0')
        modalBody.innerHTML = ''
        modalBody.appendChild(searchAreaform)
        searchInputAddress.disabled = searchSelectDistrict.innerText.includes('區域') ? true : false
        searchInputAddress.value = ''
        searchInputAddress.oninput = function () {
            if (searchInputAddress.value != '') {
                searchModalSubmitBtn.disabled = false
            }
        }
        createSubmitButton('搜尋', function () {
            myModal.hide()
            let city = searchSelectCity.innerText
            let district = searchSelectDistrict.innerText
            let road = searchInputAddress.value
            transToArea(city, district, road)
        })
    }
    window.onresize = function () {
        if (window.innerWidth >= 992) {
            searchSearchbar.appendChild(searchSearchform)
            myModal.hide()
        }
    }
    fetch(cityDistrictJsonUrl).then(r => r.json()).then(r => {
        r.forEach(c => {
            const cityDropdownItem = searchSelectDropdownitem.content.cloneNode(true)
            const cityDropdownItemBtn = cityDropdownItem.querySelector('button')
            cityDropdownItemBtn.innerText = c.City
            searchSelectCitylist.appendChild(cityDropdownItem)
            cityDropdownItemBtn.onclick = function () {
                searchModalSubmitBtn.disabled = false
                searchSelectCity.innerText = c.City
                searchSelectDistrict.innerText = '區域'
                searchSelectDistrictlist.innerHTML = ''
                searchSelectDistrictlist.classList.remove('d-none')
                const districtDropdownItem = searchSelectDropdownitem.content.cloneNode(true)
                const districtDropdownItemBtn = districtDropdownItem.querySelector('button')
                districtDropdownItemBtn.innerText = '全區域'
                searchInputAddress.disabled = true
                searchInputAddress.value = ''
                searchSelectDistrictlist.appendChild(districtDropdownItem)
                districtDropdownItemBtn.onclick = function () {
                    searchSelectDistrict.innerText = '全區域'
                    searchInputAddress.disabled = true
                }
                c.District.forEach(d => {
                    const districtDropdownItem = searchSelectDropdownitem.content.cloneNode(true)
                    const districtDropdownItemBtn = districtDropdownItem.querySelector('button')
                    districtDropdownItemBtn.innerText = d
                    searchSelectDistrictlist.appendChild(districtDropdownItem)
                    districtDropdownItemBtn.onclick = function () {
                        searchModalSubmitBtn.disabled = false
                        searchSelectDistrict.innerText = d
                        searchInputAddress.disabled = false
                    }
                })
            }
        })

    })
    searchBrand.onclick = function () {
        searchBrand.value = ''
        getBrandList('')
    }
    searchBrand.oninput = function () {
        let input = searchBrand.value
        var re = /[\u4e00-\u9fa5a-zA-Z0-9]/
        if (re.test(input)) {
            getBrandList(input)
        } else { getBrandList('') }
    }
}
function createSubmitButton(btntext, btnclickfunc) {
    searchModalSubmitBtn = document.createElement('button')
    searchModalSubmitBtn.type = 'submit'
    searchModalSubmitBtn.classList.add('p-4', 'w-50', 'rounded-1', 'bg_pink', 'text_white', 'fw-bold')
    searchModalSubmitBtn.id = 'search_modal_submitbtn'
    searchModalSubmitBtn.disabled = true
    modalFooter.innerHTML = ''
    modalFooter.appendChild(searchModalSubmitBtn)
    modalFooter.classList.add('justify-content-center')
    modalHeader.classList.add('justify-content-center')
    searchModalSubmitBtn.innerText = btntext
    searchModalSubmitBtn.onclick = btnclickfunc
}
function transSearchForm(data = { Method: '', Lat: 0, Lng: 0, Address: '', Brand:'', Category:'', Evaluate: '' }, filterReset = false) {
    let breadtext
    switch (data.Method) {
        case "nearby":
            breadtext = '找附近'
            break
        case "area":
            breadtext = `找區域 - ${data.Address}`
            break
        case "ordered":
            breadtext = '曾點過'
            break
    }
    window.history.replaceState(null, null, `/Search/${data.Method}/${data.Lat}/${data.Lng}/${data.Address}/${data.Brand}/${data.Category}/${data.Evaluate}`)
    getShopCards({
        Method: data.Method,
        Lat: data.Lat,
        Lng: data.Lng,
        Address: data.Address,
        Brand: data.Brand,
        Category: data.Category,
        Evaluate: data.Evaluate
    }, searchShopCardsBox)
    searchBreadPopupmenuBtn.innerText = breadtext
    requestData.Method = data.Method
    requestData.Lat = data.Lat
    requestData.Lng = data.Lng
    requestData.Address = data.Address
    if (filterReset) {
        searchBrand.value = ''
        searchCategoryText.innerText = '全部'
        searchCategoryImg.style.display = 'none'
        searchEvaluate.innerText = '全部'
        requestData.Brand = ' '
        requestData.Category = '全部'
        requestData.Evaluate = '全部'
    }
}
function transToNearby() {
    getPosition(function (position) {
        transSearchForm({
            Method: 'nearby',
            Lat: position.coords.latitude,
            Lng: position.coords.longitude,
            Address:' ',
            Brand: ' ',
            Category: '全部',
            Evaluate: '全部'
        }, true)
    })
}
function transToOrdered() {
    transSearchForm({
        Method: 'ordered',
        Lat: 0,
        Lng: 0,
        Address:' ',
        Brand: ' ',
        Category: '全部',
        Evaluate: '全部'
    }, true)
}
function transToArea(city, district, road) {
    var re = /[\u4e00-\u9fa50-9]/
    let address = `${city}${district.includes('區域') ? '' : district}${re.test(road) ? road : ''}`
    transSearchForm({
        Method: 'area',
        Lat: 0,
        Lng:0,
        Address: address,
        Brand: ' ',
        Category: '全部',
        Evaluate: '全部'
    }, true)
}
function getBrandList(input) {
    fetch(`/Store/SearchBrand/${input}`).then(r => r.text())
        .then(r => {
            searchBrandList.innerHTML = r
            setBrandItem()
        })
}