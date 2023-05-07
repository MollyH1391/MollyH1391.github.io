const menuModel = document.querySelector('#menu_item_modal')
const totalPrice = document.querySelector('#menu_modal_dollar')
const addBtn = document.querySelector('#add-quentity')
const reduceBtn = document.querySelector('#reduce-quentity')
const quentity = document.querySelector('#product-quentity')
const price = document.querySelector('#menu_modal_dollar')

let customList = []
let currentUnitPrice
let currentTotalPrice

//設定數量控制
addBtn.onclick = function () {
    if (isNaN(currentUnitPrice)){
        currentUnitPrice = parseInt(price.dataset.basicprice)
    }
    let orginQuen = parseInt(quentity.value)
    let unitPrice = currentUnitPrice
    let amendQuen = orginQuen + 1
    
    quentity.value = amendQuen
    totalPrice.innerText = (unitPrice * amendQuen).toLocaleString('en-US')
    currentTotalPrice = unitPrice * amendQuen
}

reduceBtn.onclick = function () {
    if (isNaN(currentUnitPrice)){
        currentUnitPrice = parseInt(price.dataset.basicprice)
    }
    let orginQuen = parseInt(quentity.value)
    let unitPrice = currentUnitPrice
    let amendQuen
    
    if (orginQuen > 1) {
        amendQuen = orginQuen - 1
        quentity.value = amendQuen
        totalPrice.innerText = (unitPrice * amendQuen).toLocaleString('en-US')
        currentTotalPrice = unitPrice * amendQuen
    }
}

function SetSelectedItems(p) {
    let customs = p.Note.split(' / ')
    let btns = menuModel.querySelectorAll('label')

    let btnTxt = Array.from(btns).map(x => x.innerText.split('+').map(x => x.trim())[0])


    btnTxt.forEach((b, idx) => {
            //debugger
        if (customs.includes(b)) {
            btns[idx].click()
        }
    })
    quentity.value = p.Quantity
    price.innerText = p.UnitPrice * p.Quantity
}

function SetProductDetailModal(product) {
    menuModel.querySelector('.custom-body').innerText = ''
    quentity.value = 1
    menuModel.querySelector('h3').innerText = product.Name
    menuModel.querySelector('h3').setAttribute('data-productid', product.ProductId)
    menuModel.querySelector('#menu_modal_dollar').innerText = product.BasicPrice
    menuModel.querySelector('#menu_modal_dollar').setAttribute('data-basicprice', product.BasicPrice)
    if (product.Figure != '' && product.Figure != null) {
        let imgContainer = document.querySelector('#menu_modal_product_img')
        imgContainer.innerHTML = ''
        let img = document.createElement('img')
        img.setAttribute('src', product.Figure)
        img.classList.add('img', 'w-100', 'rounded_corner_small')
        
        imgContainer.append(img)
    }

    product.Customs.forEach((cus, cusIdx) => {
        let group
        if (cus.Necessary == true) {
            group = SetCustom('radio', cus, cusIdx)
        }
        else {
            group = SetCustom('checkbox', cus, cusIdx)
        }
        menuModel.querySelector('.custom-body').append(group)
    })
}

function SetCustom(type, custom, customIdx) {
    let group = document.querySelector(`#${type}GroupTemp`).content.cloneNode(true)
    if (type == 'radio') {
        group.querySelector('.custom-title').innerText = custom.Name + '(必選)'
    }
    else if(custom.MaxAmount == null) {
        group.querySelector('.custom-title').innerText = custom.Name + '(選取數量無上限)'
    }
    else {
        group.querySelector('.custom-title').innerText = custom.Name + `(最多${custom.MaxAmount}項)`
    }

    let body = group.querySelector('.group-body')
    body.setAttribute('id', `${type}${customIdx}`)
    custom.Selections.forEach((sel, selIdx) => {
        let selection = document.querySelector(`#${type}Temp`).content.cloneNode(true)
        let input = selection.querySelector('input')
        input.setAttribute('name', `btn${type}${customIdx}`)
        input.setAttribute('id', `btn${type}${customIdx}-${selIdx}`)
        input.setAttribute('data-addprice', sel.AddPrice)

        let label = selection.querySelector('label')
        label.setAttribute('for', `btn${type}${customIdx}-${selIdx}`)
        label.innerText = sel.Name

        if (sel.AddPrice != 0) {
            label.innerText += ` + $${sel.AddPrice}`
        }

        if (custom.MaxAmount != null) {
            //限制選取數量
            label.onclick = SetMoneyAndCheckSelectAmount.bind(event, custom.MaxAmount, `btn${type}${customIdx}`)
        }

        body.append(selection)
    })
    return group
}

function SetMoneyAndCheckSelectAmount(maxAmount, searchTarget) {
    setTimeout(function () {
        currentUnitPrice = 0
        let currentSelect = menuModel.querySelectorAll(`.menu_item_selection_set_checkbox [type="checkbox"]:checked`)
        
        currentUnitPrice = parseInt(price.dataset.basicprice)
        currentSelect.forEach(sel => {
            let addPrice = parseInt(sel.dataset.addprice)
            currentUnitPrice += addPrice
        })
        //總金額增加後乘上數量
        totalPrice.innerText = (currentUnitPrice * quentity.value).toLocaleString('en-US')
        currentTotalPrice = currentUnitPrice * quentity.value

        //限制checkbox數量
        let currentAmount = currentSelect.length

        if (currentAmount == maxAmount) {
            menuModel.querySelectorAll(`[name="${searchTarget}"]:not(:checked)`).forEach(inp => {
                inp.setAttribute('disabled', true)
            })
        }
        else {
            menuModel.querySelectorAll(`[name="${searchTarget}"]:not(:checked)`).forEach(inp => {
                inp.removeAttribute('disabled')
            })
        }

    }, 50)
}

function CheckRadioSelection() {
    let radioGroups = document.querySelectorAll('.menu_item_selection_set_radio')
    let needToSelect = []

    radioGroups.forEach(set => {
        let groupTitle = set.querySelector('.custom-title').innerText
        let selectedCount = set.querySelectorAll('[type="radio"]:checked').length

        if (selectedCount == 0) {
            needToSelect.push(groupTitle)
        }
    })

    if (needToSelect.length > 0) {
        let selections = needToSelect.join('、')
        return {
            success: false,
            message: `${selections}未選`
        }
    }

    else {
        return {
            success: true,
            message: '加入購物車'
        }
    }
}

// radio1 / radio2 / checkbox1 / 單位金額 / Quentity
function GetCustomString() {
    let radios = document.querySelectorAll('.menu_item_selection_set_radio')
    let checkboxes = document.querySelectorAll('.menu_item_selection_set_checkbox')

    customList.length = 0

    radios.forEach(radio => {
        let checked = radio.querySelector(':checked + label').innerText
        customList.push(checked)
    })

    checkboxes.forEach(checkbox => {
        let checkedItems = checkbox.querySelectorAll(':checked + label')
        checkedItems.forEach(item => {
            let checked = item.innerText
            customList.push(checked)
        })
    })

    let unitPrice = `$ ${currentUnitPrice}`
    customList.push(unitPrice)

    let totalQuentity = `${quentity.value}份`
    customList.push(totalQuentity)

    return customList.join(' / ')
}