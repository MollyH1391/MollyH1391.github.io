using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class AdvertiseEnum
    {
        public enum StatusEnum
        {
            [Description("申請中")]
            Pending = 0,
            [Description("刊登中")]
            Published = 1,
            [Description("已下架")]
            Removed = 2,
            [Description("拒絕申請")]
            Rejected = 3,
        }
    }
}
