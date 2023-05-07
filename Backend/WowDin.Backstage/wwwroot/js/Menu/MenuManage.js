import ClassList from './ClassList.js'
import AddClassModal from './AddClassModal.js'
import CopyMenuModal from './CopyMenuModal.js'

let isAccordionMoving = false

//const API_BASE_URL = 'https://wowdin-backstage.azurewebsites.net'
const app = new Vue({
    el: '#app',
    data: {
        shopList: [],
        classList: [],
        currentShop: 0,
        recordTime: '',
        isLoading: false,
        deleteClass: {
            class: null,
            classIdx: -1,
        },
        inputData: {
            classInput: {
                shopId: 0,
                menuClassId: -1,
                className: '',
            },
            productInput: {
                shopId: 0,
                id: '',
                name: '',
                menuClassId: 0,
                state: '',
                figure: '',
                basicPrice: 0,
                changeNote: '',
                customs: [],
                deletedCustoms: []
            },
        },
        addProduct: {
            productInput: "",
            groupId: 0
        },
        dragAndDrop: {
            classes: '',
            products: '',
            productContainers: '',
            source: null,
            overItem: null,
            currentContainer: null
        },
        // collapseOpen: false,
        showDetailState: false,
        copyBtnEnable: true,
        drag: false,
        dragOptions: {
            animation: 200,
            ghostClass: 'ghosting_class'
        },
        arrange: {
            arrangeState: false,
            arrangeLoading: false,
        },
        selectedClass: [],
        copyCustomSource: 0,
        overlayShow: false,
    },
    computed: {
        hasMenu(){  
            return this.classList.length == 0 ? false : true
        },
        showDetailBtnText() {
            return this.showDetailState ? '隱藏產品明細' : '顯示產品明細'
        },
        copyBtnText(){
            return this.copyBtnEnable ? '複製當前菜單' : '儲存中'
        },
        arrangeBtnText(){
            if (this.arrange.arrangeLoading){
                return '儲存中'
            }
            if (this.arrange.arrangeState){
                return '儲存排序'
            }
            else{
                return '進行排序'
            }
        },
        arrangeBtnVariant(){
            return this.arrange.arrangeState ? 'primary' : 'outline-primary'
        },
        classOptions(){
            return this.classList.map(x => ({ value: x.id, text: x.name }))
        }
    },
    created () {
        this.GetShops()
    },
    watch:{
        currentShop: {
            handler(){
                let btnsArea = document.querySelector('#btn_area')
                let menuArea = document.querySelector('#menu_area')

                if(this.currentShop == 0){
                    btnsArea.classList.add('d-none')
                    menuArea.classList.add('d-none')
                    return
                }
                else{
                    this.isLoading = true
                    btnsArea.classList.remove('d-none')

                    this.showDetailState = false
                    this.inputData.classInput.shopId = this.currentShop
                    this.inputData.productInput.shopId = this.currentShop
                    this.arrange.arrangeState = false
        
                    this.GetMenuData()
                }
            },
            immediate: true
        },
        collapseOpen: function(){
            collapses = document.querySelectorAll('.collapse')
            if (this.collapseOpen){
                collapses.forEach(x => 
                { 
                    if (!x.classList.includs('show'))
                    x.classList.add('show') 
                })
            }
            else{
                collapses.forEach(x => 
                { 
                    x.classList.remove('show') 
                })
            }
        },
        drag: {
            handler(){
                isAccordionMoving = this.drag
                if(!this.drag){
                    console.log(this.classList)
                }
            }
        },
        'arrange.arrangeState': {
            handler(){
                const btns = document.querySelectorAll('#app button:not(#arrange_btn):not(.class_accordion_btn):not(#swich_detail_btn)')
                const selector = document.querySelector('#shop_selector')
                if(this.arrange.arrangeState){
                    btns.forEach(x => {
                        x.setAttribute('disabled', 'true')
                    })
                    selector.setAttribute('disabled', 'true')
                }
                else{
                    btns.forEach(x => {
                        x.removeAttribute('disabled')
                    })
                    selector.removeAttribute('disabled')
                }
            },
            immediate: true
        }
    },
    methods: {
        GetShops(){
            axios.get(`${API_BASE_URL}/api/GetShops/${brandId}`)
            .then((res) => {
                console.log(res.data.msg)
                if(res.data.status == 20000){
                    this.shopList = [{id: 0, name: '-- 請選擇分店 --'}, ... res.data.result]
                    this.currentShop = this.shopList[0].id
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        GetMenuData(){
            this.overlayShow = true
            let menuArea = document.querySelector('#menu_area')
            
            axios.get(`${API_BASE_URL}/api/menudata/${this.currentShop}`)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.classList = res.data.result.menuClasses
                        this.recordTime = res.data.result.updateTime
                        console.log(res.data.msg)
                    }
                    else{
                        console.log(res.data.msg)
                    }
                })
                .then((res) => {
                    this.isLoading = false
                    this.overlayShow = false
                    menuArea.classList.remove('d-none')
                    this.SetAutoExpand()
                })
                .catch(error => {
                    console.log(error)
                })
        },
        ShowProductDetail() {
            this.showDetailState = !this.showDetailState
        },
        SetAutoExpand(){
            let accordions = document.querySelectorAll('.class_accordion')
            let topArea = document.querySelector('#scroll_top')
            let bottomArea = document.querySelector('#scroll_bottom')

            accordions.forEach(x => {
                let btn = x.querySelector('.class_accordion_btn')
                let container = x.querySelector('.product_container')

                btn.addEventListener('dragover', function(evt){
                    if(!isAccordionMoving){
                        btn.classList.add('hovered')
                    }
                })
                btn.addEventListener('dragenter', function(evt){
                    if(!isAccordionMoving){
                        if(!container.classList.contains('show') && evt.target.tagName == 'BUTTON')
                        {
                            setTimeout(function(){
                                if(btn.classList.contains('hovered')){
                                    btn.click()
                                    btn.classList.remove('hovered')
                                }
                            }, 1000)
                        }
                    }
                })
                btn.addEventListener('dragleave', function(evt){
                    if(!isAccordionMoving){
                        btn.classList.remove('hovered')
                    }
                })
            })

            topArea.addEventListener('dragover', function(){
                window.scrollBy(0, -5)
            })

            bottomArea.addEventListener('dragover', function(){
                window.scrollBy(0, 5)
            })
        },
        ArrangeClass() {
            //點擊進行排序
            if(!this.arrange.arrangeState){
                this.$bvModal.msgBoxConfirm('排序完成後請點擊儲存排序。', {
                    title: '提醒',
                    size: 'sm',
                    okVariant: 'danger',
                    okTitle: '確認',
                    cancelTitle: '取消',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                })
                .then((res) => {
                    if(res){
                        this.arrange.arrangeState = true
                    }
                    else{
                        toastr.info('取消變更')
                    }
                })
                .catch(err => {
                    console.log(err)
                })
            }
            //點擊儲存排序
            else{
                this.arrange.arrangeState = false
                this.arrange.arrangeLoading = true
                
                let request = {
                    shopId: this.currentShop,
                    menuClasses: this.classList
                }

                axios.post(`${API_BASE_URL}/api/Arrange/`, request)
                    .then((res) => {
                        console.log(res.data.msg)
                        if(res.data.status == 20000){
                            toastr.success(res.data.msg)
                        }
                        else{
                            toastr.warning(res.data.msg)
                        }
                    })
                    .then(res => {
                        this.arrange.arrangeLoading = false
                    })
                    .catch((err) => {
                        console.log(err)
                    })
            }
        },
        CopyMenu(selectedShops){
            let shops = this.shopList.filter(x => selectedShops.includes(x.id)).map(x => x.name)
            this.$bvModal.msgBoxConfirm(`確定複製菜單至${shops.join('、')}? 其現有的菜單資料將會被刪除`, {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
            .then(res => {
                if(res == true){
                    const selector = document.querySelector('#shop_selector')
                    this.copyBtnEnable = false
                    selector.setAttribute('disabled', 'true')
                    
                    let request = 
                    { 
                        source: this.classList, 
                        targets: selectedShops 
                    }
                    axios.post(`${API_BASE_URL}/api/Copy/`, request)
                    .then((res) => {
                        console.log(res.data.msg)
                        if(res.data.status == 20000){
                            toastr.success(res.data.msg)
                        }
                        else{
                            toastr.warning(res.data.msg)
                        }
                    })
                    .then((res) => {
                        selector.removeAttribute('disabled')
                        this.copyBtnEnable = true
                    })
                    .catch((err) => {
                        console.log(err)
                    })
                }
                else{
                    toastr.info('取消變更')
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        CopyCustom(productId){
            this.$bvModal.show('copyCustomTo')
            this.copyCustomSource = productId
        },
        CopyCustomConfirm(){
            this.overlayShow = true
            let request = {
                classIds: this.selectedClass,
                sourceProductId: this.copyCustomSource
            }
            console.log(request)
            
            axios.post(`${API_BASE_URL}/api/CopyCustom/`, request)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then((res) => {
                this.GetMenuData()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        resetCopyCustomModal(){
            this.selectedClass = []
        },
        CreateClass(className) {
            this.inputData.classInput.ClassName = className.trim()
            this.inputData.classInput.ShopId = this.currentShop
            
            axios.post(`${API_BASE_URL}/api/Class/`, this.inputData.classInput)
            .then((res) => {
                console.log(res.data.msg)
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
                this.ResetClassInput()
            })
            .then((res) => {
                this.GetMenuData()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        DeleteClass(classGroup) {
            let productAmount = classGroup.products.length
            this.$bvModal.msgBoxConfirm(`確定刪除類別? 共${productAmount}筆產品資料將一併刪除`, {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
            .then((res) => {
                if(res == true){
                    let idx = this.classList.findIndex(x => x.id == classGroup.id)
                    this.classList.splice(idx, 1)

                    this.inputData.classInput.MenuClassId = classGroup.id
                    this.inputData.classInput.ShopId = this.currentShop
                    
                    axios.delete(`${API_BASE_URL}/api/Class/`, { data: this.inputData.classInput })
                    .then((res) => {
                        if(res.data.status == 20000){
                            toastr.success(res.data.msg)
                        }
                        else{
                            toastr.warning(res.data.msg)
                        }
                    })
                    .catch((err) => {
                        console.log(err)
                    })
                }
                else{
                    toastr.info('取消變更')
                }
            })
            .catch((err) => {
                console.log(err)
            })
            
        },
        EditClass(classId, className){
            this.inputData.classInput.MenuClassId = classId
            this.inputData.classInput.ClassName = className.trim()
            this.inputData.classInput.ShopId = this.currentShop
            axios.put(`${API_BASE_URL}/api/Class/`, this.inputData.classInput )
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        ResetClassInput(){
            this.inputData.classInput.ClassName = ''
            this.inputData.classInput.ShopId = 0
        },
        DeleteProduct(group, product){
            this.$bvModal.msgBoxConfirm('確定刪除產品?', {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
            .then((res) => {
                if(res == true){
                    let idx = group.products.findIndex(x => x.id == product.id)
                    group.products.splice(idx, 1)

                    axios.put(`${API_BASE_URL}/api/Product/`, {shopId: this.currentShop, id: product.id })
                    .then((res) => {
                        if(res.data.status == 20000){
                            toastr.success(res.data.msg)
                        }
                        else{
                            toastr.warning(res.data.msg)
                        }
                        //this.ResetProductInput()
                    })
                    .catch((err) => {
                        console.log(err)
                    })
                }
                else{
                    toastr.info('取消變更')
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        UpdateProduct(groupId, product){
            this.inputData.productInput.id = product.id
            this.inputData.productInput.name = product.name.trim()
            this.inputData.productInput.menuClassId = groupId
            this.inputData.productInput.state = product.state
            this.inputData.productInput.figure = product.figure
            this.inputData.productInput.basicPrice = product.basicPrice
            this.inputData.productInput.changeNote = product.changeNote
            this.inputData.productInput.customs = product.customs
            this.inputData.productInput.deletedCustoms = product.deletedCustoms

            console.log(this.inputData.productInput)
            axios.post(`${API_BASE_URL}/api/Product/`, this.inputData.productInput)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
                this.ResetProductInput()
            })
            .then(() => {
                this.GetMenuData()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        ResetProductInput(){
            this.inputData.productInput.id = ''
            this.inputData.productInput.name = ''
            this.inputData.productInput.menuClassId = 0
            this.inputData.productInput.state = ''
            this.inputData.productInput.figure = ''
            this.inputData.productInput.basicPrice = ''
            this.inputData.productInput.changeNote = ''
        },
    },
    components: {
        'add-class-modal': AddClassModal,
        'copy-menu-modal': CopyMenuModal,
        'class-list': ClassList
    }
})