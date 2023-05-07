export default {
    data() {
        return {
            otherShops: [
            ],
            selectedShops: [],
            selectionState: null,
        }
    },
    props:{
        currentshopid:{
            type: Number,
            required: true
        },
        shoplist:{
            type: Array,
            required: true
        }
    },
    watch: {
        currentshopid: {
            handler(){
                //console.log(this.shoplist)
                if(this.currentshopid != null){
                    this.disableCurrentShop();
                }
            },
            immediate: false
        }
    },
    methods: {
        disableCurrentShop(){
            this.shoplist = this.shoplist.slice(1, this.shoplist.length)
            let currentShop = this.shoplist.find(x => x.id == this.currentshopid)
            let currentShopIdx = this.shoplist.indexOf(currentShop)
            
            this.otherShops = [... this.shoplist]
            //console.log(this.otherShops)
            this.otherShops[currentShopIdx] = { id: currentShop.id, name: currentShop.name + '(目前)', notEnabled: true }
        },
        checkFormValidity() {
            if(this.selectedShops.length == 0){
                this.selectionState = false
                return false
            }
            return true
        },
        resetModal() {
            this.selectedShops = []
            this.selectionState = null
        },
        handleOk(bvModalEvt) {
            // Prevent modal from closing
            bvModalEvt.preventDefault()
            // Trigger submit handler
            this.handleSubmit()
        },
        handleSubmit() {
            // Exit when the form isn't valid
            if (!this.checkFormValidity()) {
                return
            }
            //debugger
            this.$emit('copy-menu', this.selectedShops)
            // Hide the modal manually
            this.$nextTick(() => {
                this.$bvModal.hide('copy-menu-modal')
                this.resetModal()
            })
        } 
    },
    template: /*html*/
    `<div class="text-end">
        <b-modal
        id="copy-menu-modal"
        ref="modal"
        title="複製菜單"
        @show="resetModal"
        @hidden="resetModal"
        @ok="handleOk"
        >
        <template v-slot:modal-ok>
            確定
        </template>
        <template v-slot:modal-cancel>
            取消
        </template>
        <form ref="form" @submit.stop.prevent="handleSubmit">
            <b-form-group
            label="將目前的菜單複製到以下店面:"
            label-for="shop-selection"
            invalid-feedback="請選擇至少一個店面"
            :state="selectionState"
            >
            <b-form-checkbox-group
            v-model="selectedShops"
            :options="otherShops"
            value-field="id"
            text-field="name"
            disabled-field="notEnabled"
            stacked
            >
            </b-form-checkbox-group>
        </form>
        </b-modal>
    </div>`
}