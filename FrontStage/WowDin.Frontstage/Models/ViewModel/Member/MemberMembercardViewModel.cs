using System;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class MemberMembercardViewModel
    {
        public int LoginType { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Photo { get; set; }
        public int Sex { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Distrinct { get; set; }
        public int Point { get; set; }
        public string CardImgUrl { get; set; }
        public int CouponAmount { get; set; }
        public string CardTypeName { get; set; }
        public string NextCardTypeName { get; set; }
        public int Range { get; set; }
    }
}
