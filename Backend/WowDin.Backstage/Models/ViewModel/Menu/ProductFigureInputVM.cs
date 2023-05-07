using Microsoft.AspNetCore.Http;

namespace WowDin.Backstage.Models.ViewModel.Menu
{
    public class ProductFigureInputVM
    {
        public int ProductId { get; set; }
        public IFormFile UploadFigure { get; set; }

    }
}
