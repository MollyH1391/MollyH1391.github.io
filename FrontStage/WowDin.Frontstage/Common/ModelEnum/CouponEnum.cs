using System.ComponentModel;

namespace WowDin.Frontstage.Common.ModelEnum
{
    public class CouponEnum
    {
        public enum CouponType
        {
            [Description("單品折價券")]
            ForProduct = 1,
            [Description("優惠券")]
            Storewide = 2

        }

        public enum CouponStatus
        {
            [Description("開放兌換")]
            Available = 1,
            [Description("停止兌換")]
            Unavailable = 2
        }

    }



}
