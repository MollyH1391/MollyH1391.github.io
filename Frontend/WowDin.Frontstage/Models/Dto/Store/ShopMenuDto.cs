using System;
using System.Collections.Generic;
using WowDin.Frontstage.Common.ModelEnum;
using WowDin.Frontstage.Models.Dto.Order;
using static WowDin.Frontstage.Common.ModelEnum.ShopMethodEnum;
using static WowDin.Frontstage.Common.ModelEnum.ShopPaymentTypeEnum;

namespace WowDin.Frontstage.Models.Dto.Store
{
    public class ShopMenuDto
    {
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public Map MapData { get; set; }
        public double Star { get; set; }
        public double StarAmount { get; set; }
        //public IEnumerable<string> OpenDays { get; set; }
        public string OpenTimeSpan { get; set; }
        public string Phone { get; set; }
        public string FullAddress { get; set; }
        public List<string> PayMethod { get; set; }
        public string PriceLimit { get; set; }
        public bool PreOrder { get; set; }
        public bool HasSticker { get; set; }
        public List<FigureDto> ShopFigures { get; set; }
        public string FirstColor { get; set; }
        public string SecondColor { get; set; }
        public List<PromotionDto> Promotions { get; set; }
        public List<MethodDto> TakeOuts { get; set; }
        public List<MethodDto> Deliveries { get; set; }
        //public IEnumerable<ProductClassDto> MenuClasses { get; set; }
    }

    public class Map
    {
        public string FullAddress { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
    public class MethodDto
    {
        public string Condition { get; set; }
        public string Result { get; set; }
    }
    public class PromotionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class FigureDto
    {
        public string Path { get; set; }
        public string Alt { get; set; }
    }
    public class ProductClassDto
    {
        public string Name { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string Figure { get; set; }
        public decimal BasicPrice { get; set; }
        public bool SellOut { get; set; }
        public IEnumerable<CustomDto> Customs { get; set; }
    }

    public class CustomDto
    {
        public string Name { get; set; }
        public int? MaxAmount { get; set; }
        public bool Necessary { get; set; }
        public IEnumerable<SelectionDto> Selections { get; set; }
    }

    public class SelectionDto
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public decimal AddPrice { get; set; }
    }
}
