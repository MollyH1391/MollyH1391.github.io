const app = new Vue({
    el: '#app',
    data: {
        openItems: [],
        closeItems: [],
        fields: [
            {
                key: 'number', 
                label: '#',
            },
            { 
                key: 'couponName', 
                label: '優惠名稱',
                sortable: true,
                formatter: (value) => {
                    if(value.length > 5){
                        return value.substring(0, 5) + '...'
                    }
                    else{
                        return value
                    }
                }, 
            },
            {
                key: 'shopName',
                label: '適用店面',
                sortable: true,
                class: 'text-center',
            },
            {
                key: 'startTime',
                label: '開始時間',
                sortable: true,
                class: 'text-center',
            },
            {
                key: 'endTime',
                label: '結束時間',
                sortable: true,
                class: 'text-center',
            },
            {
                key: 'type',
                label: '類型',
                class: 'text-center',
                formatter: (value) => {
                    switch (value) {
                        case '1': return "單品折價券";
                        case '2': return "全店優惠券";
                        default: return "未分類";
                    }
                },
            },
            {
                key: 'thresholdAmount',
                label: '優惠門檻',
                class: 'text-center',
                formatter: (value, key, item) => {
                    if(item['type'] == '1'){
                        return '指定商品'
                    }
                    else{
                        return `${value}元`
                    }
                },
            },
            {
                key: 'discountAmount',
                label: '優惠金額',
                sortable: true,
                class: 'text-center',
                formatter: (value, key, item) => {
                    if(item['type'] == '1'){
                        return `${value}元`
                    }
                    else{
                        return `${value * 10}折`
                    }
                },
            },
            {
                key: 'maxAmount',
                label: '數量限制',
                sortable: true,
                class: 'text-center',
                formatter: (value) => {
                    if(value == 0){
                        return '無上限'
                    }
                    else{
                        return `${value}張`
                    }
                },
            },
            {
                key: 'code',
                label: '兌換代碼',
                class: 'text-center',
            },
            {
                key: 'releasedAmount',
                label: '已發送數量',
                sortable: true,
                class: 'text-center',
                formatter: (value) => {
                    return `${value}張`
                },
            },
            { 
                key: 'actions', 
                label: '管理',
                class: 'text-center',
            }
        ],
        tabIndex: 0,

        coupons: [],
        shopAndProduct: [],
        shopList: [],
        
        openCurrentPage: 1,
        openPerPage: 10,
        openTotalRows: 0,
        closeCurrentPage: 1,
        closePerPage: 10,
        closeTotalRows: 0,
        pageOptions: [10, 20, 50],

        filter: '',
        filterOn: [],

        sortBy: '',

        isBusy: false,

        codeTypeOptions: [
            { text: '單品折價券', value: 1 },
            { text: '全店優惠券', value: 2 }
        ],
        formCheck: {
            couponName: {
                check: false,
                errorMsg: '請輸入優惠名稱'
            },
            maxAmount: {
                check: true,
                errorMsg: '數量不得為負數'
            },
            startTime: {
                check: false,
                errorMsg: '請選擇開始日期'
            },
            endTime: {
                check: false,
                errorMsg: '請選擇結束日期'
            },
            type: {
                check: false,
                errorMsg: '請選擇一個種類'
            },
            thresholdAmount:{
                check: false,
                errorMsg: '請輸入優惠門檻'
            },
            discountAmount:{
                check: false,
                errorMsg: '請輸入優惠數量'
            },
            shops: {
                check: false,
                errorMsg: '請選擇適用店面'
            }
        },
        newCoupon: {
            couponName: '',
            startTime: null,
            endTime: null,
            type: 0,
            thresholdAmount: '1',
            discountAmount: '1',
            maxAmount: null,
            description: '',
            status: false,
            shops: [],
        },
        selectedShopFields: [
            {
                key: 'shopName',
                label: '店名'
            },
            {
                key: 'products',
                label: '產品',
                formatter: (value) => {
                    if(app.newCoupon.type == 2){
                        return '-'
                    }
                    else{
                        return value.map(x => x.productName).join('、')
                    }
                },
            },
            {
                key: 'addProduct',
                label: ''
            }
        ],
        selectedShops: [],
        currentShop: 0,
        productListOfShop: [],
        selectedProducts: [],

        productFilter: '',
        productFiltered: [],
        
        allSelected: false,
        indeterminate: false,

        passedCoupons: []
    },
    watch: {
        openItems: {
            handler(){
                this.checkPass()
            }
        },
        currentShop: {
            handler(){
                let shop = this.shopAndProduct.find(x => x.shopId == this.currentShop)
                this.productListOfShop = shop.products.map(x => x.productName)
                this.productFiltered = [ ... this.productListOfShop]
            },
            immediate: false
        },
        'newCoupon.couponName' :{
            handler(){
                if(this.newCoupon.couponName == ''){
                    this.formCheck.couponName.check = false
                }
                else{
                    this.formCheck.couponName.check = true
                }
            }
        },
        'newCoupon.startTime' :{
            handler(){
                this.checkTime()
            }
        },
        'newCoupon.endTime' :{
            handler(){
                this.checkTime()
            }
        },
        'newCoupon.type' :{
            handler(){
                if(this.newCoupon.type == 0){
                    this.formCheck.type.check = false
                }
                else{
                    this.checkShopsOfNewCoupon()
                    if(this.newCoupon.type == 1){
                        this.newCoupon.thresholdAmount = '2'
                        this.newCoupon.discountAmount = '1'
                    }
                    else if(this.newCoupon.type == 2){
                        this.newCoupon.thresholdAmount = '1'
                        this.newCoupon.discountAmount = '0.99'
                    }
                    this.formCheck.type.check = true
                }
            }
        },
        'newCoupon.thresholdAmount' :{
            handler(){
                if(this.newCoupon.thresholdAmount == ''){
                    this.formCheck.thresholdAmount.check = false
                }
                else{
                    this.formCheck.thresholdAmount.check = true
                }
            },
            immediate: true
        },
        'newCoupon.discountAmount' :{
            handler(){
                if(this.newCoupon.discountAmount == '' || this.newCoupon.discountAmount == 0){
                    this.formCheck.discountAmount.check = false
                }
                else{
                    if(this.newCoupon.type == 2 && parseInt(this.newCoupon.discountAmount) == 1){
                        this.formCheck.discountAmount.check = false
                    }
                    else{
                        this.formCheck.discountAmount.check = true
                    }
                }
            },
            immediate: true
        },
        'newCoupon.shops' :{
            handler(){
                this.checkShopsOfNewCoupon()
            },
            deep: true
        },
        selectedShops(newValue, oldValue) {
            // Handle changes in individual flavour checkboxes
            if (newValue.length === 0) {
                this.indeterminate = false
                this.allSelected = false
            } else if (newValue.length === this.shopList.length) {
                this.indeterminate = false
                this.allSelected = true
            } else {
                this.indeterminate = true
                this.allSelected = false
            }
        },
        productFilter:{
            handler(){
                this.productFiltered = this.productListOfShop.filter(x => x.includes(this.productFilter))
            },
        },
        selectedProducts(newValue, oldValue) {
            // Handle changes in individual flavour checkboxes
            if (newValue.length === 0) {
                this.indeterminate = false
                this.allSelected = false
            } else if (newValue.length === this.productFiltered.length) {
                this.indeterminate = false
                this.allSelected = true
            } else {
                this.indeterminate = true
                this.allSelected = false
            }
        },
    },
    created() {
        this.getAllCoupon()
        this.getShopsAndProducts()
    },
    mounted() {
        //this.$bvModal.show('modal-create')
    },
    methods: {
        startDateDisabled(ymd, date){
            let today = new Date()
            return today - date > 86400000
        },
        endDateDisabled(ymd, date){
            let today = new Date()
            let startDay = new Date(this.newCoupon.startTime)
            return startDay > date || today - date > 86400000
        },
        endtimePassClass(item){
            if(item != null && this.isTimePass(item.endTime)){
                return 'table-warning'
            }
        },
        checkTime(){
            let today = new Date()
            if(this.newCoupon.startTime == null){
                this.formCheck.startTime.check = false
                this.formCheck.startTime.errorMsg = '請選擇開始日期'
            }

            if(this.newCoupon.endTime == null){
                this.formCheck.endTime.check = false
                this.formCheck.endTime.errorMsg = '請選擇結束日期'
            }

            if(this.newCoupon.startTime != null && this.newCoupon.endTime != null)
            {
                if(new Date(this.newCoupon.endTime) - new Date(this.newCoupon.startTime) <= 0){
                    this.formCheck.startTime.check = false
                    this.formCheck.startTime.errorMsg = '請早於結束日期'
                    this.formCheck.endTime.check = false
                    this.formCheck.endTime.errorMsg = '請晚於開始日期'
                }
                else{
                    this.formCheck.startTime.check = true
                    this.formCheck.endTime.check = true
                }
            }
        },
        isTimePass(endtime){
            if(new Date(endtime) - new Date() < -86400000){
                return true
            }
            else{
                return
            } 
        },
        checkPass(){
            this.passedCoupons.length = 0
            this.openItems.forEach(x => {
                if(this.isTimePass(x.endTime)){
                    this.passedCoupons.push(x.id)
                }
            })
        },
        checkShopsOfNewCoupon(){
            if(this.newCoupon.shops.length == 0){
                this.formCheck.shops.check = false
                if(this.newCoupon.type == 1){
                    this.formCheck.shops.errorMsg = '請選擇適用店面及產品'
                }
                else{
                    this.formCheck.shops.errorMsg = '請選擇適用店面'
                }
            }
            else{
                if(this.newCoupon.type == 1 && this.isProductAmountOfShopEmpty()){
                    this.formCheck.shops.check = false
                    this.formCheck.shops.errorMsg = '尚有店面未選擇適用產品'
                }
                else{
                    this.formCheck.shops.check = true
                }
            }
        },
        isProductAmountOfShopEmpty(){
            let isEmpty = false
            this.newCoupon.shops.forEach(x => {
                if(x.products.length == 0){
                    isEmpty = true
                }
            })
            return isEmpty
        },
        getAllCoupon(){
            this.isBusy = true
            axios.get(`${API_BASE_URL}/api/GetAllCoupon/${brandId}`)
            .then((res) => {
                if(res.data.status == 20000){
                    this.coupons = res.data.result.coupons
                    console.log(this.coupons)

                    this.openItems = this.coupons.filter(x => x.status == "開放兌換")
                    this.closeItems = this.coupons.filter(x => x.status == "停止兌換")
                    this.openTotalRows = this.openItems.length
                    this.closeTotalRows = this.closeItems.length
                    this.isBusy = false
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        getShopsAndProducts(){
            axios.get(`${API_BASE_URL}/api/GetShopsAndProducts/${brandId}`)
            .then((res) => {
                this.shopAndProduct = res.data.result.shops
                console.log(this.shopAndProduct)
                this.shopList = this.shopAndProduct.map(x => x.shopName)
            })
            .catch((err) => {
                console.log(err)
            })
        },
        resetModal(){
            this.newCoupon.couponName = ''
            this.newCoupon.startTime = null
            this.newCoupon.endTime = null
            this.newCoupon.type = 0
            this.newCoupon.thresholdAmount = '1'
            this.newCoupon.dicountAmount = '1'
            this.newCoupon.maxAmount = null
            this.newCoupon.Description = ''
            this.newCoupon.status = false
            this.newCoupon.shops = []
        },
        createCoupon(bvModalEvent){
            bvModalEvent.preventDefault()
            this.$bvModal.msgBoxConfirm('將產生一組優惠代碼，並將此代碼關聯至所選的店面及產品。優惠新增後便不能修改內容，僅可切換開放或關閉。', {
                title: '提醒',
                size: 'sm',
                okVariant: 'success',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            }).then(res => {
                if(res) {this.handleSubmit()}
            })
        },
        checkForm(){
            let isValid = true
            for(var prop in this.formCheck){
                if(this.formCheck[prop].check == false){
                    isValid = false
                }
            }
            return isValid
        },
        handleSubmit(){
            if(!this.checkForm()){
                toastr.warning('尚有欄位未填妥!')
                return
            }
            let start = new Date(this.newCoupon.startTime)
            let end = new Date(this.newCoupon.endTime)
            
            let request = {
                couponName: this.newCoupon.couponName,
                startTime: new Date(Date.UTC(start.getUTCFullYear(), start.getUTCMonth(), start.getUTCDate())),
                endTime: new Date(Date.UTC(end.getUTCFullYear(), end.getUTCMonth(), end.getUTCDate())),
                type: this.newCoupon.type,
                thresholdAmount: parseFloat(this.newCoupon.thresholdAmount),
                discountAmount: parseFloat(this.newCoupon.discountAmount),
                maxAmount: this.newCoupon.maxAmount,
                description: this.newCoupon.description,
                status: this.newCoupon.status == true? 1 : 2,
                couponBelong: this.newCoupon.shops.map(x => (
                    {
                        shopId: x.shopId,
                        productIds: x.products.map(p => p.productId)
                    }))
            }

            axios.post(`${API_BASE_URL}/api/Coupon`, request)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then(res => {
                this.getAllCoupon()
            })
            .catch((err) => {
                console.log(err)
            })

            this.$nextTick(() => {
                this.$bvModal.hide('modal-create')
            })
        },
        switchStatus(coupon){
            axios.get(`${API_BASE_URL}/api/Coupon/${coupon.id}`)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then(res => {
                this.getAllCoupon()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        switchAll(){
            if(this.passedCoupons.length == 0){
                toastr.info('目前沒有開放的過期優惠')
                return
            }

            axios.post(`${API_BASE_URL}/api/SwitchAll`, this.passedCoupons)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then(res => {
                this.getAllCoupon()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        setShops(){
            if(this.newCoupon.shops.length == 0){
                this.selectedShops.forEach(x => {
                    let shop = this.shopAndProduct.find(y => y.shopName == x)
                    this.newCoupon.shops.push({
                        shopId: shop.shopId,
                        shopName: shop.shopName,
                        products: []
                    })
                })
            }
            else{
                this.selectedShops.forEach(x => {
                    if(!this.newCoupon.shops.map(s => s.shopName).includes(x)){
                        let shop = this.shopAndProduct.find(y => y.shopName == x)
                        this.newCoupon.shops.push({
                            shopId: shop.shopId,
                            shopName: shop.shopName,
                            products: []
                        })
                    }
                })
            }
        },
        removeShop(index){
            this.selectedShops.splice(index, 1)
            this.newCoupon.shops.splice(index, 1)
        },
        setProductModal(shop){
            this.currentShop = shop.shopId
        },
        setProducts(){
            let shopContainer = this.newCoupon.shops.find(y => y.shopId == this.currentShop).products
            shopContainer = []

            let productSource = this.shopAndProduct.find(s => s.shopId == this.currentShop).products
            this.selectedProducts.forEach(x => {
                let product = productSource.find(p => p.productName == x)
                shopContainer.push(product)
            })
            this.newCoupon.shops.find(y => y.shopId == this.currentShop).products = shopContainer
        },
        toggleAll(checked) {
            this.selectedShops = checked ? [ ... this.shopList] : []
        },
        toggleAllProduct(checked) {
            this.selectedProducts = checked ? [ ... this.productFiltered] : []
        },
        resetShop(){
            this.selectedShops = []
        },
        resetProduct(){
            this.selectedProducts = []
            this.productFilter = ''
        },
    },
})