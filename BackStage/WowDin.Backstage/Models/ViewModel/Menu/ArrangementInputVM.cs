using System.Collections.Generic;

namespace WowDin.Backstage.Models.ViewModel.Menu
{
    public class ArrangementInputVM
    {
        public int ShopId { get; set; }
        public List<ClassArrangementVM> MenuClasses { get; set; }
    }

    public class ClassArrangementVM
    {
        public int Id { get; set; }
        public List<ProductArrangementVM> Products { get; set; }
    }

    public class ProductArrangementVM
    {
        public int Id { get; set; }
    }
}
