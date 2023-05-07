using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Frontstage.Models.Entity
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            AddressOptions = new HashSet<AddressOption>();
            Carts = new HashSet<Cart>();
            CouponContainers = new HashSet<CouponContainer>();
            Favorites = new HashSet<Favorite>();
            OrderDetails = new HashSet<OrderDetail>();
            Orders = new HashSet<Order>();
            PointHistories = new HashSet<PointHistory>();
            Responses = new HashSet<Response>();
            UserCards = new HashSet<UserCard>();
        }

        public int LoginType { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Photo { get; set; }
        public int? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Distrinct { get; set; }
        public bool Verified { get; set; }
        public int VerificationId { get; set; }
        public string UserStamp { get; set; }
        public int UserAccountId { get; set; }

        public virtual Verification Verification { get; set; }
        public virtual ICollection<AddressOption> AddressOptions { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<CouponContainer> CouponContainers { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PointHistory> PointHistories { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<UserCard> UserCards { get; set; }
    }
}
