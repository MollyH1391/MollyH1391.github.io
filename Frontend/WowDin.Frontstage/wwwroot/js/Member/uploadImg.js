//const API_BASE_URL = 'https://localhost:5001/UploadApi'
const API_BASE_URL = 'https://wowdin.azurewebsites.net/UploadApi'

const app = new Vue({
    el: '#app',
    data: {
        imgFile: '',
        currentImgUrl: photo,
        imgErrorMsg: '',
        uploadBusyMsg: ''
    },
    methods: {
        uploadImg() {
            
            this.uploadBusyMsg = '上傳中，請稍候...'
            const file = this.$refs.imgFile.files[0]
            console.log(file)
            const form = new FormData()
            form.append('file', file)
            console.log(form.get('file'))

            axios.post(`${API_BASE_URL}/uploadImg`, form)
                .then((res) => {
                    console.log(res)
                    if (res.data.isSuccess == true) {
                        toastr.success('上傳成功')
                        this.currentImgUrl = res.data.imgUrl
                    } else {
                        toastr.warning('上傳失敗')
                        this.imgErrorMsg = res.data.msg
                    }
                    this.uploadBusyMsg = ''
                })
                .catch((error) => { console.log(error) })
        }
    }
})