using System.ComponentModel;

namespace WowDin.Frontstage.Common.ModelEnum
{
    public class ShopMethodEnum
    {
        public enum TakeMethodEnum
        {
            [Description("自取")]
            TakeOut=0,
            [Description("外送")]
            Delivery=1,
            [Description("訂位")]
            Booking=2
        }
    }
}
