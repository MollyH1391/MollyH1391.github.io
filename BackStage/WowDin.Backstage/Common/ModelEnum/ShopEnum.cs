using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class ShopEnum
    {
        public enum StateEnum
        {
            [Description("營業中")]
            Open=0,
            [Description("休息中")]
            Rest=1,
            [Description("歇業")]
            Close=2,
            [Description("自動")]
            Auto=3,
            [Description("移除")]
            Remove=4,
        }

        public enum DayListEnum
        {
            [Description("星期一")]
            Monday = 0,
            [Description("星期二")]
            Tuesday = 1,
            [Description("星期三")]
            Wednesday = 2,
            [Description("星期四")]
            Thursday = 3,
            [Description("星期五")]
            Friday = 4,
            [Description("星期六")]
            Saturday = 5,
            [Description("星期日")]
            Sunday = 6,
        }
    }
}
