using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Verification
    {
        public Verification()
        {
            Brands = new HashSet<Brand>();
            Shops = new HashSet<Shop>();
            UserAccounts = new HashSet<UserAccount>();
        }

        public int AccountType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int VerificationId { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
