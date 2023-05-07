// import Prouduct from './Product.js'
export default {
    data() {
        return {
            classTitle:
            {
                inputDisabled: true,
                btnActive: false,
            },
            inputDataCheck: {
                classNameError: false,
                inputErrorMsg: '名稱不可空白',
            },
            deleteProduct: {
                product: null,
                productIdx: -1,
            },
            newProduct: {
                id: 'new',
                name: '',
                menuClassId: -1,
                state: '',
                figure: '',
                basicPrice: 0,
                changeNote: '',
                customs: []
            },
            editClassBtn: false,
            drag: false,
            dragOptions: {
                animation: 200,
                ghostClass: 'ghosting_product', //被拖拉的元件樣式
            }
        }
    },
    props: {
        group: {
            type: Object,
            required: true
        },
        detailstate:{
            type: Boolean,
            required: true
        },
        arrangestate: {
            type: Boolean,
            required: true
        }
    },
    watch: {
        'group.products': function () {
            this.productsNotDeleted = this.group.products.filter(p => p.state != '刪除')
        },
        drag:{
            handler(){
                
                if(!this.drag){
                    console.log(this.drag)

                }
            }
        }
    },
    computed: {
        editClassBtnText() {
            return this.classTitle.btnActive ? '確定' : '修改名稱'
        },
        productsNotDeleted(){
            if (this.group.products.length > 0){
                return this.group.products.filter(p => p.state != '刪除')
            }
        },
        handleClass(){
            if(this.arrangestate){
                return 'handle'
            }
            else{
                return 'd-none'
            }
        }
    },
    methods: {
        // OnMoving: function(evt){
        //     debugger
        //     console.log(evt.related.parentElement.parentElement.parentElement.parentElement)
        // },
        EditClass() {
            this.StopPropagation()
            if (this.group.name == "" ) {
                return
            }
            this.SwitchClassNameInputStatus()
            
            this.$emit('edit-class', this.group.id, this.group.name)
        },
        EditClassNameBtn() {
            this.StopPropagation()
            this.SwitchClassNameInputStatus()
        },
        SwitchClassNameInputStatus() {
            this.editClassBtn = !this.editClassBtn
            this.classTitle.inputDisabled = !this.classTitle.inputDisabled
            this.classTitle.btnActive = !this.classTitle.btnActive
        },
        DeleteClass() {
            this.StopPropagation()
            this.$emit('delete-class', this.group)
        },
        StopPropagation() {
            event.stopPropagation()
        },
        CreateProduct() {
            this.StopPropagation()
            let string = 'newProdcut' + this.group.id
            this.$bvModal.show(string)
            // this.$emit('create-product', this.group.id, this.addProduct)
        },
        CopyCustom(productId){
            this.$emit('copy-custom', productId)
        },
        DeleteProduct(product) {
            this.$emit('delete-product', this.group, product)
        },
        UpdateProduct(product){
            this.$emit('update-product', this.group.id, product)
        },
    },
    template: /*html*/ `
    <div class="class_accordion" role="tablist" >
        <b-card no-body class="mb-1">
            <b-card-header header-tag="header" class="p-1" role="tab">
                <b-button block v-b-toggle="'accordion-' + group.id" variant="dark" class="class_accordion_btn fw-bold border-0">
                    <div class="class_title d-flex align-items-center justify-content-between px-2">
                        <span :class="handleClass">⋮⋮</span>
                        <div class="col-4 text-start d-flex align-items-center">
                            <input class="group_name_input rounded_corner_small pl-2" 
                            :disabled="classTitle.inputDisabled"
                            v-model="group.name"
                            @click="StopPropagation"
                            :placeholder="inputDataCheck.inputErrorMsg">
                        </div>
                        <div class="col class_btns d-flex justify-content-end">
                            <b-button v-if="editClassBtn" variant="outline-warning" @click="EditClass" class="ml-1">確定</b-button>
                            <b-button v-else="editClassBtn" variant="outline-warning" @click="EditClassNameBtn" class="ml-1">修改名稱</b-button>
                            <b-button variant="outline-success" class="add_product_btn" @click="CreateProduct" class="ml-1">新增產品</b-button>
                            <product-details-modal
                            :product="newProduct"
                            :modalId="'newProdcut' + group.id"
                            :isNew="true"
                            @update-product="UpdateProduct"
                            >
                            </product-details-modal>
                            <b-button variant="outline-danger" @click="DeleteClass" class="ml-1">刪除類別</b-button>
                        </div>
                    </div>
                </b-button>
            </b-card-header>
            <b-collapse :id="'accordion-' + group.id" class="product_container">
                <b-list-group class="p-2"
                is="draggable" 
                v-model="group.products"
                handle=".handle"
                tag="div"
                v-on:start="drag = true"
                v-on:end="drag = false"
                v-bind="dragOptions"
                group="{name: 'products'}"
                >
                    <transition-group type="transition" :name="drag ? null : 'flip-list'">
                        <product-list-item
                        v-for="(groupItem, index) in productsNotDeleted"
                        :product="groupItem"
                        :key="'product' + groupItem.id"
                        :arrangestate="arrangestate"
                        :detailstate="detailstate"
                        @copy-custom="CopyCustom"
                        @delete-product="DeleteProduct">
                        </product-list-item>
                    </transition-group>
                </b-list-group>
            </b-collapse>
            <product-details-modal
            v-for="(groupItem, index) in productsNotDeleted"
            :product="groupItem"
            :modalId="'editProduct' + groupItem.id"
            :isNew="false"
            :key="'productModal' + groupItem.id"
            @update-product="UpdateProduct"
            >
            </product-details-modal>
        </b-card>
    </div>`,
    components: {
        'product-list-item': {
            data() {
                return {
                    productTitle:
                    {
                        inputDisabled: true,
                        btnActive: false,
                    },
                    inputDataCheck: {
                        inputErrorMsg: '名稱不可空白',
                    },
                }
            },
            props: {
                product: {
                    type: Object,
                    required: true
                },
                detailstate :{
                    type: Boolean,
                    required: true
                },
                arrangestate: {
                    type: Boolean,
                    required: true
                }
            },
            computed: {
                disable() {
                    return this.productTitle.inputDisabled
                },
                // btnState() {
                //     return this.productTitle.btnActive ? '確定' : '修改名稱'
                // },
                itemActive() {
                    return this.productTitle.inputDisabled ? '' : 'border-2 border-dark'
                },
                handleClass(){
                    if(this.arrangestate){
                        return 'handle'
                    }
                    else{
                        return 'd-none'
                    }
                },
            },
            methods: {
                EditProductNameBtn() {
                    event.stopPropagation()
                    if (this.product.name == "") {
                        return
                    }
                    this.productTitle.inputDisabled = !this.productTitle.inputDisabled
                    this.productTitle.btnActive = !this.productTitle.btnActive
                    let input = event.srcElement.parentElement.parentElement.querySelector('input')
                    setTimeout(function () {
                        input.focus()
                    }, 100)
                },
                EditProductName() {
                    event.stopPropagation()
                },
                DeleteProduct() {
                    this.$emit('delete-product', this.product)
                },
                CopyCustom(){
                    this.$emit('copy-custom', this.product.id)
                }
            },
            template: /*html */ `
            <div class="menu_product">
                <div class="product_top ps-4 py-1 d-flex align-items-center justify-content-between " 
                :class="itemActive">
                    <span :class="handleClass">⋮⋮</span>
                    <div class="col col-4 product_name_input_warp">
                        <input class="product_name_input rounded_corner_small w-100"
                        v-model="product.name"
                        :disabled="disable"
                        @click="EditProductName"
                        :placeholder="inputDataCheck.inputErrorMsg">
                    </div>
                    <div class="col product_btns d-flex justify-content-end">
                        <b-button size="sm" v-b-modal="'editProduct' + product.id" variant="info" class="mx-1">編輯內容</b-button>
                        <b-button size="sm" @click="CopyCustom" variant="success" class="mx-1">複製題型</b-button>
                        <b-button size="sm" class="btn btn-danger mx-1" @click="DeleteProduct">刪除產品</b-button>
                    </div>
                    
                </div>
                <div v-if="detailstate" class="product_content bg_page px-4 py-2">
                    <div class="product_detail_title row align-items-center fw-bold">
                        <p class="col col-1 text-start mb-0 small">
                            基本價格 
                        </p>
                        <p class="col col-1 text-start mb-0 small">
                            狀態
                        </p>
                        <p class="col col-2 text-start mb-0 small">
                            產品圖片
                        </p>
                        <p class="col col-8 text-start text-nowrap mb-0 small">
                            題型與選項
                        </p>
                    </div>
                    <div class="product_detail_content row align-items-center">
                        <p class="col col-1 text-start  mb-0 small">
                            {{ product.basicPrice | currency }}
                        </p>
                        <p class="col col-1 text-start  mb-0 small">
                            {{ product.state }}
                        </p>
                        <p class="col col-2 text-start mb-0 small product_img_detail_wrap">
                            <img v-if="product.figure != null" :src="product.figure" class="product_img_detail rounded_corner_small my-2">
                            <span v-else class="small">無</span>
                        </p>
                        <div class="col col-8">
                            <p v-for="(custom, idx) in product.customs"
                            :key="custom.id"
                            class="product_custom text-start mb-0 d-flex flex-wrap overflow-auto align-items-center">
                                <span class="custom_title small text-nowrap ">
                                {{ custom.name }} ( {{ custom.necessary ? '必選' : '非必選' }}, {{ custom.maxAmount | maxAmountFilter }} ):  
                                </span>
                                <b-badge pill variant="info"
                                v-for="(selection, idx) in custom.selections"
                                :key="selection.idx"
                                class="mx-1">
                                {{ selection.name }} {{ selection.price | currencyNotZero }}
                                </b-badge>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            `,
            filters: { //渲染之前先做處理
                currency(val) {
                    return `NT$ ${val.toLocaleString('en-US')}`
                },
                currencyNotZero(val) {
                    if (val != 0) {
                        return `+ NT$ ${val.toLocaleString('en-US')}`
                    }
                    else return ''
                },
                maxAmountFilter(val) {
                    if (val != null) return `最多選${val}項`
                    else return '無選取上限'
                },
            }
        },
        'product-details-modal': {
            props: {
                product: {
                    type: Object,
                    required: true
                }, 
                isNew: {
                    type: Boolean,
                    required: true
                },
                modalId: {
                    type: String,
                    required: true
                }
            },
            data() {
                return {
                    title: this.isNew ? '新增產品' : '編輯產品',
                    modalForm: {
                        stateSelected: '',
                        stateOptions: ['上架', '下架']
                    },
                    newCustomCounting: 0,
                    deleteCustom: {
                        custom: null,
                        customIdx: -1
                    },
                    formVerify: false,
                    inputCheckState: {
                        productNameState: null,
                        priceState: null,
                        stateState: null,
                    },
                    checkInfo:{
                        emptyCustoms: [],
                        priceInvalidFeedback: '',
                    },
                    figure: {
                        hasFigure: false,
                        figureLoading: false,
                        uploadResult: ''
                    }
                }
            },
            watch: {
                'product.name': {
                    handler(){
                        if (this.product.name == '' || this.product.name == null){
                            this.inputCheckState.productNameState = false
                        }
                        else{
                            this.inputCheckState.productNameState = null
                        }
                    },
                    immediate: true,
                },
                'product.basicPrice': {
                    handler(){
                        let regex = /^(0|[1-9][0-9]*)$/
                        if (this.product.basicPrice == ''){
                            this.checkInfo.priceInvalidFeedback = '請輸入金額'
                            this.inputCheckState.priceState = false
                        }
                        else{
                            if(!regex.test(this.product.basicPrice)){
                                this.checkInfo.priceInvalidFeedback = '格式錯誤'
                                this.inputCheckState.priceState = false
                            }
                            else{
                                this.inputCheckState.priceState = null
                            }
                        }
                    },
                    immediate: true
                },
                'product.state': {
                    handler(){
                        if (this.product.state == '' || this.product.state == null){
                            this.inputCheckState.stateState = false
                        }
                        else{
                            this.inputCheckState.stateState = null
                        }
                    },
                    immediate: true
                },
                'product.figure': {
                    handler(){
                        if (this.product.figure == '' || this.product.figure == null){
                            this.figure.hasFigure = false
                        }
                        else{
                            this.figure.hasFigure = true
                        }
                    },
                    immediate: true
                },
                'product.uploadFigure':{
                    handler(){
                        this.UploadFigure(this.product.uploadFigure)
                    }
                }
            },
            methods: {
                checkFormValidity() {
                    for(let prop in this.inputCheckState){
                        if(this.inputCheckState[prop] == false){
                            this.formVerify = false
                            return
                        }
                    }
                    this.formVerify = true
                },
                checkCustomHasSelection(){
                    if(this.product.customs.length > 0){
                        this.product.customs.forEach(x => {
                            if(x.selections.length == 0){
                                this.checkInfo.emptyCustoms.push(x.name)
                            }
                        })
                        if (this.checkInfo.emptyCustoms.length > 0){
                            toastr.warning(`題型${this.checkInfo.emptyCustoms.join('、')}沒有選項`)
                            this.checkInfo.emptyCustoms.length = 0
                            return false
                        }
                    }
                    return true
                },
                handleOk(bvModalEvt) {
                    // Prevent modal from closing
                    bvModalEvt.preventDefault()
                    // Trigger submit handler
                    this.handleSubmit()
                    
                },
                handleSubmit() {
                    // Exit when the form isn't valid
                    this.checkFormValidity()
                    
                    if (!this.formVerify) {
                        return
                    }
                    if (!this.checkCustomHasSelection()){
                        return
                    }
                    
                    this.$emit('update-product', this.product)
                    // Hide the modal manually
                    this.$nextTick(() => {
                        this.$bvModal.hide(this.modalId)
                        if(this.isNew){
                            this.ResetProductModal()
                        }
                    })
                },
                resume(){
                    toastr.info('取消編輯')
                    if(this.isNew){
                        this.ResetProductModal()
                    }
                },
                ResetProductModal(){
                    this.product.name = ''
                    this.product.state = ''
                    this.product.figure = ''
                    this.product.basicPrice = 0
                    this.product.changeNote = ''
                    this.product.customs = []
                    this.product.deletedCustoms = []
                },
                CreateCustom() {
                    let newCustom = {
                        id: 'newCustom',
                        name: '',
                        maxAmount: 1,
                        necessary: true,
                        productId: this.product.id,
                        selections: []
                    }
                    newCustom.id += this.newCustomCounting
                    this.product.customs.push(newCustom)
                    this.newCustomCounting++
                    //console.log(this.newCustom.id)
                },
                DeleteCustom(customId) {
                    this.deleteCustom.custom = this.product.customs.find(x => x.id == customId)
                    this.deleteCustom.customIdx = this.product.customs.indexOf(this.deleteCustom.custom)
                    this.product.customs.splice(this.deleteCustom.customIdx, 1)
                    //待確認
                    this.product.deletedCustoms.push(customId)
                    this.deleteCustom.custom = null
                    this.deleteCustom.customIdx = -1
                },
                CreateSelection(newSelection){
                    let custom = this.product.customs.find(x => x.id == newSelection.customId)
                    console.log(custom)
                    custom.selections.push(newSelection)
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

                        this.product.figure = result.FileUrl
                        this.figure.uploadResult = result.Message
                        this.figure.figureLoading = false
                    })
                    .catch((err) => {
                        console.log(err)
                    })
                },
                RemoveFigure(){
                    this.product.figure = ''
                    this.figure.uploadResult = '圖片已移除'
                    document.querySelector('[for="figure_selector"] .form-file-text').innerText = '選擇檔案'
                }
            },
            template: /*html*/
            `
            <b-modal
            :id="modalId"
            ref="modal"
            size="xl"
            scrollable
            header-bg-variant="info"
            header-text-variant="light"
            @ok="handleOk"
            @cancel="resume"
            no-close-on-backdrop
            no-close-on-esc
            content-class="shadow"
            >
                <template v-slot:modal-header>
                    <h2 class="fw-bold fz_18 mb-0">{{ title }}</h2>
                </template>
                <template #modal-footer="{ ok, cancel }">
                    <b-button variant="success" @click="CreateCustom">
                        新增題型
                    </b-button>
                    <b-button variant="secondary" @click="cancel()">
                        取消
                    </b-button>
                    <b-button variant="primary" @click="ok()">
                        確定
                    </b-button>
                    
                </template> 
                
                <form ref="form" @submit.stop.prevent="handleSubmit" class="p-4">
                    <div class="row">
                        <div class="col col-4 my-1 d-flex align-items-center">
                        <b-form-group
                        label="產品名稱: "
                        label-for="product_name"
                        invalid-feedback="請輸入名稱"
                        :state="inputCheckState.productNameState"
                        >
                            <b-form-input id="product_name" 
                            v-model="product.name" 
                            :state="inputCheckState.productNameState"
                            placeholder="名稱不得為空" 
                            required>
                            </b-form-input>
                        </b-form-group>
                        
                        </div>
                        <div class="col col-4 my-1 d-flex align-items-center">
                            <b-form-group
                            label="基本金額: "
                            label-for="basic_price"
                            :invalid-feedback="checkInfo.priceInvalidFeedback"
                            :state="inputCheckState.priceState"
                            >
                                <b-form-input id="basic_price" 
                                v-model="product.basicPrice" 
                                :state="inputCheckState.priceState"
                                class="d-inline-block"
                                placeholder="金額不得為空" 
                                required>
                                </b-form-input>
                            </b-form-group>
                        </div>
                        <div class="col col-4 my-1">
                            <b-form-group
                            label="產品狀態: "
                            label-for="btn-radios-1"
                            invalid-feedback="請選擇狀態"
                            :state="inputCheckState.stateState"
                            >
                            <b-form-radio-group
                            id="btn-radios-1"
                            v-model="product.state"
                            :options="modalForm.stateOptions"
                            :state="inputCheckState.stateState"
                            name="radios-btn-default"
                            checked="product.state"
                            button-variant="outline-primary"
                            buttons
                            ></b-form-radio-group>
                            </b-form-group>
                        </div>
                        
                    </div>
                    <div class="">
                        <p class="mb-0 mt-2">產品圖片: </p>
                        <div class="row">
                            <div class="col product_img_modal_wrap text-center rounded_corner_small mb-2 d-flex align-items-center justify-content-center text-center">
                                <img v-if="figure.hasFigure" class="product_img_modal" :src="product.figure">
                                <span v-else="figure.hasFigure" class="text-secondary">目前沒有圖片</span>
                            </div>
                            <div class="col ml-4">
                                <div class="row align-items-center">
                                    <div class="col col-6">
                                        <b-overlay :show="figure.figureLoading" rounded="lg" opacity="0.6">
                                            <template #overlay>
                                                <div class="d-flex align-items-center">
                                                    <b-spinner small type="grow" variant="secondary" class="m-2"></b-spinner>
                                                    <b-spinner small type="grow" variant="secondary" class="m-2"></b-spinner>
                                                    <b-spinner small type="grow" variant="secondary" class="m-2"></b-spinner>
                                                </div>
                                            </template>
                                            <b-form-file 
                                            id="figure_selector"
                                            v-model="product.uploadFigure" 
                                            placeholder="選擇檔案"
                                            size="sm"
                                            browse-text="瀏覽"
                                            accept="image/jpeg, image/png, image/gif"
                                            ></b-form-file>
                                        </b-overlay>
                                    </div>
                                    <div class="col d-flex">
                                        
                                        <b-button variant="danger" @click="RemoveFigure" :disabled="!figure.hasFigure" class="ml-1">
                                            移除圖片
                                        </b-button>
                                    </div>
                                </div>
                                <span class="small">(接受JPG、JPEG、PNG檔案)</span>
                                <p class="m-0 small text-primary">{{ figure.uploadResult }}</p>
                            </div>
                        </div>
                        
                    </div>
                    <div class="">
                        <p class="mb-0 mt-2">題型與選項: </p>
                        <custom-form
                        v-for="(custom, index) in product.customs"
                        :custom="custom"
                        :key="custom.id"
                        @delete-custom="DeleteCustom"
                        @create-selection="CreateSelection">
                        </custom-form>
                    </div>
                </form>
            </b-modal>
            `,
            components: {
                'custom-form': {
                    data() {
                        return {
                            selectionInput: "",
                            customForm: {
                                necessaryOpt: ['必選', '非必選'],
                                necessarySelected: '',
                                maxAmountDisable: true,
                                unlimitedDisable: true,
                                unlimitedBox: false
                            },
                            addSelection: {
                                name: '',
                                price: 0
                            },
                            newSelectionCounting: 0,
                            deleteSelection: {
                                selection: null,
                                selectionIdx: -1
                            }
                        }
                    },
                    props: {
                        custom: {
                            type: Object,
                            required: true
                        }
                    },
                    watch: {
                        'customForm.necessarySelected': function () {
                            if (this.customForm.necessarySelected == '必選') {
                                this.custom.maxAmount = 1
                                this.custom.necessary = true
                                this.customForm.maxAmountDisable = true
                                this.customForm.unlimitedDisable = true
                                this.customForm.unlimitedBox = false
                            }
                            else {
                                this.custom.necessary = false
                                this.customForm.maxAmountDisable = false
                                this.customForm.unlimitedDisable = false
                            }
                        },
                        'customForm.unlimitedBox': function () {
                            if (this.customForm.unlimitedBox) {
                                this.custom.maxAmount = null
                                this.customForm.maxAmountDisable = true
                            }
                            else {
                                this.custom.maxAmount = 1
                                if (this.customForm.necessarySelected == '非必選') {
                                    this.customForm.maxAmountDisable = false
                                }
                            }
                        }
                    },
                    mounted() {
                        if (this.custom.necessary == true) this.customForm.necessarySelected = '必選'
                        else {
                            this.customForm.necessarySelected = '非必選'
                            if (this.custom.maxAmount == null) {
                                this.customForm.unlimitedBox = true
                            }
                        }
                    },
                    methods: {
                        DeleteCustom() {
                            this.$emit('delete-custom', this.custom.id)
                        },
                        CreateSelection() {
                            if (this.addSelection.name == "") {
                                return
                            }
                            if (this.addSelection.price == null ) {
                                return
                            }
                            let newSelection = { 
                                id: this.custom.id + 'newSelection' + this.newSelectionCounting,
                                customId: this.custom.id,
                                name: this.addSelection.name,
                                price: this.addSelection.price
                            }
                            this.custom.selections.push(newSelection)
                            this.newSelectionCounting++ 
                            this.ResetSelectionModal()
                        },
                        ResetSelectionModal(){
                            this.addSelection.name = ''
                            this.addSelection.price = 0
                        },
                        DeleteSelection(selectionId) {
                            this.deleteSelection.selection = this.custom.selections.find(x => x.id == selectionId)
                            this.deleteSelection.selectionIdx = this.custom.selections.indexOf(this.deleteSelection.selection)
                            this.custom.selections.splice(this.deleteSelection.selectionIdx, 1)
                            //待確認
                            this.custom.deletedSelections.push(selectionId)
                            this.deleteSelection.selection = null
                            this.deleteSelection.selectionIdx = -1
                        }
                    },
                    filters: { //渲染之前先做處理
                        currencyNotZero(val) {
                            if (val != 0) {
                                return `+ NT$ ${val.toLocaleString('en-US')}`
                            }
                            else return ''
                        },
                    },
                    template: /* html */
                    `
                    <div class="bg_page rounded_corner p-4 my-2 position-relative">
                        
                        <button pill variant="outline-secondary" class="delete_custom_btn" @click="DeleteCustom">x</button>
                        
                        <div class="row justify-content-between align-items-center">
                            <div class="col col-4 my-1">
                                <label>題型: </label>
                                <input v-model:value="custom.name" placeholder="名稱不得為空" class="px-2" required>
                            </div>
                            <div class="col col-5 d-flex justify-content-center align-items-center my-1">
                                <label :for="'sb-wrap' + custom.id" class="mb-0 mx-2">最大選取數量: </label>
                                <b-form-spinbutton
                                :id="'sb-wrap' + custom.id"
                                size="sm"
                                wrap min="1"
                                max="25"
                                placeholder="--"
                                class="w-25 mx-2 text-center"
                                v-model:value="custom.maxAmount"
                                :disabled="customForm.maxAmountDisable"></b-form-spinbutton>
                                <b-form-checkbox
                                :id="'unlimited-checkbox' + custom.id"
                                v-model="customForm.unlimitedBox"
                                name="'unlimited-checkbox' + custom.id"
                                :disabled="customForm.unlimitedDisable"
                                class="mx-2"
                                >
                                無上限
                                </b-form-checkbox>
                            </div>
                            <div class="col col-3 my-1">
                                <b-form-radio-group
                                :id="'custom-necessary-btns'+ custom.id"
                                v-model="customForm.necessarySelected"
                                :options="customForm.necessaryOpt"
                                name="radios-btn-default"
                                checked="customForm.necessarySelected"
                                button-variant="outline-primary"
                                buttons
                                required
                                class="text-end"
                                ></b-form-radio-group>
                            </div>
                            
                        </div>
                        
                        <div class="row mt-3 align-items-center">
                            <div class="col col-2">
                                <b-button v-b-modal="'add_selection_modal'+ custom.id" variant="outline-secondary" class="add_selection_btn">新增選項</b-button>
                                <b-modal :id="'add_selection_modal'+ custom.id"
                                size="sm" title="新增選項" ok-only @ok="CreateSelection">
                                    <b-form-group
                                    label="選項名稱: "
                                    label-for="selection_name"
                                    invalid-feedback="請輸入名稱"
                                    >
                                        <b-form-input
                                            id="selection_name"
                                            v-model="addSelection.name"
                                            required
                                        ></b-form-input>
                                    </b-form-group>

                                    <b-form-group
                                    label="選項加價: "
                                    label-for="selection_price"
                                    invalid-feedback="請輸入價格"
                                    >
                                        <b-form-input
                                            id="selection_price"
                                            v-model="addSelection.price"
                                            required
                                        ></b-form-input>
                                    </b-form-group>
                                </b-modal>
                            </div>    
                                
                            <div class="col col-10 d-flex flex-wrap">
                                <div v-for="(selection, index) in custom.selections"
                                :key="'selection' + index"
                                class="text-center m-2">
                                    <b-button-group>
                                        <b-button v-b-modal="'edit_selection_modal' + selection.id" variant="outline-info" class="selection_btn px-3">
                                            {{ selection.name }} {{ selection.price | currencyNotZero }}
                                        </b-button>
                                        <b-button variant="outline-info" @click="DeleteSelection(selection.id)" class="selection_btn">x</b-button>
                                    </b-button-group>
                                </div>
                            </div>
                            
                            <b-modal 
                            v-for="(selection, index) in custom.selections"
                            :key="'selection_modal' + index"
                            :id="'edit_selection_modal' + selection.id" 
                            size="sm" title="編輯選項" ok-only>
                            <label for="selection_name" class=" mb-0">選項名稱: </label>
                            <input id="selection_name" v-model:value="selection.name" placeholder="名稱不得為空" class="col-6 px-2 border-0 border-bottom" required >
                            <label for="selection_price" class=" mb-0">選項加價: </label>
                            <input id="selection_price" v-model:value="selection.price" placeholder="預設為零" class="col-6 px-2 border-0 border-bottom" required >
                            </b-modal>
                        </div>
                        
                    </div>
                    `
                }
            }
        }
    },
}