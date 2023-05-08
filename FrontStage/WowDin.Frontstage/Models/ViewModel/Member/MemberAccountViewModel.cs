using System;
using System.ComponentModel.DataAnnotations;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class MemberAccountViewModel
    {
        public int UserAccountId { get; set; }
        public int LoginType { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "最少需2個字元")]
        public string RealName { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "最少需1個字元")]
        public string NickName { get; set; }
        public string Photo { get; set; }
        public int Sex { get; set; }
        public string Birthday { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "請填入台灣手機號碼，格式為09開頭共10碼")]
        public string Phone { get; set; }
        public string City { get; set; }
        public string Distrinct { get; set; }
        public int Point { get; set; }
        public int CouponAmount { get; set; }
    }
}
