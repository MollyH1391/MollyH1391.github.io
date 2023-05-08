using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class ShopPaymentTypeEnum
    {
        public enum PaymentTypeEnum
        {
            [Description("現金")]
            Cash = 0,
            [Description("信用卡")]
            CreditCard = 1,
            [Description("Line Pay")]
            LinePay = 2,
            [Description("街口支付")]
            JKOPay = 3
        }
    }
}
