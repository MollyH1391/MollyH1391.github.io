using System;
using System.Collections.Generic;
using static WowDin.Frontstage.Common.ModelEnum.OrderEnum;

namespace WowDin.Frontstage.Models.Dto.OrderDetails
{
    public class CreateCommentInputDto
    {
        public int OrderId { get; set; }
        public string Comment1 { get; set; }
        public int Star { get; set; }
       
    }
}
