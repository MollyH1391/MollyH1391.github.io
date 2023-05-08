using System;
using System.Collections.Generic;

namespace WowDin.Frontstage.Models.ViewModel.Store
{
    public class StoreShopMenuViewModel
    {

        public int LeaderId {get; set;}
        public bool IsOpen { get; set; }
        public string Span { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }
        public string MapData { get; set; }
        public double Star { get; set; }
        public int StarAmount { get; set; }
        //public List<string> OpenDays { get; set; }
        public string OpenTimeSpan { get; set; }
        public string Phone { get; set; }
        public string FullAddress { get; set; }
        public string PayMethod { get; set; }
        public string PriceLimit { get; set; }
        public bool PreOrder { get; set; }
        public bool HasSticker { get; set; }
        public IEnumerable<FigureVM> ShopFigures { get; set; }
        public string FirstColor { get; set; }
        public string SecondColor { get; set; }
        public IEnumerable<PromotionVM> Promotions { get; set; }
        public IEnumerable<MethodVM> TakeOuts { get; set; }
        public IEnumerable<MethodVM> Deliveries { get; set; }
        public ProductDetailModal ProductDetailModal { get; set; } 
        //public List<MenuClassVM> MenuClasses { get; set; }
        //public string MenuClassJson { get; set; }
        public ProductInCart ProductInCart { get; set; }
        
    }

    public class ProductInCart
    {
        public int ShopId { get; set; }
        public int ProductsAmount { get; set; }
        public string TotalPrice { get; set; }
        public bool IsLeader { get; set; }
        public string MsgForGroup { get; set; }

    }
    public class ProductDetailModal
    {
        public bool HasSticker { get; set; }
        public string BtnText { get; set; }
        public bool BtnEnable { get; set; }
    }
    public class MethodVM
    {
        public string Condition { get; set; }
        public string Result { get; set; }
    }
    public class PromotionVM
    {
        //直接拿到時間範圍內的活動 -> service
        public string Name { get; set;}
        public string Description { get; set; }
    }

    public class FigureVM
    {
        public string Path { get; set; }
        public string Alt { get; set; }
    }

    public class MenuClassVM
    {
        public string Name { get; set;}
        public IEnumerable<ProductVM> Products { get; set; }
    }

    public class ProductVM
    {
        public string Name { get; set;}
        public int ProductId { get; set; }
        public string Figure { get; set; }
        public decimal BasicPrice { get; set; }
        public bool SellOut { get; set; }
        public IEnumerable<CustomVM> Customs { get; set; }
    }

    public class CustomVM
    {
        public string Name { get; set;}
        public int? MaxAmount { get; set; }
        public bool Necessary { get; set; }
        public IEnumerable<SelectionVM> Selections { get; set; }
    }

    public class SelectionVM
    {
        public string Name { get; set;}
        public decimal AddPrice { get; set; }
    }

}
