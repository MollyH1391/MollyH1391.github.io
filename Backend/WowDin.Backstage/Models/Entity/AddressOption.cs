using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class AddressOption
    {
        public int AddressOptionId { get; set; }
        public int UserAccountId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
