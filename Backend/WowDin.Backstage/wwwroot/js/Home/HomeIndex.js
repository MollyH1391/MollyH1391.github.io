const totalAmountType = document.querySelector('#totalAmountType')
const totalAmountChart = document.querySelector('#totalAmountChart')
const totalQuantityType = document.querySelector('#totalQuantityType')
const totalQuantityChart = document.querySelector('#totalQuantityChart')


const index = new Vue({
    el: '#index',
    data: {
        AllData: [],
        TodayTotalQuantity: "",
        TodayTotalAmount: "",
        TodayBestProduct: "",
        YearTotalQuantity: "",
        YearTotalAmount: "",
        YearBestProduct: "",
    },
    created() {
        this.GetChartData()
    },
    methods: {
        GetChartData() {
            axios.get(`/api/GetChartDataById`)
                .then((res) => {
                    if (res.data.status == 20000) {
                        this.AllData = res.data.result
                        this.TodayTotalQuantity = this.AllData.todayOrders
                        this.TodayTotalAmount = this.AllData.todayTotalAmount
                        this.TodayBestProduct = this.AllData.quantityTodayDataList == 0 ? "尚無訂單" : this.AllData.quantityTodayDataList[0].productName
                        this.YearTotalQuantity = this.AllData.yearOrders
                        this.YearTotalAmount = this.AllData.yearTotalAmount
                        this.YearBestProduct = this.AllData.quantityMonthDataList == 0 ? "尚無訂單" : this.AllData.quantityMonthDataList[0].productName
                    }
                    else {
                        toastr.warning('資料讀取失敗')
                    }
                })
                .catch((error) => { console.log(error) })
        }
    }
})

window.onload = function () {
    getChartData()
}

let allData = []
let monthAmountData = []
let dayAmountData = []
let weekAmountData = []
let monthProductDataData = []
let monthTotalQuantityData = []
let dayProductDataData = []
let dayTotalQuantityData = []
let weekProductDataData = []
let weekTotalQuantityData = []
let dayCount = []
let allLineChartList = []
let allBarChartList = []
let dayLabels = []
let dayArray = []
let monthArray = []
let weekArray = []
let noneQuantity = [{ productName: '尚無訂單', totalQuantity: 0 }]
let amountLength = []
let quantityLength = []

function getChartData() {
    axios.get(`/api/GetChartDataById`)
        .then((res) => {
            if (res.data.status == 20000) {
                allData = res.data.result
                dayCount = allData.dayCount
                getDay(dayCount)
                monthAmountData = allData.amountMonth
                dayAmountData = allData.amountDay
                weekAmountData = allData.amountWeek
                amountLength = [ weekAmountData.length, dayAmountData.length, monthAmountData.length ]
                monthProductData = allData.quantityMonthDataList.map(x => x.productName)
                dayProductData = allData.quantityDayDataList.map(x => x.productName)
                weekProductData = allData.quantityWeekDataList.map(x => x.productName)
                monthTotalQuantityData = allData.quantityMonthDataList.map(x => x.totalQuantity)
                dayTotalQuantityData = allData.quantityDayDataList.map(x => x.totalQuantity)
                weekTotalQuantityData = allData.quantityWeekDataList.map(x => x.totalQuantity)
                quantityLength = [weekTotalQuantityData.length, dayTotalQuantityData.length, monthTotalQuantityData.length]
                getMonthData(monthAmountData)
                getDayData(dayAmountData)
                getWeekData(weekAmountData)
                getLineChartList()
                getBarChartList()
                drawLineChart(allLineChartList[0], amountLength[0])
                drawBarChart(allBarChartList[0], quantityLength[0])
                showBtnGroup(allLineChartList, allBarChartList)
            }
            else {
                console.warn('撈資料失敗')
            }
        })
        .catch((error) => { console.log(error) })
}


function getDay(dayCountData) {
    for (let i = 1; i <= dayCountData; i++) {
        dayLabels.push(i)
    }
    return dayLabels
};

function getDayData(dayAmountData) {
    for (let i = 0; i < dayAmountData.length; i++) {
        for (let j = 0; j < dayLabels.length; j++) {
            let day = dayArray.find(m => m.x == dayLabels[j])
            let dayTotalAmount = dayArray.find(m => m.y == "0")
            if (dayLabels[j] == dayAmountData[i].day && dayArray.indexOf(day) === -1) {
                daydata = { x: dayLabels[j], y: dayAmountData[i].totalPrice };
                dayArray.push(daydata)
            }
            else if (dayLabels[j] == dayAmountData[i].day && dayArray.indexOf(day) > -1 && dayArray.indexOf(dayTotalAmount) > -1) {
                dayArray.splice(j, 1, { x: dayLabels[j], y: dayAmountData[i].totalPrice })
            }
            else if (dayLabels[j] != dayAmountData[i].day && dayArray.indexOf(day) === -1) {
                daydata = { x: dayLabels[j], y: "0" };
                dayArray.push(daydata)
            }
        }
    }
    return dayArray;
}

