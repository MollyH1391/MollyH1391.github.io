using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class CouponContainerEnum
    {
        public enum CouponState
        {
            [Description("全部")]
            All = 1,
            [Description("可使用")]
            UseCoupon = 2,
            [Description("轉贈紀錄")]
            Transfer = 3,
            [Description("歷史紀錄")]
            Historical = 4
        }
    }
}
