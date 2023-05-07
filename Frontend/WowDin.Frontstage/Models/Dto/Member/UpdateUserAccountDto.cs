using System;

namespace WowDin.Frontstage.Models.Dto.Member
{
    public class UpdateUserAccountDto
    {
        public int UserAccountId { get; set; }
        public int LoginType { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Photo { get; set; }
        public int Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Distrinct { get; set; }
    }
}
