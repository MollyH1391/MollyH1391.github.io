using System.ComponentModel;

namespace WowDin.Frontstage.Common.ModelEnum
{
    public class UserAccountEnum
    {
        public enum LoginTypeEnum
        {
            Email = 0,
            Facebook = 1,
            Google = 2,
            Line = 3
        }
        public enum SexEnum
        {
            [Description("男")]
            Male = 0,

            [Description("女")]
            Female = 1,

            [Description("不提供")]
            NoProvide = 2
        }
    }
}
