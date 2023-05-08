function setFavorite() {
    document.querySelectorAll('.shopcard_favorite_btn').forEach(btn => {
        let heart = btn.querySelector('i')
        btn.onclick = function (event) {
            let id = btn.attributes.din_shopid.value
            if (btn.attributes.din_favorite.value == 'true') {
                fetch(`/Store/RemoveFavorite/${id}`)
                    .then(r => r.text()).then(r => {
                        if (r.includes('成功')) {
                            heart.classList.replace('fas', 'far')
                            heart.classList.replace('text-danger', 'text_gray')
                            btn.attributes.din_favorite.value = 'false'
                        }
                        toastr['info'](r)
                    })
            }
            else if (btn.attributes.din_favorite.value == 'false') {
                fetch(`/Store/AddFavorite/${id}`)
                    .then(r => r.text()).then(r => {
                        if (r.includes('成功')) {
                            heart.classList.replace('far', 'fas')
                            heart.classList.replace('text_gray', 'text-danger')
                            btn.attributes.din_favorite.value = 'true'
                        }
                        toastr['info'](r)
                    })
            }
            event.preventDefault()
        }
    })
}
function getPosition(successfunc, container) {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successfunc, error)
        function error() {
            toastr['info']('無法取得您的位置')
            if (container) {
                container.innerHTML =`<div class="d-flex flex-column align-items-center p-5">
                <img src="/img/PartialView/recommend_errorpic(1).png" alt="無法確定你的位置" width="200px" class="p-2">
                <p class="p-2 fz_16 fw-bold">無法確定你的位置，請開啟定位功能或找區域搜尋！</p>
                <p class="p-2 fz_16 fw-bold">也可選擇品牌，查看品牌下所有門市</p>
                </div>`
            }
        }
    } else {
        toastr['info']('無法取得您的位置')
        if (container) {
            container.innerHTML = `<div class="d-flex flex-column align-items-center p-5">
                <img src="/img/PartialView/recommend_errorpic(1).png" alt="無法確定你的位置" width="200px" class="p-2">
                <p class="p-2 fz_16 fw-bold">無法確定你的位置，請開啟定位功能或找區域搜尋！</p>
                <p class="p-2 fz_16 fw-bold">也可選擇品牌，查看品牌下所有門市</p>
                </div>`
        }
    }
}
function getSearchPage(data = { Method: '', Lat: 0, Lng: 0, Address: '', Brand: '', Category: '', Evaluate: '' }) {
    window.location.href = `/Search/${data.Method}/${data.Lat}/${data.Lng}/${data.Address}/${data.Brand}/${data.Category}/${data.Evaluate}`
    setFavorite()
}
function getShopCards(data = { Method: '', Lat: 0, Lng: 0, Address: '', Brand: '', Category: '', Evaluate: '' }, container) {
    container.innerHTML =`<div id="search_spinner" class="text-center d-flex align-items-center justify-content-center">
                <span>搜尋中</span>
                <div class="spinner-border text-dark ms-4" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>
`
    fetch('/Store/SearchShops', {
        headers: {
            'Accept': 'application/json,text/plain',
            'Content-Type': 'application/json;charset=UTF-8'
        },
        method: 'POST',
        body: JSON.stringify(data)
    }).then(r => r.text()).then(r => {
        container.innerHTML = r
        setFavorite()
    })
}
function getSearchNearbyPage(brandname) {
    getPosition(function (position) {
        getSearchPage({
            Method: 'nearby',
            Lat: position.coords.latitude,
            Lng: position.coords.longitude,
            Address: ' ',
            Brand: brandname ? brandname:' ',
            Category: '全部',
            Evaluate:'全部'
        })
    })
}
function getSearchOrderedPage(brandname) {
    getSearchPage({
        Method: 'ordered',
        Lat: 0,
        Lng: 0,
        Address: ' ',
        Brand: brandname ? brandname : ' ',
        Category: '全部',
        Evaluate: '全部'

    })
}
function getSearchAreaPage(city, district, road, brandname) {
    var re = /[\u4e00-\u9fa50-9]/
    let address = `${city}${district.includes('區域') ? '' : district}${re.test(road) ? road : ''}`
    getSearchPage({
        Method: 'area',
        Lat: 0,
        Lng:0,
        Address: address,
        Brand: brandname ? brandname : ' ',
        Category: '全部',
        Evaluate: '全部'
    })
}
function getNearbyShops(container) {
    getPosition(function (position) {
        let data = {
            Method: 'nearby',
            Lat: position.coords.latitude,
            Lng: position.coords.longitude,
        }
        fetch('/Store/NearbyShopCards', {
            headers: {
                'Accept': 'application/json,text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            method: 'POST',
            body: JSON.stringify(data)
        }).then(r => r.text()).then(r => {
            container.innerHTML = r
            setFavorite()
        })
    }, container)
}
function getBrandShops(brandname, container) {
    getShopCards({
        Method: 'all',
        Brand: brandname
    }, container)
}
