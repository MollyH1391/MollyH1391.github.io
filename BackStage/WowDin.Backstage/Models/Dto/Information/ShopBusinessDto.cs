using System;
using System.Collections.Generic;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Models.Dto.Information
{
    public class ShopBusinessDto
    {
        public string UpdateTime { get; set; }
        public ShopEnum.StateEnum State { get; set; }
        public IEnumerable<string> OpenDayList { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }

    }
}
