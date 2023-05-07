using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.ViewModel.Information
{
    public class ShopBusinessViewModel
    {
        public string UpdateTime { get; set; }
        public ShopEnum.StateEnum State { get; set; }
        public List<string> OpenDayList { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }

    }
}
