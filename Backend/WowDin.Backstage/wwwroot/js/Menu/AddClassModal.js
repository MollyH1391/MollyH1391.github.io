export default {
    data() {
        return {
            addedClass: '',
            nameState: null,
            submittedNames: [],
        }
    },
    methods: {
        checkFormValidity() {
            const valid = this.$refs.form.checkValidity()
            this.nameState = valid
            return valid
        },
        resetModal() {
            this.name = ''
            this.nameState = null
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
            this.$emit('create-class', this.addedClass)
            // Hide the modal manually
            this.$nextTick(() => {
                this.$bvModal.hide('modal-prevent-closing')
            })
        } 
    },
    template: /*html*/
    `<div class="text-end">
        <b-modal
        id="modal-prevent-closing"
        ref="modal"
        title="新增類別"
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
            label="分類名稱"
            label-for="name-input"
            invalid-feedback="請輸入名稱"
            :state="nameState"
            >
            <b-form-input
                id="name-input"
                v-model="addedClass"
                :state="nameState"
                required
            ></b-form-input>
            </b-form-group>
        </form>
        </b-modal>
    </div>`
}