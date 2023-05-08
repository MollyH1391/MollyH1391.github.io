//const API_BASE_URL = 'https://localhost:5001/api/MemberApi'
const API_BASE_URL = 'https://wowdin-backstage.azurewebsites.net/api/MemberApi'

let app = new Vue({
    el: '#app',
    data: {
        isBusy: true,
        filter: null,
        filterOn: [],
        sortBy: '',
        sortDesc: false,
        sortDirection: 'asc',
        sortDirection: 'asc',
        fields: [
            //欄位處理
            { key: 'name', label: '名稱', class: 'text-center' },
            { key: 'cardImgUrl', label: '圖片', class: 'text-center' },
            { key: 'range', label: '消費次數門檻', class: 'text-center' },
            { key: 'actions', label: '操作', class: 'text-center' }
        ],
        totalRows: 1,
        currentPage: 1,
        perPage: 5,
        pageOptions: [5, 10, 25, 100],
        cardGradingList: [],
        createCardData: { //傳回資料庫的值
            name: '',
            cardImgUrl: '',
            range: ''
        },
        createCardDataCheck: {
            nameError: false,
            nameErrorMsg: '',
            cardImgUrlError: false,
            cardImgUrlErrorMsg: '',
            rangeError: false,
            rangeErrorMsg: ''
        },
        updateCardData: {
            name: '',
            cardImgUrl: '',
            range: ''
        },
        updateCardDataCheck: {
            nameError: false,
            nameErrorMsg: '',
            rangeError: false,
            rangeErrorMsg: ''
        },
        modalErrorMsg: '',
        currentCard: {},
        addVerify: false,
        editVerify: true,
        brandImgFile: [],
        brandEditImgFile: [],
        uploadBusy: false,
        showImg: {
            isDisplay: 'none',
            showImg: ''
        }
    },
    created() {
        this.getAllData()
    },
    watch: {
        'createCardData.name': {
            handler() {
                if (this.createCardData.name == '') {
                    this.createCardDataCheck.nameError = true
                    this.createCardDataCheck.nameErrorMsg = '名稱不得為空'
                }
                else {
                    this.createCardDataCheck.nameError = false
                    this.createCardDataCheck.nameErrorMsg = ''
                }
                this.checkVerify()
            }
        },
        'createCardData.cardImgUrl': {
            handler() {
                if (this.createCardData.cardImgUrl == '') {
                    this.createCardDataCheck.cardImgUrlError = true
                    this.createCardDataCheck.cardImgUrlErrorMsg = '請上傳圖片'
                }
                else {
                    this.createCardDataCheck.cardImgUrlError = false
                    this.createCardDataCheck.cardImgUrlErrorMsg = ''
                }
                this.checkVerify()
            }
        },
        'createCardData.range': {
            handler() {
                console.log(this.createCardData.range)
                if (this.createCardData.range == '') {
                    this.createCardDataCheck.rangeError = true
                    this.createCardDataCheck.rangeErrorMsg = '消費次數不得為空'
                }
                else if (this.createCardData.range <= 0) {
                    this.createCardDataCheck.rangeError = true
                    this.createCardDataCheck.rangeErrorMsg = '消費次數不得小於等於0'
                }
                else {
                    this.createCardDataCheck.rangeError = false
                    this.createCardDataCheck.rangeErrorMsg = ''
                }
                this.checkVerify()
            }
        },
        'updateCardData.name': {
            handler() {
                if (this.updateCardData.name == '') {
                    this.updateCardDataCheck.nameError = true
                    this.updateCardDataCheck.nameErrorMsg = '名稱不得為空'
                }
                else {
                    this.updateCardDataCheck.nameError = false
                    this.updateCardDataCheck.nameErrorMsg = ''
                }
                this.checkUpdateVerify()
            }
        },
        'updateCardData.range': {
            handler() {
                if (this.updateCardData.range == '') {
                    this.updateCardDataCheck.rangeError = true
                    this.updateCardDataCheck.rangeErrorMsg = '消費次數不得為空'
                }
                else if (this.updateCardData.range <= 0) {
                    this.updateCardDataCheck.rangeError = true
                    this.updateCardDataCheck.rangeErrorMsg = '消費次數不得小於等於0'
                }
                else {
                    this.updateCardDataCheck.rangeError = false
                    this.updateCardDataCheck.rangeErrorMsg = ''
                }
                this.checkUpdateVerify()
            }
        }
    },
    methods: {
        onFiltered(filteredItems) {
            this.totalRows = filteredItems.length
            this.currentPage = 1
        },
        getAllData() {
            axios.get(`${API_BASE_URL}/getCardGrading`)
                .then((res) => {
                    if (res.status == 200) {
                        this.cardGradingList = res.data
                        this.totalRows = this.cardGradingList.length
                        this.isBusy = false
                    }
                    else {
                        toastr.warning('資料讀取失敗')
                    }
                })
        },
        createCard(file) {
            this.uploadBusy = true
            const form = new FormData()
            form.append('file', file)

            axios.post(`${API_BASE_URL}/uploadImg`, form)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.createCardData.cardImgUrl = String(res.data.result)

                        axios.post(`${API_BASE_URL}/createCardGrading`, this.createCardData)
                            .then((res) => {
                                if (res.data.status == 20000) {
                                    toastr.success('新增成功')
                                    this.getAllData()
                                    $('#create-modal').modal('hide')
                                    this.initData()
                                } else {
                                    toastr.warning('新增失敗')
                                    this.uploadBusy = false
                                    this.modalErrorMsg = res.data.msg
                                }
                            })
                            .catch((error) => { console.log(error) })

                    } else {
                        toastr.warning('上傳失敗')
                        this.uploadBusy = false
                    }
                    this.checkVerify()
                    this.uploadBusy = false
                })
                .catch((error) => { console.log(error) })
        },
        deleteCard(card) {
            this.$bvModal.msgBoxConfirm('確定刪除資料?', {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
                .then((value) => {
                    if (value) {
                        axios.delete(`${API_BASE_URL}/deleteCard`, { data: card.item })
                            .then((res) => {
                                if (res.data.status == 20000) {
                                    toastr.success('刪除成功')
                                    this.getAllData()
                                } else {
                                    toastr.warning('刪除失敗')
                                }
                            })
                            .catch((error) => { console.log(error) })
                    } else {
                        toastr.info('取消變更')
                    }
                })
                .catch((error) => { console.log(error) })
        },
        selectEdit(card) {
            let copyCard = { ...card.item }
            this.currentCard = copyCard
        },
        updateCard() {
            axios.put(`${API_BASE_URL}/updateCard`, this.currentCard)
                .then((res) => {
                    if (res.data.status == 20000) {
                        toastr.success('保存成功')
                        this.getAllData()
                        $('#edit-modal').modal('hide')
                    } else {
                        toastr.warning('保存失敗')
                        this.uploadBusy = false
                        this.modalErrorMsg = res.data.msg
                    }
                })
                .catch((error) => { console.log(error) })
        },
        checkVerify() {
            for (let prop in this.createCardDataCheck) {
                if (this.createCardDataCheck[prop] == true) {
                    this.addVerify = false
                    return
                }
                this.addVerify = true
            }
        },
        checkUpdateVerify() {
            for (let prop in this.updateCardDataCheck) {
                if (this.updateCardDataCheck[prop] == true) {
                    this.editVerify = false
                    return
                }
                this.editVerify = true
            }
        },
        uploadImg(file) {
            this.uploadBusy = true
            const form = new FormData()
            form.append('file', file)

            axios.post(`${API_BASE_URL}/uploadImg`, form)
                .then((res) => {
                    if (res.data.status == 20000) {
                        toastr.success('上傳成功')
                        this.createCardData.cardImgUrl = String(res.data.result)
                        this.currentCard.cardImgUrl = String(res.data.result)
                    } else {
                        toastr.warning('上傳失敗')
                        this.errorMsg.uploadImgErrorMsg = res.data.msg
                    }
                    this.checkVerify()
                    this.uploadBusy = false
                })
                .catch((error) => { console.log(error) })
        },
        showImgModal(img) {
            this.showImg.isDisplay = 'flex'
            this.showImg.showImg = img
        },
        closeModal() {
            this.showImg.isDisplay = 'none'
        },
        initCreateData() {
            this.createCardData.name = ''
            this.createCardData.range = ''
            this.modalErrorMsg = ''
        },
        initUpdateData() {
            this.modalErrorMsg = ''
        }
    }
})