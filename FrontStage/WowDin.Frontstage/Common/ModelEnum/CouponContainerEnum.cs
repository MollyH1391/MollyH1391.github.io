using System.ComponentModel;

namespace WowDin.Frontstage.Common.ModelEnum
{
    public class CouponContainerEnum
    {
        public enum CouponState
        {
            [Description("全部")]
            All = 1,
            [Description("可使用")]
            Usable = 2,
            [Description("已使用")]
            Used = 3,
            [Description("已過期")]
            Expired = 4
        }
    }
}
