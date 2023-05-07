using System.ComponentModel;

namespace WowDin.Backstage.Common.ModelEnum
{
    public class VerificationEnum
    {
        public enum AccountTypeEnum
        {
            [Description("消費者")]
            User = 0,

            [Description("商店管理者")]
            ShopAdmin = 1,

            [Description("品牌管理者")]
            BrandAdmin = 2,

            [Description("平台管理者")]
            Admin = 3
        }
    }
}
