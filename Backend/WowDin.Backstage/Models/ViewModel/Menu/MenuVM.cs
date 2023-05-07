using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Menu
{
    public class MenuVM
    {
        public IEnumerable<MenuClassVM> MenuClasses { get; set; }
        public string UpdateTime { get; set; }
    }

    public class MenuClassVM
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
    }

    public class ProductVM
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string State { get; set; }
        public string Figure { get; set; }
        public IFormFile UploadFigure { get; set; }
        public decimal BasicPrice { get; set; }
        public string ChangeNote { get; set; }

        public IEnumerable<CustomVM> Customs { get; set; }
        public IEnumerable<string> DeletedCustoms { get; set; }
    }

    public class CustomVM
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Necessary { get; set; }
        public int? MaxAmount { get; set; }
        public IEnumerable<SelectionVM> Selections { get; set; }
        public IEnumerable<string> DeletedSelections { get; set; }
    }

    public class SelectionVM
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public decimal Price { get; set; }
    }
}
