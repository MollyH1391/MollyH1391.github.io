﻿using System.Collections.Generic;
using WowDin.Frontstage.Models.ViewModel.Member;

namespace WowDin.Frontstage.Models.Dto.Activity
{
    public class CouponResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class CouponContentDto
    {
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeSpan  { get; set; }
        public int RestTime { get; set; }
        public IEnumerable<string> Shops { get; set; }
    }
}
