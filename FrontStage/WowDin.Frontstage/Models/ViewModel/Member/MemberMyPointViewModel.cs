using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.Entity;

namespace WowDin.Frontstage.Models.ViewModel.Member
{
    public class MemberMyPointViewModel
    {
        public int PointHistoryId { get; set; }
        public int UserAccountId { get; set; }
        public int UserCardId { get; set; }
        public string ConsumeType { get; set; }
        public int PointAmount { get; set; }
        public int? OrderId { get; set; }
        public DateTime Date { get; set; }

        public virtual Models.Entity.Order Order { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserCard UserCard { get; set; }
        public bool IsPlus { get; set; }
    }
}
