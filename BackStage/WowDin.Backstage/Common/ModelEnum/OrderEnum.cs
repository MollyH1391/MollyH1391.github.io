using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class OrderEnum
    {
        public enum OrderStateEnum
        {
            [Description("未接單")]
            OrderEstablished = 0, //未接單

            [Description("已接單")]
            OrderAccepted = 1, //店家已接單

            [Description("準備中")]
            Product_Processing = 2, //商品準備中

            [Description("揪團中")]
            GroupOrder_Processing = 3, //團長揪團中

            [Description("揪團成功")]
            GroupOrder_Success = 4, //揪團成功、跟團成功(若判斷是團長才顯示揪團成功)

            [Description("揪團失敗")]
            GroupOrder_Fail = 5, //揪團失敗

            [Description("準備運送")]
            Product_ReadyToDeliver = 6,//商品準備運送

            [Description("完成")]
            OrderComplete = 7, //完成

            [Description("訂單取消")]
            OrderRejected_Cancelled = 8, //訂單取消(未付款)

            [Description("門市取消訂單")]
            OrderRejected_CancelledByShop = 9,//門市取消訂單(如果判斷已付款就要換到OrderRejected_Payback，未付款就換到OrderRejected_Cancelled)

            [Description("訂單取消")]
            OrderRejected_Payback = 10 //訂單取消(需要退款)
        }

        public enum TakeMethodEnum
        {

            [Description("外送")]
            Delivery = 0,

            [Description("自取")]
            SelfPiclUp = 1
        }

        public enum PaymentTypeEnum
        {
            [Description("現金")]
            Cash = 0,

            [Description("信用卡")]
            VisaCard = 1,

            [Description("信用卡")]
            MasterCard = 2,

            [Description("Line Pay")]
            LinePay = 3,

            [Description("街口支付")]
            JkoPay = 4,

            [Description("找團長付款")]
            PayToGroup = 5 //找團長付款
        }

        public enum PaymentStateEnum
        {
            [Description("付款成功")]
            Success = 0,

            [Description("付款失敗")]
            Fail = 1,

            [Description("待付款")]
            Pending = 2, //待付款

            [Description("進行中")]
            Processing = 3, //進行中

            [Description("退款中")]
            Processing_Payback = 4, //退款中

            [Description("退款完成")]
            Success_complete = 5 //退款完成
        }
        public enum ReceiptTypeEnum
        {
            [Description("紙本發票")]
            Receipt = 0, //紙本發票

            [Description("載具")]
            Receipt_Barcode = 1, //載具

            [Description("統一邊號")]
            Receipt_VATnumber = 2 //統編
        }
        public enum CommentEnum
        {
            [Description("服務態度良好")]
            ServiceQuality = 0,

            [Description("商品品質很棒")]
            ProductQuality = 1,

            [Description("外送很準時")]
            DeliveryOntime = 2,

            [Description("其他")]
            Else = 3
        }
    }
}
