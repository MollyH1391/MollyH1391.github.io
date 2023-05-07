//const API_BASE_URL = 'https://localhost:5001'
//const API_BASE_URL = 'https://wowdin-backstage.azurewebsites.net'

const showPageNum = document.querySelector('#showPageNum')

const brandorderdetail = new Vue({
    el: '#brandorderdetailVue',
    data: {
        filter: "",
        filters: [],
        shopId: '' ,
        allOrderdetailList: [], //放全部訂單資料
        //shopList: [],
        //distinctShopList: [],
        shopListByBrand: [],
        selected: '',
        options: [
            {name: '請選擇分店'}
        ],
        orderdetailsByShop: [], //放分店訂單資料
        currentPage: 1,
        perPage: 10,
        filterCounts: ''
    },
    created() {
        this.GetAllOrderDetailsByBrand()
        this.GetAllShopsByBrand()
    },
    methods: {
        showModal(orderId) {
            this.$bvModal.show(`modalxl_${orderId}`)
        },
        GetAllOrderDetailsByBrand() {
            axios.get(`/api/OrderDetails_Brand`)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.allOrderdetailList = res.data.result
                    }
                    else {
                        console.warn('撈資料失敗')
                    }
                })
                .catch((error) => { console.log(error) })
        },
        GetAllShopsByBrand() {
            axios.get(`/api/ShopListByBrand`)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.shopListByBrand = res.data.result
                        this.options = this.shopListByBrand
                    }
                    else {
                        console.warn('撈資料失敗')
                    }
                })
        },
        highlightMatches(text) {
            const matchExists = text
                .toString()
                .includes(this.filter.toString())
            if (!matchExists) return text;

            const re = new RegExp(this.filter, "ig")
            return text.replace(re, matchedText => `<strong class="text_blue">${matchedText}</strong>`)
        },
        filteredRows() {
            return this.orderdetailsByShop.filter(row => {
                const usernames = row.userName.toString()
                const orderStamp = row.orderStamp.toLowerCase()

                const searchTerm = this.filter.toString()

                return (
                    usernames.includes(searchTerm) || orderStamp.includes(searchTerm)
                )
                
            })
        }
    },
    watch: {
        selected() {
            this.orderdetailsByShop = this.allOrderdetailList.filter(s => s.shopId == `${this.selected}`)
            this.shopId = this.orderdetailsByShop.shopId
            this.filters = this.orderdetailsByShop
            this.filterCounts = this.filters.length
            console.log(this.selected)
            //debugger
        },
        filter: {
            handler() {
                
                //有輸入關鍵字的時候，要把dataShow換成filters[]資料
                if (this.filter == "") {
                    this.filters = this.orderdetailsByShop
                    this.filterCounts = this.filters.length
                    return
                }
                const searchTerm = this.filter.toString()
                this.filters = this.orderdetailsByShop.filter(row => {
                    const usernames = row.userName.toString()
                    const orderStamp = row.orderStamp.toLowerCase()
                    const orderState = row.orderState.toString()
                    const pickUpTime = row.pickUpTime.toString()
                    const takeMethod = row.takeMethod.toString()
                    const receiptType = row.receiptType.toString()
                    const orderDate = row.orderDate.toString()
                    const coupon = row.coupon.toString()
                    const searchTerm = this.filter.toString()

                    return (
                        usernames.includes(searchTerm) || orderStamp.includes(searchTerm) || orderState.includes(searchTerm) || orderDate.includes(searchTerm)
                        || pickUpTime.includes(searchTerm) || takeMethod.includes(searchTerm) || receiptType.includes(searchTerm) || coupon.includes(searchTerm) 
                    )
                    
                })
            },
            immediate: true
            
        }
    },
    computed: {
        total() {
            return this.filters ? this.filters.length : 0
        },
        showDataList() {
            let tempArr = JSON.parse(JSON.stringify(this.filters))
            return tempArr.splice((this.currentPage - 1) * this.perPage, this.perPage)
        },
        
    }
})


function gotoShopDetails() {
    let id = brandorderdetail.selected

    console.log(id)
    window.location.href = `/Order/OrderDetailsByShop/${id}`
}