var monthLabels = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']

function getMonthData(monthAmountData) {
    for (let i = 0; i < monthAmountData.length; i++) {
        for (let j = 0; j < monthLabels.length; j++) {
            let month = monthArray.find(m => m.x == monthLabels[j])
            let monthTotalAmount = monthArray.find(m => m.y == "0")
            //如果資料庫data == 現有月份 而且目前的陣列中沒有相同的月份
            if (monthLabels[j] == monthAmountData[i].month && monthArray.indexOf(month) === -1) { 
                yeardata = { x: monthLabels[j], y: monthAmountData[i].totalPrice };
                monthArray.push(yeardata)
            }
            //如果資料庫data == 現有月份 & 目前的陣列中有相同的月份 & 目前的陣列中有相同的金額
            else if (monthLabels[j] == monthAmountData[i].month && monthArray.indexOf(month) > -1 && monthArray.indexOf(monthTotalAmount) > -1) {
                monthArray.splice(j, 1, { x: monthLabels[j], y: monthAmountData[i].totalPrice })
            }
            //如果資料庫data != 現有月份 & 而且目前的陣列中沒有相同的月份
            else if (monthLabels[j] != monthAmountData[i].month && monthArray.indexOf(month) === -1) {
                yeardata = { x: monthLabels[j], y: "0" };
                monthArray.push(yeardata)
            }
        }
    }
    return monthArray;
}

var weekLabelsCh = ['星期一', '星期二', '星期三', '星期四', '星期五', '星期六', '星期日']
var weekLabels = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Sataurday', 'Sunday']
function getWeekData(weekAmountData) {
    for (let i = 0; i < weekAmountData.length; i++) {
        for (let j = 0; j < weekLabels.length; j++) {
            let week = weekArray.find(m => m.x == weekLabelsCh[j])
            let weekTotalAmount = weekArray.find(m => m.y == "0")
            if (weekLabels[j] == weekAmountData[i].week && weekArray.indexOf(week) === -1) {
                weekdata = { x: weekLabelsCh[j], y: weekAmountData[i].totalPrice };
                weekArray.push(weekdata)
            }
            else if (weekLabels[j] == weekAmountData[i].week && weekArray.indexOf(week) > -1 && weekArray.indexOf(weekTotalAmount) > -1) {
                weekArray.splice(j, 1, { x: weekLabelsCh[j], y: weekAmountData[i].totalPrice })
            }
            else if (weekLabels[j] != weekAmountData[i].week && weekArray.indexOf(week) === -1) {
                weekdata = { x: weekLabelsCh[j], y: "0" };
                weekArray.push(weekdata)
            }
        }
    }
    return weekArray;
}

function getLineChartList() {
    allLineChartList = [
        {
            name: '週',
            //銷售額報表
            lineChartLabels: weekLabelsCh,
            amountData: weekArray,
            lineChartTitle: '本週銷售額',
            lineChartXTitle: '星期',
        },
        {
            name: '月',
            //銷售額報表
            lineChartLabels: dayLabels,
            amountData: dayArray,
            lineChartTitle: '本月銷售額',
            lineChartXTitle: '日期',
        },
        {
            name: '年',
            //銷售額報表
            lineChartLabels: monthLabels,
            amountData: monthArray,
            lineChartTitle: '本年度銷售額',
            lineChartXTitle: '月份',
        }
    ]

    return allLineChartList
}

function getBarChartList() {
    allBarChartList = [
        {
            name: '週',

            //銷售量報表
            barChartLabels: weekProductData,
            quantityData: weekTotalQuantityData,
            barChartTitle: '本週銷量排行',
        },
        {
            name: '月',

            //銷售量報表
            barChartLabels: dayProductData,
            quantityData: dayTotalQuantityData,
            barChartTitle: '本月銷量排行',
        },
        {
            name: '年',

            //銷售量報表
            barChartLabels: monthProductData,
            quantityData: monthTotalQuantityData,
            barChartTitle: '本年度銷量排行',
        }
    ]

    return allBarChartList
}

function showBtnGroup() {
    const chartList = allBarChartList.map(x => x.name)
    chartList.forEach((item, index) => {
        const amountBtn = document.createElement('b-button')
        amountBtn.innerText = item
        amountBtn.classList.add('btn', 'btn-outline-info')

        const quantityBtn = document.createElement('b-button')
        quantityBtn.innerText = item
        quantityBtn.classList.add('btn', 'btn-outline-info')

        amountBtn.onclick = function () {
            drawLineChart(allLineChartList[index], amountLength[index])
        }

        quantityBtn.onclick = function () {
            drawBarChart(allBarChartList[index], quantityLength[index])
        }

        totalAmountType.appendChild(amountBtn)
        totalQuantityType.appendChild(quantityBtn)
    })

}

