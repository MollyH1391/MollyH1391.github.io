using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Shop
    {
        public Shop()
        {
            Carts = new HashSet<Cart>();
            Comments = new HashSet<Comment>();
            Coupons = new HashSet<Coupon>();
            CustomSets = new HashSet<CustomSet>();
            Favorites = new HashSet<Favorite>();
            MenuClasses = new HashSet<MenuClass>();
            MenuHistories = new HashSet<MenuHistory>();
            Orders = new HashSet<Order>();
            Responses = new HashSet<Response>();
            ShopHistories = new HashSet<ShopHistory>();
            ShopMethods = new HashSet<ShopMethod>();
            ShopPaymentTypes = new HashSet<ShopPaymentType>();
        }

        public int ShopId { get; set; }
        public int BrandId { get; set; }
        public int VerificationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OpenDayList { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal? PriceLimit { get; set; }
        public bool PreOrder { get; set; }
        public int State { get; set; }
        public bool Verified { get; set; }
        public bool HasSticker { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Star { get; set; }
        public int StarAmount { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Verification Verification { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual ICollection<CustomSet> CustomSets { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<MenuClass> MenuClasses { get; set; }
        public virtual ICollection<MenuHistory> MenuHistories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<ShopHistory> ShopHistories { get; set; }
        public virtual ICollection<ShopMethod> ShopMethods { get; set; }
        public virtual ICollection<ShopPaymentType> ShopPaymentTypes { get; set; }
    }
}
