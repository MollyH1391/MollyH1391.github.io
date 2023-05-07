using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class ProductEnum
    {
        public enum StateEnum
        {
            [Description("上架")]
            Enable = 1,
            [Description("下架")]
            Disable = 2,
            [Description("刪除")]
            Deleted = 3,
            [Description("未定義")]
            Undefined = 0
        }
    }
}
