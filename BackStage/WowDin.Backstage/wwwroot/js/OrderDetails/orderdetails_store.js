
const orderdetail = new Vue({
    el: '#orderdetailVue',
    data: {
        filter: '',
        filters: [],
        filter_cancel: '',
        filters_cancel: [],
        orderdetailListO: [], //放全部訂單資料
        orderdetailListO_Establish: [], //放"未接單"
        orderdetailListO_Accept: [], //放"已接單"
        orderdetailListO_Cancel: [], //放"已取消"
        orderdetailListO_Complete: [], //放"已完成"
        acceptAllList: [],
        currentPage: 1,
        perPage: 10,
        shopList: [],
        shop:''
    },
    created() {
        this.GetAllOrderDetailsByShop()
    },
    methods: {
        showModal(orderId) {
            //this.$refs['my-modal'].show()
            this.$bvModal.show(`modalxl_${orderId}`)
            
        },
        //店家取消訂單
        hideModal_cancel(order, index) {
            this.currentOrderdetail = { ...order }
            axios.put(`/api/UpdateOrderState_Cancel`, this.currentOrderdetail)
                .then((res) => {
                    if (res.data.status == 20000 && res.data.result == true) {
                        this.$bvModal.hide(`modalxl_${index}`)
                        //this.$bvModal.hide('my-modal')
                        toastr.error('訂單已取消')
                        this.GetAllOrderDetailsByShop()
                    }
                    else {
                        toastr.error('取消失敗')
                    }
                })
                .catch((error) => { console.log(error) })
        },
        //店家接單
        hideModal_accept(order, index) {
            this.currentOrderdetail = { ...order }
            
            axios.put(`/api/UpdateOrderState_Accept`, this.currentOrderdetail)
                .then((res) => {
                    if (res.data.status == 20000 && res.data.result == true) {
                        //this.$refs['my-modal'].hide()
                        this.$bvModal.hide(`modalxl_${ index }`)
                        toastr.success('成功接單')
                        this.GetAllOrderDetailsByShop()
                    }
                    else {
                        toastr.error('接單失敗')
                    }
                })
                .catch((error) => { console.log(error) })
        },
        //完成訂單
        hideModal_complete(order, index) {
            this.currentOrderdetail = { ...order }

            axios.put(`/api/UpdateOrderState_Complete`, this.currentOrderdetail)
                .then((res) => {
                    if (res.data.status == 20000 && res.data.result == true) {
                        //this.$refs['my-modal'].hide()
                        this.$bvModal.hide(`modalxl_${index}`)
                        toastr.success('完成訂單')
                        this.GetAllOrderDetailsByShop()
                    }
                    else {
                        toastr.error('訂單未完成')
                    }
                })
                .catch((error) => { console.log(error) })
        },
        acceptAll_hide() {
            if (this.orderdetailListO_Establish.length < 1) {
                toastr.error('Oops! 沒有需要接的訂單:(')
            }
            else {
                axios.put(`/api/UpdateOrderState_AcceptALL`, this.orderdetailListO_Establish)
                    .then((res) => {
                        if (res.data.status == 20000 && res.data.result == true) {
                            this.$bvModal.hide('bv-modal-accept')
                            toastr.success('完成訂單成功')
                            location.reload()
                        }
                        else {
                            toastr.error('接單失敗')
                        }
                    })
                    .catch((err) => {
                        console.log(err)
                    })
            }
        },
        cancelAll_hide() {
            if (this.orderdetailListO_Establish.length < 1) {
                toastr.error('Oops! 沒有訂單可以取消:(')
            }
            else {
                axios.put(`/api/UpdateOrderState_CancelALL`, this.orderdetailListO_Establish)
                    .then((res) => {
                        if (res.data.status == 20000 && res.data.result == true) {
                            this.$bvModal.hide('bv-modal-cancel')
                            toastr.success('取消訂單成功')
                            location.reload()
                        }
                        else {
                            toastr.error('取消失敗')
                        }
                    })
                    .catch((err) => {
                        console.log(err)
                    })
            }
            
        },
        completeAll_hide() {
            if (this.orderdetailListO_Accept.length < 1) {
                toastr.error('訂單已經都完成啦!')
            }
            else {
                axios.put(`/api/UpdateOrderState_CompletelALL`, this.orderdetailListO_Accept)
                    .then((res) => {
                        if (res.data.status == 20000 && res.data.result == true) {
                            this.$bvModal.hide('bv-modal-complete')
                            toastr.success('訂單全部完成')
                            location.reload()
                        }
                        else {
                            toastr.error('完成失敗')
                        }
                    })
                    .catch((err) => {
                        console.log(err)
                    })
            }
            
        },
        //把訂單資料 [Get]下來
        GetAllOrderDetailsByShop() {
            axios.get(`/api/OrderDetails_Shop/${shopId}`)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.orderdetailListO = res.data.result
                        this.orderdetailListO_Establish = this.orderdetailListO.filter(os => os.orderState == "OrderEstablished")
                        this.orderdetailListO_Accept = this.orderdetailListO.filter(os => os.orderState == "OrderAccepted")
                        this.orderdetailListO_Complete = this.orderdetailListO.filter(os => os.orderState == "OrderComplete")
                        this.orderdetailListO_Cancel = this.orderdetailListO.filter(os => os.orderState == "OrderRejected_CancelledByShop" || os.orderState == "OrderRejected_Cancelled")
                        this.filters = this.orderdetailListO_Complete
                        this.filters_cancel = this.orderdetailListO_Cancel
                        this.shopList = this.orderdetailListO.map(s => s.shopName)
                        this.shop = Array.from(new Set(this.shopList))[0]
                        this.acceptAllList = this.orderdetailListO_Establish.map(o => JSON.parse(o.orderId))
                    }
                    else {
                        console.warn('撈資料失敗')
                    }
                })
                .catch((error) => { console.log(error)})
        },
        highlightMatches_Complete(text) {
            const matchExists = text
                .toString()
                .includes(this.filter.toString());
            if (!matchExists) return text;

            const re = new RegExp(this.filter, "ig");
            return text.replace(re, matchedText => `<strong class="text_blue">${matchedText}</strong>`);
        },
        highlightMatches_Cancel(text) {
            
            const matchExists = text
                .toString()
                .includes(this.filter_cancel.toString());
            if (!matchExists) return text;
            debugger
            const re = new RegExp(this.filter_Cancel, "ig");
            return text.replace(re, matchedText => `<strong class="text_blue">${matchedText}</strong>`);
        },
        filteredRows_Complete() {
            
            return this.orderdetailListO_Complete.filter(row => {
                const usernames = row.userName.toString()
                const orderStamp = row.orderStamp.toLowerCase();
                const searchTerm = this.filter.toString();

                return (
                    usernames.includes(searchTerm) || orderStamp.includes(searchTerm)
                );
            });
        },
        filteredRows_Cancel() {
            return this.orderdetailListO_Cancel.filter(row => {
                const usernames = row.userName.toString()
                const orderStamp = row.orderStamp.toLowerCase();
                const searchTerm = this.filter_Cancel.toString();

                return (
                    usernames.includes(searchTerm) || orderStamp.includes(searchTerm)
                );
            });
        }
    },
    watch: {
        filter: {
            handler() {
                //有輸入關鍵字的時候，要把dataShow換成filters[]資料
                if (this.filter == "") {
                    this.filters = this.orderdetailListO_Complete
                    return
                }
                const searchTerm = this.filter.toString()
                this.filters = this.orderdetailListO_Complete.filter(row => {
                    const usernames = row.userName.toString()
                    const orderStamp = row.orderStamp
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
        },
        filter_cancel: {
            handler() {
                if (this.filter_cancel == "") {
                    this.filters_cancel = this.orderdetailListO_Cancel
                    return
                }
                
                const searchTerm = this.filter_cancel.toString()
                this.filters_cancel = this.orderdetailListO_Cancel.filter(row => {
                    const usernames = row.userName.toString()
                    const orderStamp = row.orderStamp.toLowerCase()
                    const orderState = row.orderState.toString()
                    const pickUpTime = row.pickUpTime.toString()
                    const takeMethod = row.takeMethod.toString()
                    const receiptType = row.receiptType.toString()
                    const orderDate = row.orderDate.toString()
                    const coupon = row.coupon.toString()
                    const searchTerm = this.filter_cancel.toString()

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
            return this.orderdetailListO_Establish ? this.orderdetailListO_Establish.length : 0
        },
        showDataList() {
            let tempArr = JSON.parse(JSON.stringify(this.orderdetailListO_Establish))
            return tempArr.splice((this.currentPage - 1) * this.perPage, this.perPage)
        },
        total_Complete() {
            return this.filters ? this.filters.length : 0
        },
        showDataList_Complete() {
            let tempArr = JSON.parse(JSON.stringify(this.filters))
            return tempArr.splice((this.currentPage - 1) * this.perPage, this.perPage)
        },
        total_Cancel() {
            return this.filters_cancel ? this.filters_cancel.length : 0
        },
        showDataList_Cancel() {
            let tempArr = JSON.parse(JSON.stringify(this.filters_cancel))
            return tempArr.splice((this.currentPage - 1) * this.perPage, this.perPage)
        }
    }
}
)

function backToBranOrder() {
   
    window.location.href = ` /Order/OrderDetailsByBrand`
}