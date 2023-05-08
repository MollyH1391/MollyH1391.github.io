////let menuList

//比較menu_main_col中menu_borad的offsetHeight的總和，選最小的填入
const cols = document.querySelectorAll('.menu_main_col')
const menuBoradTemp = document.querySelector('#menu_borad_temp')
const menuItemTemp = document.querySelector('#menu_item_temp')
const menuNavClassTemp = document.querySelector('#menu_nav_class')
const menuNavClassDropTemp = document.querySelector('#menu_nav_class_drop')

const searchModalUl = document.querySelector('#menu_search_modal ul')
const menuNav = document.querySelector('.menu_nav_class')
const menuNavDrop = document.querySelector('.menu_dropdown_menu')

//三欄初始化
let firstColHeight
let secondColHeight
let thirdColHieght
let colHeights = [firstColHeight, secondColHeight, thirdColHieght]

function InsertBorad(menuClass, classIdx) {
    //找到目前最短的col
    CalculateColHeight()
    let sortColHeights = [...colHeights]
    let min = sortColHeights.sort((a, b) => { return a - b })[0]
    let idxOfMin = colHeights.indexOf(min)
    //插入board到畫面
    let board = SetBoardData(menuClass, classIdx)
    cols[idxOfMin].append(board)
}

function CalculateColHeight() {
    colHeights = [0, 0, 0]
    cols.forEach((col, idx) => {
        col.querySelectorAll('.menu_borad').forEach(board => {
            colHeights[idx] += board.offsetHeight
        })
    })
}

function SetBoardData(menuClass, classIdx) {
    let board = menuBoradTemp.content.cloneNode(true)
    // let searchModalUl = document.querySelector('#menu_search_modal ul')

    // 設定分類
    board.querySelector('h2').innerText = menuClass.Name
    board.querySelector('.menu_borad').setAttribute('id', `menuClass-${classIdx}`)

    // 分類加入Nav中
    SetNavClass(menuClass, classIdx)

    //加入產品
    menuClass.Products.forEach(pro => {
        let item = SetProduct(pro)
        board.querySelector('ul').append(item)

        //同時加入至search modal - item只能append一次???
        let itemForModal = SetProduct(pro)
        searchModalUl.append(itemForModal)
    })

    return board
}

function SetNavClass(menuClass, idx) {
    let navClass = menuNavClassTemp.content.cloneNode(true)
    let navClassDrop = menuNavClassDropTemp.content.cloneNode(true)

    navClass.querySelector('a').setAttribute('href', `#menuClass-${idx}`)
    navClass.querySelector('a').innerText = menuClass.Name
    menuNav.append(navClass)

    navClassDrop.querySelector('a').setAttribute('href', `#menuClass-${idx}`)
    navClassDrop.querySelector('a').innerText = menuClass.Name
    navClassDrop.querySelector('a').onclick = function () {
        document.querySelector('[for="menu_nav_control"]').click()
    }

    menuNavDrop.append(navClassDrop)
    
}

function SetProduct(product) {
    let item = menuItemTemp.content.cloneNode(true)
    if (product.Figure == null || product.Figure == "") {
        item.querySelector('.menu_borad_item_img').classList.add('d-none')
    }
    else {
        item.querySelector('.menu_borad_item_img img').setAttribute('src', product.Figure)
    }
    item.querySelector('.menu_borad_item_name').innerText = product.Name
    item.querySelector('.menu_item_price').innerText = `$ ${product.BasicPrice.toLocaleString()}`

    if (product.SellOut == true) {
        item.querySelector('.badge').innerText = '完售'
    }

    //加入事件
    item.querySelector('button').onclick = SetProductDetailModal.bind(event, product)
    
    return item
}

// 設定搜尋功能
function SetSearch(){
    const searchInput = document.querySelector('#menu-search-input')
        
    let productList = []
    menuList.forEach(unit => {
        unit.Products.forEach(product => {
            productList.push(product)
        })
    })

    searchInput.oninput = function(){
        searchModalUl.innerHTML = ''
        let filterProducts = productList.filter(x => x.Name.includes(searchInput.value))
        filterProducts.forEach(pro => {
            let itemForModal = SetProduct(pro)
            searchModalUl.append(itemForModal)
        })
    }
}


