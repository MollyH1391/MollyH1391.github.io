const appleList = [
    {
        type: 'iPadAir',
        name: 'iPadAir',
        price: 18900,
        mainImg: './pic/ipad_air_all.jpg',
        colorList: [
            {name: '太空灰色', color: 'rgb(177,180,182)',colorImg: './pic/space_grey.png', img:'./pic/ipad-air-spacegray.png'},
            {name: '銀色', color: 'rgb(224,225,224)', img:'./pic/ipad-air-silver.png'},
            {name: '玫瑰金色', color: 'rgb(236,203,196)', img:'./pic/ipad-air-rosegold.png'},
            {name: '綠色', color: 'rgb(204,221,201)', img:'./pic/ipad-air-green.png'},
            {name: '天藍色', color: 'rgb(202,217,229)', img:'./pic/ipad-air-blue.png'}
        ],
        spec:[
            {
                name: '儲存裝置',
                specDetails:[
                    {
                        name: '64GB',
                        fit: '0'
                    },
                    {
                        name: '128GB',
                        fit: '5000'
                    }
                ]
            },
            {
                name: '連線能力',
                specDetails:[
                    {
                        name: 'Wi-Fi',
                        fit: 0
                    },
                    {
                        name: 'Wi-Fi + 行動網路',
                        fit: '4300'
                    }
                ]
            }
        ]
    },
    {
        type: 'iPad',
        name: 'iPad',
        price: 10500,
        mainImg: './pic/ipad-all.jpg',
        colorList: [
            {name: '太空灰色', color: 'rgb(177,180,182)', img:'./pic/ipad-space.png'},
            {name: '銀色', color: 'rgb(224,225,224)', img:'./pic/ipad-silver.png'},
        ],
        spec:[
            {
                name: '儲存裝置',
                specDetails:[
                    {
                        name: '64GB',
                        fit: '0'
                    },
                    {
                        name: '128GB',
                        fit: '4500'
                    }
                ]
            },
            {
                name: '連線能力',
                specDetails:[
                    {
                        name: 'Wi-Fi',
                        fit: 0
                    },
                    {
                        name: 'Wi-Fi + 行動網路',
                        fit: '4000'
                    }
                ]
            }
        ]
    }
]

//DON 宣告
const navbar = document.querySelector('.nav-bar')
const productType = document.querySelector('.product-type')
const priceTop = document.querySelector('.price-top')
const productName = document.querySelector('.product-name')
const productImg = document.querySelector('.product-img')
const colorArea = document.querySelector('.color-area')
const colorAreaControlBtn = document.querySelector('[aria-controls="panelsStayOpen-color"]')
const accordionBox = document.querySelector('.accordion')
//window onload
window.onload = function(){
    showNavbar()
    selectProduct(appleList[0])
    //畫面載入時，預設要載入第一個商品
}

//function
function showNavbar(){
    const productList = appleList.map(x => x.name)
    productList.forEach((item, index) => {
        const li = document.createElement('li')
        const a = document.createElement('a')
        a.innerText = item
        a.href = `#${item}`
        a.classList.add('btn', 'btn-dark', 'product')
        a.onclick = function(){
            selectProduct(appleList[index])
            //導覽列的產品名稱按下去之後，頁面要替換掉現在的商品(用appleList的索引找到要去的商品)
        }
        li.appendChild(a)
        navbar.appendChild(li)
    })
}

