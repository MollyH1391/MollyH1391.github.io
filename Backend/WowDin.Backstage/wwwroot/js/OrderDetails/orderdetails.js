const app = new Vue({
    el: '#app',
    data: {
        productList: [],
        title: "test",
        fields: [
            { key: 'OrderStamp', label: '訂單編號' },
            { key: 'OrderDate', label: '訂單建立時間' },
            { key: 'UserName', label: '湯湯' },
            { key: 'PickUpTime', label: '取餐時間' },
            { key: 'TakeMethod', label: '取餐方式' },
            { key: 'ReceiptType', label: '發票方式' },
            { key: 'TotalPrice', label: '總價' },
            { key: 'Action', label: '詳細' }
        ],
        items: [
            {
                OrderId: 1,
                UserName: '湯湯',
                OrderStamp: 'M123456789',
                OrderDate: '2022-03-06',
                PickUpTime: '2022-03-06 12:00',
                TakeMethod: '外送',
                Message: '放門口',
                ReceiptType: '紙本發票',
                Address: '台北市信義區',
                PaymentType: '現金',
                TotalPrice: `$ ${165}元`
            }
        ],
        products: [
            {
                ProductName: '１四季春珍波椰-大',
                Quantity: 2,
                UnitPrice: 45,
                Note: '微糖/微冰',
            },
            {
                ProductName: '３可可芭蕾混珠-大',
                Quantity: 1,
                UnitPrice: 75,
                Note: '微糖/微冰',
            }

        ]
    },
})