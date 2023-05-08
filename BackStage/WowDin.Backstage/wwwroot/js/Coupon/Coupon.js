const app = new Vue({
    el: "#vue",
    data() {
        return {
            items: [
                { 
                    promoCodeId: '1',
                    promoCodeType: 'type',
                    startTime: '2022',
                    endTime: '2022',
                    promoStatusType: 'status',
                    actions: '',
                    _showDetails: true
                }
            ],
            fields: [
                { key: 'promoCodeId', label: '優惠代碼', sortable: true, sortDirection: 'desc' },
                {
                    key: 'promoCodeType',
                    label: '優惠類型',
                    formatter: (value) => {
                        switch (value) {
                            case 0: return "折價券";
                            case 1: return "單品折扣券";
                            default: return "未分類";
                        }
                    },
                    sortable: true,
                    sortDirection: 'desc',
                    sortByFormatted: true,
                    filterByFormatted: true,
                    class: 'text-center'
                },
                {
                    key: 'startTime',
                    label: '開始時間',
                    formatter: (value) => {
                        return (value == null) ? "" : DateFormat(value, 'YYYY/MM/DD HH:mm:ss');
                    },
                    sortable: true,
                    sortDirection: 'desc',
                    sortByFormatted: true,
                    filterByFormatted: true,
                    class: 'text-center'
                },
                {
                    key: 'endTime',
                    label: '結束時間',
                    formatter: (value) => {
                        return (value == null) ? "" : DateFormat(value, 'YYYY/MM/DD HH:mm:ss');
                    },
                    sortable: true,
                    sortDirection: 'desc',
                    sortByFormatted: true,
                    filterByFormatted: true,
                    class: 'text-center'
                },
                {
                    key: 'promoStatusType',
                    label: '優惠狀態',
                    formatter: (value, key, item) => {
                        //console.log(item);
                        var start = new Date(DateFormat(item.startTime, 'YYYY/MM/DD HH:mm:ss'));
                        var end = new Date(DateFormat(item.endTime, 'YYYY/MM/DD HH:mm:ss'));
                        var today = new Date(DateFormat(Date.now(), 'YYYY/MM/DD HH:mm:ss'));
                        //console.log(item, today, start, end);
                        if (end > today && today > start) {
                            return `活動尚${parseInt(Math.abs(end - today) / (1000 * 3600 * 24), 10)}天`;
                        }
                        else if (today >= end) {
                            return `已過期${parseInt(Math.abs(today - end) / (1000 * 3600 * 24), 10)}天`;
                        }
                        else if (today < start) {
                            return `離開始尚${parseInt(Math.abs(today - start) / (1000 * 3600 * 24), 10)}天`;

                        } else {
                            return "代碼異常";
                        }
                        return value;
                    },
                    sortable: true,
                    sortDirection: 'desc',
                    sortByFormatted: true,
                    filterByFormatted: true,
                    class: 'text-center'
                },
                { key: 'actions', label: '管理', class: 'text-center' }
            ],
            newData: {
                promoCodeId: null,
                promoCodeType: 0,
                promoCodeMoney: 0,
                startTime: null,
                endTime: null,
                status: false
            },
            updateData: {
                promoCodeId: null,
                promoCodeType: 0,
                promoCodeMoney: 0,
                startTime: null,
                endTime: null,
                status: false
            },
            updateDateTime: {
                dateTime: [],
                endDateTime: null,
                dateTimestatus: { status: false, errorMsg: '' }
            },
            dataState: {
                promoCodeId: { status: false, errorMsg: '請輸入優惠代碼。' },
                promoCodeMoney: { status: false, errorMsg: '請輸入正確有效的優惠值。' },
                activityDateTime: { status: false, errorMsg: '請輸入正確有效的日期。' }
            },
            promoCodeTypeOptions: [
                { text: '折價券', value: 0 },
                { text: '單品折扣券', value: 1 }
            ],
            promoCodeMoneyInput: 0,
            activityDateTime: [Date.now(), Date.now()],
            //表格設定
            currentitem: null,
            tabIndex: 0,
            totalRows: 1,
            currentPage: 1,
            perPage: 5,
            pageOptions: [5, 10, 15],
            sortBy: '',
            sortDesc: false,
            sortDirection: 'asc',
            filter: null,
            filterOn: [],

            //描述頁面是否忙碌中，EX:進行非同步作業
            isonStatusBusy: { PageBusy: false, DetailsBusy: false },
            isoffStatusBusy: { PageBusy: false, DetailsBusy: false },

            //url列表
            //for live server
            urllist: {
                ActivatePromoCode: '/api/GetPromoCodeList?status=true',
                NotActivatedPromoCode: '/api/NotActivatedPromoCode',
                UpdatePromoCodeStatus: '/api/UpdatePromoCodeStatus',
                CreatePromoCode: '/api/CreatePromoCode',
                UpdatePromoCode: '/api/UpdatePromoCode',
            },

            // //代碼啟用關閉MessageBox參數
            // ConfirmBoxProps: {
            //     onStatus: { message: '請再次確認是否要啟用代碼', data: { Status: true } },
            //     offStatus: { message: '請再次確認是否要關閉代碼', data: { Status: false } },
            // }
        }
    },
    computed: {
        allValidation() {
            let invalid = Object.keys(this.dataState).some(key => this.dataState[key].status === true);
            return {
                error: invalid,
                errorMsg: invalid ? '尚有無效的輸入欄位' : ''
            }
        }
    },
    watch: {
        items: function () {
            this.totalRows = this.items.length
        },
        tabIndex: function () {
            switch (this.tabIndex) {
                case 0:
                    this.onStatusPage();
                    break;
                case 1:
                    this.offStatusPage();
                    break;
                default:
                    break;
            }
        },
        "newData.promoCodeId": function () {
            this.dataState.promoCodeId.status = false;
            if (!this.items.some(x => x.promoCodeId == this.newData.promoCodeId)
                && this.newData.promoCodeId != null && this.newData.promoCodeId != '') {
                this.dataState.promoCodeId.status = true;
                this.dataState.promoCodeId.errorMsg = '';

            } else if (this.items.some(x => x.promoCodeId == this.newData.promoCodeId)) {
                this.dataState.promoCodeId.errorMsg = '已有相同代碼。';

            } else if (this.newData.promoCodeId == null || this.newData.promoCodeId == '') {
                this.dataState.promoCodeId.errorMsg = '代碼不可為空。';
            } else {
                this.dataState.promoCodeId.errorMsg = '不知名錯誤!';
                toastr.warning("不知名錯誤!");
            }
        },
        "newData.promoCodeType": function () {
            this.promoCodeMoneyInput = 0
        }
        ,
        "promoCodeMoneyInput": function () {
            this.promoCodeMoneyInput = parseFloat(parseFloat(this.promoCodeMoneyInput).toFixed(1));
            this.dataState.promoCodeMoney.status = false;
            var input = this.promoCodeMoneyInput;
            if (input > 0) {
                if (this.newData.promoCodeType == 0) {
                    this.newData.promoCodeMoney = input;
                    this.dataState.promoCodeMoney.status = true;
                    this.dataState.promoCodeMoney.errorMsg = '';
                } else if (this.newData.promoCodeType == 1) {
                    //console.log(input);
                    this.newData.promoCodeMoney = input * 0.1;
                    this.dataState.promoCodeMoney.status = true;
                    this.dataState.promoCodeMoney.errorMsg = '';
                } else {
                    this.dataState.promoCodeId.errorMsg = '不知名錯誤!';
                    toastr.warning("不知名錯誤!");
                }
            } else {
                this.dataState.promoCodeMoney.errorMsg = '優惠值無效。';
            }

        },
        "activityDateTime": function () {
            this.dataState.activityDateTime.status = false;

            if (this.activityDateTime != null && this.activityDateTime != '') {
                if (this.activityDateTime[0] > Date.now()) {
                    this.newData.startTime = DateFormat(this.activityDateTime[0], 'YYYY/MM/DD HH:mm:ss');
                    this.newData.endTime = DateFormat(this.activityDateTime[1], 'YYYY/MM/DD HH:mm:ss');
                    this.dataState.activityDateTime.status = true;
                    this.dataState.activityDateTime.errorMsg = '';
                } else {
                    this.dataState.activityDateTime.errorMsg = '開始時間不可選擇過期的日期時間。';
                }

            } else if (this.activityDateTime == null || this.activityDateTime == '') {
                this.dataState.activityDateTime.errorMsg = '日期時間不可為空。';

            } else {
                toastr.warning("不知名錯誤!");
                this.dataState.activityDateTime.errorMsg = '不知名錯誤!';
            }
        },
        "updateDateTime.dateTime": function () {
            this.updateDateTime.status = false;

            var theendTime = Date.parse(this.updateDateTime.dateTime[1]);
            var thestartTime = Date.parse(this.updateDateTime.dateTime[0]);

            //console.log(this.updateDateTime.dateTime[1]);
            if (this.updateDateTime.dateTime[1] != null && this.updateDateTime.dateTime[1] != '') {
                if (theendTime > Date.now() && theendTime > thestartTime) {
                    this.updateData.startTime = new Date(this.updateDateTime.dateTime[0]);
                    this.updateData.endTime = new Date(this.updateDateTime.dateTime[1]);
                    //this.updateData.endTime = DateFormat(theendTime, 'YYYY/MM/DD HH:mm:ss');
                    //console.log(this.updateData.endTime);
                    this.updateDateTime.status = true;
                    this.updateDateTime.errorMsg = '';

                } else if (theendTime < thestartTime) {
                    this.updateDateTime.errorMsg = '結束日期時間不可早於開始日期時間。';
                }
                else if (theendTime < Date.now()) {
                    this.updateDateTime.errorMsg = '結束日期時間不可早於現在。';
                } else {
                    toastr.warning("不知名錯誤!1");
                    this.updateDateTime.errorMsg = '不知名錯誤!';
                }

            } else if (this.updateDateTime.dateTime[1] == null || this.updateDateTime.dateTime[1] == '') {
                this.updateDateTime.errorMsg = '日期時間不可為空。';

            } else {
                toastr.warning("不知名錯誤!2");
                this.updateDateTime.errorMsg = '不知名錯誤!';
            }
            //console.log(this.updateDateTime.errorMsg);
        }

    },
    created() {
        //初始化頁面
        this.tabIndex = 0;
        this.onStatusPage();
        //this.StatusPage();
    },
    mounted() {

    },
    methods: {
        //設定頁面預設狀態
        SetPageDefault() {
            this.items = [];
            this.totalRows = 1;
            this.currentPage = 1;
            this.perPage = 5;
            this.sortBy = '';
            this.sortDesc = false;
            this.filter = null;
        },
        //切換啟用代碼頁
        onStatusPage() {
            // console.log('onStatuspage');
            this.SetPageDefault();
            this.getPromoCode(this.urllist.ActivatePromoCode, this.isonStatusBusy);
        },
        //切換關閉代碼頁
        offStatusPage() {
            // console.log('offStatuspage');
            this.SetPageDefault();
            this.getPromoCode(this.urllist.NotActivatedPromoCode, this.isoffStatusBusy);
        },
        //取得優惠代碼清單
        getPromoCode(uri, busyobj) {
            busyobj.PageBusy = true;
            $.ajax({
                url: uri,
                type: "GET",
                success: function (returnData) {
                    thevue.items = returnData.result;
                    //console.log(thevue.items);
                },
                error: function (jqXHR, errorMsg) {
                    toastr.warning(errorMsg);
                },
                complete: function () {
                    busyobj.PageBusy = false;
                }
            });

        },
        //更新代碼狀態
        UpdatePromoCodeStatus(uri, data) {
            let successMsg = data.Status ? '已成功將代碼啟用' : '已成功將代碼關閉';
            let errorMsg = data.Status ? '代碼啟用請求失敗' : '代碼關閉請求失敗';
            //console.log(data, data.ID, data.Status);
            $.ajax({
                url: uri,
                type: "PUT",
                contentType: 'application/json',
                data: JSON.stringify({
                    ID: data.ID,
                    Status: data.Status
                }),
                success: function (returnData) {
                    //console.log(returnData);
                    let index = thevue.items.findIndex(x => x.promoCodeId === data.ID)
                    if (index >= 0) {
                        thevue.$bvToast.toast(successMsg, {
                            title: `代碼狀態更改操作成功`,
                            variant: "success",
                            autoHideDelay: 1200,
                            appendToast: true
                        });
                        thevue.items.splice(index, 1);
                    }
                },
                error: function (jqXHR, errorMsg) {
                    thevue.$bvToast.toast(errorMsg, {
                        title: `代碼狀態更改操作失敗`,
                        variant: "danger",
                        autoHideDelay: 1200,
                        appendToast: true
                    });
                },
                complete: function () {

                }
            });
        },
        onFiltered(filteredItems) {
            // Trigger pagination to update the number of buttons/pages due to filtering
            this.totalRows = filteredItems.length
            this.currentPage = 1
        },
        //顯示啟用與關閉確認視窗
        ShowUpdateStatusConfirm(promoCodeId, cfg) {
            this.$bvModal.msgBoxConfirm(cfg.message, {
                title: '操作確認',
                size: 'md',
                buttonSize: 'md',
                okVariant: 'warning ',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: true,
                centered: true,
                noCloseOnEsc: true,
                noCloseOnBackdrop: true
            })
                .then(value => {
                    if (value) {
                        let data = {
                            ID: promoCodeId,
                            Status: cfg.data.Status,
                        }
                        //console.log(data, data.ID, data.Status);
                        this.UpdatePromoCodeStatus(this.urllist.UpdatePromoCodeStatus, data)
                    }
                })
                .catch(err => {
                    // An error occurred
                })
        },

        //以下為新增相關方法
        checkFormValidity() {
            const valid = Object.values(this.dataState).some(x => x.status == false)
            //console.log(valid);
            return valid
        },
        resetModal() {
            this.newData = {
                promoCodeId: null,
                promoCodeType: 0,
                promoCodeMoney: 0,
                startTime: null,
                endTime: null,
                status: false
            };
            this.activityDateTime = [Date.now(), Date.now()];
            promoCodeMoneyInput = 0;
        },
        createOk(bvModalEvt) {
            // Prevent modal from closing
            bvModalEvt.preventDefault()
            // Trigger submit handler
            this.createSubmit()
        },
        createSubmit() {
            // Exit when the form isn't valid
            if (this.checkFormValidity()) {
                toastr.warning("尚有無效的輸入欄位!");
                return
            }
            let formdata = new FormData();
            Object.keys(this.newData).forEach(key => {
                // console.log(key);
                // console.log(this.inputData[key]);
                let value;
                value = this.newData[key];
                //console.log(value);

                formdata.append(key, value);
            });

            let cfg = {
                method: 'post',
                headers: { 'Content-type': 'multipart/form-data' },
                data: formdata,
                url: this.urllist.CreatePromoCode
            };
            let successMsg = '已成功新增代碼';
            let errorMsg = '新增代碼請求失敗';

            axios(cfg)
                .then(res => {
                    //console.log(res.status);
                    if (res.status == 200) {
                        this.$bvToast.toast(successMsg, {
                            title: `新增代碼操作成功`,
                            variant: "success",
                            autoHideDelay: 3000,
                            appendToast: true
                        });
                        //this.items.splice(index, 1);
                        this.onStatusPage();
                        // Hide the modal manually
                        this.$nextTick(() => {
                            this.$bvModal.hide('modal-create')
                        })
                    } else {
                        console.error(res);
                        this.$bvToast.toast(errorMsg, {
                            title: `新增代碼操作失敗`,
                            variant: "danger",
                            autoHideDelay: 3000,
                            appendToast: true
                        });
                    }
                })
                .catch(err => {
                    console.error(err);
                    this.$bvToast.toast(errorMsg, {
                        title: `新增代碼操作失敗`,
                        variant: "danger",
                        autoHideDelay: 1200,
                        appendToast: true
                    });
                })
                .finally(() => {
                });


        },
        info(item, index, button) {
            this.updateData.promoCodeId = item.promoCodeId;
            this.updateData.promoCodeType = item.promoCodeType;
            this.updateData.promoCodeMoney = item.promoCodeMoney;
            this.updateData.status = item.status;
            this.updateData.startTime = item.startTime;
            this.updateData.endTime = item.endTime;

            this.updateDateTime.dateTime = [this.updateData.startTime, this.updateData.endTime];
            this.updateDateTime.endDateTime = this.updateData.endTime;

            this.$root.$emit('bv::show::modal', 'modal-update', button);

        },
        startUpdateModal() {

        },
        updateOk(bvModalEvt) {
            // Prevent modal from closing
            bvModalEvt.preventDefault()
            // Trigger submit handler
            this.updateSubmit()
        },
        updateSubmit() {
            // Exit when the form isn't valid
            if (!this.updateDateTime.status) {
                toastr.warning("尚有無效的輸入欄位!");
                return
            }
            let thedata = JSON.stringify({
                PromoCodeId: this.updateData.promoCodeId,
                EndTime: DateFormat(this.updateData.endTime, 'YYYY/MM/DD HH:mm:ss').toString(),
                Status: this.updateData.status
            })
            //console.log(thedata);
            let cfg = {
                method: 'put',
                headers: { 'Content-type': 'application/json' },
                data: thedata,
                url: this.urllist.UpdatePromoCode
            };
            let successMsg = '已成功編輯代碼';
            let errorMsg = '編輯代碼請求失敗';

            axios(cfg)
                .then(res => {
                    //console.log(res.status);
                    if (res.status == 200) {
                        this.$bvToast.toast(successMsg, {
                            title: `編輯代碼操作成功`,
                            variant: "success",
                            autoHideDelay: 3000,
                            appendToast: true
                        });
                        //this.items.splice(index, 1);
                        this.offStatusPage();
                        // Hide the modal manually
                        this.$nextTick(() => {
                            this.$bvModal.hide('modal-update')
                        })
                    } else {
                        console.error(res);
                        this.$bvToast.toast(errorMsg, {
                            title: `編輯代碼操作失敗`,
                            variant: "danger",
                            autoHideDelay: 3000,
                            appendToast: true
                        });
                    }
                })
                .catch(err => {
                    console.error(err);
                    this.$bvToast.toast(errorMsg, {
                        title: `編輯代碼操作失敗`,
                        variant: "danger",
                        autoHideDelay: 1200,
                        appendToast: true
                    });
                })
                .finally(() => {
                });
        }
    }
});