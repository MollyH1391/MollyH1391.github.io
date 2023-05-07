using System.ComponentModel;

namespace WowDin.Frontstage.Common.ModelEnum
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
            Remove = 4
        }
    }
}
