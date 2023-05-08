const app = new Vue({
    el: '#app',
    data: {
        tabIndex: 0,

        adItems: [],
        
        fields: [
            {
                key: 'number', 
                label: '#',
            },
            { 
                key: 'img', 
                label: '廣告圖片',
            },
            { 
                key: 'isSearchZone', 
                label: '廣告位置',
                class: 'text-center',
                formatter: (value) => {
                    if (value) {
                        return '大圖幻燈片'
                    }
                    return '小圖廣告區'
                }
            },
            { 
                key: 'couponCode', 
                label: '優惠代碼',
                class: 'text-center',
                formatter: (value) => {
                    if (value == null) {
                        return '無'
                    }
                    return value
                }
            },
        ],

        removedFields: [
            {
                key: 'number', 
                label: '#',
            },
            { 
                key: 'img', 
                label: '廣告圖片',
            },
            { 
                key: 'isSearchZone', 
                label: '廣告位置',
                class: 'text-center',
                formatter: (value) => {
                    if (value) {
                        return '大圖幻燈片'
                    }
                    return '小圖廣告區'
                }
            },
            { 
                key: 'couponCode', 
                label: '優惠代碼',
                class: 'text-center',
                formatter: (value) => {
                    if (value == null) {
                        return '無'
                    }
                    return value
                }
            },
            { 
                key: 'action', 
                label: '動作',
                class: 'text-center',
            },
            { 
                key: 'message', 
                label: '管理員訊息',
                class: 'text-center',
            },
        ],

        rejectedFields: [
            {
                key: 'number', 
                label: '#',
            },
            { 
                key: 'img', 
                label: '廣告圖片',
            },
            { 
                key: 'isSearchZone', 
                label: '廣告位置',
                class: 'text-center',
                formatter: (value) => {
                    if (value) {
                        return '大圖幻燈片'
                    }
                    return '小圖廣告區'
                }
            },
            { 
                key: 'couponCode', 
                label: '優惠代碼',
                class: 'text-center',
                formatter: (value) => {
                    if (value == null) {
                        return '無'
                    }
                    return value
                }
            },
            { 
                key: 'message', 
                label: '管理員訊息',
                class: 'text-center',
            },
        ],

        couponsItems: [],
        selectedCouponId: 0,
        couponFields: [
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
                sortable: true,
                formatter: (value) => {
                    switch (value) {
                        case '1': return "單品折價券";
                        case '2': return "全店優惠券";
                        default: return "未分類";
                    }
                },
            },
            {
                key: 'code',
                label: '兌換代碼',
                class: 'text-center',
                sortable: true,
            },
            {
                key: 'releasedAmount',
                label: '已發送數量',
                sortable: true,
                class: 'text-center',
                formatter: (value, key, item) => {
                    return `${value} / ${item.maxAmount}張`
                },
            },
        ],
        
        publishedCurrentPage: 1,
        publishedTotalRows: 0,

        pendingCurrentPage: 1,
        pendingTotalRows: 0,

        removedCurrentPage: 1,
        removedTotalRows: 0,

        rejectedCurrentPage: 1,
        rejectedTotalRows: 0,
        
        perPage: 10,
        pageOptions: [10, 20, 50],

        filter: '',
        filterOn: [],

        sortBy: '',

        isBusy: false,

        adTypeOptions: [
            { text: '大圖幻燈片', value: true },
            { text: '小圖廣告區', value: false }
        ],
        formCheck: {
            adFig: {
                check: false,
                errorMsg: '請上傳廣告圖片'
            },
        },
        newAd: {
            brandId: brandId,
            adFig: '',
            isSearchZone: false,
            couponId: 0
        },
        
        figure: {
            uploadFigure: '',
            hasFigure: false,
            figureLoading: false,
            uploadResult: '',
            valid: null,
        },

        img: null,
        imgSrc: ''
    },
    computed: {
        selectedCoupon(){
            if(this.selectedCouponId != 0){
                let coupon = this.couponsItems.find(x => x.id == this.selectedCouponId)
                return `${coupon.couponName} (代碼: ${coupon.code})` 
            }
        },
        publishedItems(){
            return this.adItems.filter(x => x.status == "刊登中")
        },
        pendingItems(){
            return this.adItems.filter(x => x.status == "申請中")
        },
        removedItems(){
            return this.adItems.filter(x => x.status == "已下架")
        },
        rejectedItems(){
            return this.adItems.filter(x => x.status == "拒絕申請")
        },
        recommandWidth(){
            if(this.newAd.isSearchZone){
                return 1000
            }
            else{
                return 720
            }
        },
        recommandHeight(){
            if(this.newAd.isSearchZone){
                return 460
            }
            else{
                return 240
            }
        }
    },
    watch: {
        currentShop: {
            handler(){
                let shop = this.shopAndProduct.find(x => x.shopId == this.currentShop)
                this.productListOfShop = shop.products.map(x => ({ value: x.productId, text: x.productName}))
            },
            immediate: false
        },
        'figure.uploadFigure': {
            handler(){
                this.UploadFigure(this.figure.uploadFigure)
            }
        },
        'newAd.adFig': {
            handler(){
                if(this.newAd.adFig != ''){
                    this.formCheck.adFig.check = true
                }
                else{
                    this.formCheck.adFig.check = false
                }

                this.isFigureValid()
            }
        },
        'newAd.isSearchZone': {
            handler(){
                this.isFigureValid()
            },
        }
    },
    created() {
        this.getAllAd()
        this.getAllCoupon()
    },
    methods: {
        endtimePassClass(item){
            if(item != null && new Date(item.endTime) - new Date() < -86400000){
                return 'table-warning'
            }
        },
        getAllAd(){
            this.isBusy = true
            axios.get(`${API_BASE_URL}/api/Advertise/${brandId}`)
            .then((res) => {
                if(res.data.status == 20000){

                    this.adItems = res.data.result.allAdvertise
                    
                    this.publishedTotalRows = this.publishedItems.length
                    this.pendingTotalRows = this.pendingItems.length
                    this.removedTotalRows = this.removedItems.length
                    this.rejectedTotalRows = this.rejectedItems.length
                    
                    this.isBusy = false
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        getAllCoupon(){
            axios.get(`${API_BASE_URL}/api/GetAllCoupon/${brandId}`)
            .then((res) => {
                if(res.data.status == 20000){
                    let distinctItems = []
                    let codes = []
                    res.data.result.coupons.filter(x => x.status == "開放兌換").forEach((x) => {
                        if(!codes.includes(x.code)){
                            distinctItems.push(x)
                            codes.push(x.code)
                        }
                    })
                    this.couponsItems = distinctItems
                }
            })
            .catch((err) => {
                console.log(err)
            })
        },
        resetModal(){
            this.newAd.adFig = ''
            this.newAd.isSearchZone = false
            this.selectedCouponId = 0
        },
        submitAd(bvModalEvent){
            bvModalEvent.preventDefault()
            this.$bvModal.msgBoxConfirm('請確認廣告圖無誤。', {
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
            return this.formCheck.adFig.check
        },
        isFigureValid() {
            this.getImgSize()
                .then((res) => {
                    this.figure.valid =
                        res.width == this.recommandWidth && res.height == this.recommandHeight
            })
        },
        getImgSize() {
            return new Promise((resolve, reject) => {
                let img = new Image()
                img.src = this.newAd.adFig
                
                img.onload = () => {
                    resolve({
                        width: img.width,
                        height: img.height
                    })
                }
            })
        },
        setCoupon(){
            this.newAd.couponId = this.selectedCouponId
        },
        handleSubmit(){
            if(!this.checkForm()){
                toastr.warning('請上傳廣告圖')
                return
            }
            
            axios.post(`${API_BASE_URL}/api/Advertise`, this.newAd)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then(res => {
                this.getAllAd()
            })
            .catch((err) => {
                console.log(err)
            })

            this.$nextTick(() => {
                this.$bvModal.hide('modal-create')
            })
        },
        reSubmit(ad){
            debugger
            axios.get(`${API_BASE_URL}/api/ReSubmit/${ad.advertiseId}`)
            .then((res) => {
                if(res.data.status == 20000){
                    toastr.success(res.data.msg)
                }
                else{
                    toastr.warning(res.data.msg)
                }
            })
            .then(res => {
                this.getAllAd()
            })
            .catch((err) => {
                console.log(err)
            })
        },
        UploadFigure(file){
            this.figure.figureLoading = true
            const formdata = new FormData()
            formdata.append('file', file)

            axios.post(`${API_BASE_URL}/Information/UploadImage/`, formdata, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then((res) => {
                let result = JSON.parse(res.data)

                this.newAd.adFig = result.FileUrl
                this.figure.uploadResult = result.Message
                this.figure.figureLoading = false
                this.figure.hasFigure = true

                this.getAllAd()
            })
            .catch((err) => {
                console.log(err)
            })
        },
    },
})
