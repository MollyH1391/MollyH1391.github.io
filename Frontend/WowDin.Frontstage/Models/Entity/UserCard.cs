using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class UserCard
    {
        public UserCard()
        {
            PointHistories = new HashSet<PointHistory>();
        }

        public int UserCardId { get; set; }
        public int UserAccountId { get; set; }
        public int BrandId { get; set; }
        public int Points { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<PointHistory> PointHistories { get; set; }
    }
}
