using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class PointHistory
    {
        public int PointHistoryId { get; set; }
        public int UserAccountId { get; set; }
        public int UserCardId { get; set; }
        public string ConsumeType { get; set; }
        public int PointAmount { get; set; }
        public int? OrderId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPlus { get; set; }

        public virtual Order Order { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserCard UserCard { get; set; }
    }
}
