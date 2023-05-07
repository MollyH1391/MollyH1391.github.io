using System;
using System.Collections.Generic;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class Brand
    {
        public Brand()
        {
            BrandFigures = new HashSet<BrandFigure>();
            BrandHistories = new HashSet<BrandHistory>();
            CardTypes = new HashSet<CardType>();
            CatagorySets = new HashSet<CatagorySet>();
            Responses = new HashSet<Response>();
            ShopFigures = new HashSet<ShopFigure>();
            Shops = new HashSet<Shop>();
            UserCards = new HashSet<UserCard>();
            Websites = new HashSet<Website>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int VerificationId { get; set; }
        public string Slogen { get; set; }
        public string Video { get; set; }
        public string Description { get; set; }
        public string FirstColor { get; set; }
        public string SecondColor { get; set; }
        public int PayLevel { get; set; }
        public string CardImgUrl { get; set; }
        public int Star { get; set; }
        public int StarAmount { get; set; }
        public bool Suspension { get; set; }
        public string Message { get; set; }
        public int Verified { get; set; }

        public virtual Verification Verification { get; set; }
        public virtual ICollection<BrandFigure> BrandFigures { get; set; }
        public virtual ICollection<BrandHistory> BrandHistories { get; set; }
        public virtual ICollection<CardType> CardTypes { get; set; }
        public virtual ICollection<CatagorySet> CatagorySets { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<ShopFigure> ShopFigures { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<UserCard> UserCards { get; set; }
        public virtual ICollection<Website> Websites { get; set; }
    }
}