function drawLineChart(chartData, amountLength) {
    resetLineChart()

    if (amountLength == 0) {
        getNonAmount(chartData.lineChartTitle)
    }
    else {
        let canvas = document.createElement('canvas')
        canvas.setAttribute('id', 'myLineChart')
        canvas.setAttribute('width', '450')
        canvas.setAttribute('height', '250')

        totalAmountChart.appendChild(canvas)

        let ctxLine = document.getElementById("myLineChart");
        var lineChart = new Chart(ctxLine, {
            type: 'line',
            data: {
                labels: chartData.lineChartLabels, //CHANGE
                datasets: [
                    {
                        label: '銷售額',
                        data: chartData.amountData, //CHANGE
                        backgroundColor: 'rgba(255, 159, 64, 0.2)',
                        borderColor: 'rgb(255, 205, 86)',
                        pointStyle: 'circle',
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        pointBackgroundColor: ['rgba(255, 159, 64, 0.6)', 'rgba(247, 92, 47, 0.6)'],
                        pointBorderColor: ['rgb(255, 205, 86)', 'rgb(255, 205, 86)']
                    }
                ]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: chartData.lineChartTitle, //CHANGE
                        font: {
                            size: '30',
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: chartData.lineChartXTitle, //CHANGE
                            color: 'black',
                            font: {
                                weight: 800,
                                size: '18',
                            },
                        },
                        ticks: {
                            color: 'black',
                            font: {
                                size: '16',
                            }
                        }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: {
                            color: 'black',
                            font: {
                                size: '16',
                            }
                        }
                    }
                }
            },
        });
    }
}

function drawBarChart(chartData, quantityLength) {
    resetBarChart()

    if (quantityLength == 0) {
        getNonQuantity(chartData.barChartTitle)
    }
    else {
        let canvas = document.createElement('canvas')
        canvas.setAttribute('id', 'myBarChart')
        canvas.setAttribute('width', '450')
        canvas.setAttribute('height', '250')

        totalQuantityChart.appendChild(canvas)

        let ctxBar = document.getElementById("myBarChart");
        var barChart = new Chart(ctxBar, {
            type: 'bar',
            data: {
                labels: chartData.barChartLabels, //CHANGE
                datasets: [
                    {
                        label: '銷售量',
                        data: chartData.quantityData, //CHANGE
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(255, 159, 64, 0.2)',
                            'rgba(255, 205, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(201, 203, 207, 0.2)'
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)'
                        ],
                        pointStyle: 'circle',
                        pointRadius: 10,
                        pointHoverRadius: 15
                    }
                ]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: chartData.barChartTitle, //CHANGE
                        font: {
                            size: '30',
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: '商品', //CHANGE
                            color: 'black',
                            font: {
                                weight: 800,
                                size: '18',
                            },
                        },
                        ticks: {
                            color: 'black',
                            font: {
                                size: '16',
                            }
                        }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 10,
                            color: 'black',
                            font: {
                                size: '16',
                            }
                        }
                    }
                }
            }
        });
    }
}

function resetLineChart() {
    totalAmountChart.innerHTML = ''
}

function resetBarChart() {
    totalQuantityChart.innerHTML = ''
}

function getNonAmount(data) {
    let h5 = document.createElement('h5')
    h5.classList.add('nonOrderTitle', 'text-center', 'fz_30', 'mt-3', 'font-weight-bold')
    h5.innerText = `${data}`
    let img = document.createElement('img')
    img.classList.add('nonOrder', 'mt-5')
    img.src = "/images/home/無訂單.png"
    let p = document.createElement('p')
    p.classList.add('nonOrderText', 'mt-3', 'text-center', 'text-dark')
    p.innerText = "目前尚無訂單"

    totalAmountChart.appendChild(h5)
    totalAmountChart.appendChild(img)
    totalAmountChart.appendChild(p)
}

function getNonQuantity(data) {
    let h5 = document.createElement('h5')
    h5.classList.add('nonOrderTitle', 'text-center', 'fz_30', 'mt-3', 'font-weight-bold')
    h5.innerText = `${data}`
    let img = document.createElement('img')
    img.classList.add('nonOrder', 'mt-5')
    img.src = "/images/home/無訂單.png"
    let p = document.createElement('p')
    p.classList.add('nonOrderText', 'mt-3', 'text-center', 'text-dark')
    p.innerText = "目前尚無訂單"

    totalQuantityChart.appendChild(h5)
    totalQuantityChart.appendChild(img)
    totalQuantityChart.appendChild(p)
}