//selectProduct function
function selectProduct(product){
    restApple()
    productType.innerText = product.type
    priceTop.innerText = '$0'
    productName.innerText = `購買 ${product.type}`
    productImg.src = product.mainImg

    //用forEach做color btn
    product.colorList.forEach((item, index) => {
        const div = document.createElement('div')
        div.classList.add('col-6', 'mb-3')
        const btn = document.createElement('button')
        btn.classList.add('btn', 'color-btn', 'w-100')
        const btnDiv = document.createElement('div')
        btn.onclick = function() {
            colorArea.querySelectorAll('.btn').forEach(b => {
                b.setAttribute('selected', 'false')
            })
            btn.setAttribute('selected', 'true')
            productImg.src = item.img
            colorAreaControlBtn.innerText = item.name
            colorAreaControlBtn.click()
            //假裝做一個點擊，讓accordion收合
        }
        btnDiv.classList.add('py-4', 'd-flex', 'flex-column', 'justify-content-center', 'align-items-center')
        const i = document.createElement('i')
        i.classList.add('fas', 'fa-circle')
        i.style.color = item.color
        const span = document.createElement('span')
        span.innerText = item.name
        span.classList.add('color-name')
        btnDiv.appendChild(i)
        btnDiv.appendChild(span)
        btn.append(btnDiv)
        div.append(btn)
        colorArea.append(div)
    })
    //用forEach 做 spec Btn
    product.spec.forEach((item) => {
        const accordionItem = document.createElement('div')
        accordionItem.classList.add('accordion-item')
        const accordionTitle = document.createElement('h2')
        accordionTitle.classList.add('accordion-header')
        const accordionBtn = document.createElement('button')
        accordionBtn.innerText = item.name
        accordionBtn.classList.add('accordion-button')
        accordionBtn.setAttribute('type', 'button')
        accordionBtn.setAttribute('data-bs-toggle', 'collapse')
        accordionBtn.setAttribute('data-bs-target', `#panelsStayOpen-${item.name}`)
        accordionBtn.setAttribute('aria-expanded', 'true')
        accordionBtn.setAttribute('aria-controls', `panelsStayOpen-${item.name}`)
        accordionTitle.appendChild(accordionBtn)
        const accordionContent = document.createElement('div')
        accordionContent.setAttribute('id', `panelsStayOpen-${item.name}`)
        accordionContent.classList.add('accordion-collapse', 'collapse', 'show')
        const accordionBody = document.createElement('div')
        accordionBody.classList.add('accordion-body')
        const h5 = document.createElement('h5')
        const strong = document.createElement('strong')
        strong.innerText = item.name
        h5.appendChild(strong)
        const specDiv = document.createElement('div')
        specDiv.classList.add('row')
        item.specDetails.forEach((specItem) => {
            const div = document.createElement('div')
            div.classList.add('col-6', 'mb-3')
            const btn = document.createElement('button')
            btn.classList.add('btn', 'color-btn', 'w-100')
            btn.setAttribute('selected', 'false')
            btn.setAttribute('fit', specItem.fit)//加上價格
            btn.onclick = function(){
                specDiv.querySelectorAll('.btn').forEach(b => {
                    b.setAttribute('selected', 'false')
                })
                btn.setAttribute('selected', 'true')//被選起來了，在CSS寫藍色外框的樣式，但是這樣每一個都會有藍色外框，在上面的的foeEach讓所有的btn都是沒有被選擇的狀態，在被選後才出現藍色外框
                specDiv.setAttribute('use-fit', specItem.fit)
                accordionBtn.innerHTML = specItem.name//改成顯示規格名稱
                accordionBtn.click()//假裝點一下
                showPrice(product)
            }
            const btnDiv = document.createElement('div')
            btnDiv.classList.add('py-4', 'd-flex', 'flex-column', 'justify-content-center', 'align-items-center')
        
            const p = document.createElement('p')
            p.classList.add('spec-val', 'm-0')
            p.innerText = specItem.name
            const span = document.createElement('span')
            span.classList.add('fit')
            span.innerText = `NT$${parseInt(product.price) + parseInt(specItem.fit)}起`
            btnDiv.appendChild(p)
            btnDiv.appendChild(span)
            btn.appendChild(btnDiv)
            div.appendChild(btn)
            specDiv.appendChild(div)
        })
        accordionBody.appendChild(h5)
        accordionBody.appendChild(specDiv)
        accordionContent.appendChild(accordionBody)
        accordionItem.appendChild(accordionTitle)
        accordionItem.appendChild(accordionContent)
        accordionBox.appendChild(accordionItem)
    })
}
//價格計算
function showPrice(product){
    const selectedFits = Array.from(document.querySelectorAll('[fit][selected="true"]'))
    //找到所有被選取的(true)，要同時有[fit]並且[selected="true"]
    //要把nodeLisst變成一個Array，因為map是Array的方法，nodeList不可以用
    const money = selectedFits.length > 0 ? selectedFits.map(x => parseInt(x.getAttributeNode('fit').value)).reduce((a, b)=> a + b) : 0
    
    priceTop.innerText = `$${product.price + money}`
    // let step1 = document.querySelectorAll('[fit][selected="true"]')
    // console.log(step1)
    // let step2 = Array.from(step1)
    //要把nodeLisst變成一個Array，因為map是Array的方法，nodeList不可以用
    // console.log(step2)
    // let step3 = step2.map(x => parseInt(x.getAttributeNode('fit').value))
    // console.log(step3)
    // let step4 = step3.reduce((a, b) => a +b)//陣列裡的數字相加
    // console.log(step4)
}
function restApple() {
    colorArea.innerHTML = ''

    const removeItem = accordionBox.querySelectorAll('.accordion-item')
    if(removeItem.length > 1) {
        for(let i=1; i < removeItem.length; i++){
            accordionBox.removeChild(accordionBox.children[1]) 
            //只保留第一個顏色區塊，剩下的accordion-item都要清掉
        }
    }
}