using System.Collections.Generic;

namespace WowDin.Backstage.Models.Dto.Menu
{
    public class GetMenuDataDto
    {
        public IEnumerable<MenuClassDto> MenuClasses { get; set; }
    }

    public class MenuClassDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string State { get; set; }
        public string Figure { get; set; }
        public decimal BasicPrice { get; set; }
        public string ChangeNote { get; set; }
        public IEnumerable<CustomDto> Customs { get; set; }
        public IEnumerable<string> DeletedCustoms { get; set; }

    }

    public class CustomDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool Necessary { get; set; }
        public int? MaxAmount { get; set; }
        public IEnumerable<SelectionDto> Selections { get; set; }
        public IEnumerable<string> DeletedSelections { get; set; }

    }

    public class SelectionDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
