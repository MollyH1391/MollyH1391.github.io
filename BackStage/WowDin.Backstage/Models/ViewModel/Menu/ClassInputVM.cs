using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Menu
{
    public class ClassInputVM
    {
        public int ShopId { get; set; }
        public int MenuClassId { get; set; }
        public string ClassName { get; set; }
    }

    public class ProductInputVM
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public int MenuClassId { get; set; }
        public string State { get; set; }
        public string Figure { get; set; }
        public decimal BasicPrice { get; set; }
        public string ChangeNote { get; set; }
        public IEnumerable<CustomVM> Customs { get; set; }
        public IEnumerable<string> DeletedCustoms { get; set; }
    }

    public class ProductDeleteInputVM
    {
        public int ShopId { get; set; }
        public int Id { get; set; }
    }

    //public class CustomInputVM
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public int? MaxAmount { get; set; }
    //    public bool Necessary { get; set; }
    //    public string ProductId { get; set; }
    //    public IEnumerable<SelectionInputVM> Selections { get; set; }
    //    public IEnumerable<string> DeletedSelections { get; set; }
    //}

    //public class SelectionInputVM
    //{
    //    public string Id { get; set; }
    //    public string CustomId { get; set; }
    //    public string Name { get; set; }
    //    public decimal Price { get; set; }
    //}
}
