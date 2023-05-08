using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.ViewModel.Menu;

namespace WowDin.Backstage.Services.Interface
{
    public interface IMenuService
    {
        public APIResult GetShopsOfBrand(int brandId);
        public APIResult GetMenuData(int shopId);
        public APIResult CopyMenu(CopyMenuInputVM request);
        public APIResult CopyCustom(CopyCustomInputVM request);
        public DateTime GetRecord(int shopId);
        public APIResult SaveArrangement(ArrangementInputVM request);
        public APIResult CreateClass(ClassInputVM request);
        public APIResult EditClass(ClassInputVM request);
        public APIResult DeleteClass(ClassInputVM request);
        public APIResult CreateProduct(ProductInputVM request);
        public APIResult EditProduct(ProductInputVM request);       
        public APIResult TempDeleteProduct(ProductDeleteInputVM request);
        public APIResult DeleteProduct(ProductDeleteInputVM request);
        //public Task<APIResult> UploadFigure(ProductFigureInputVM request);
    }
}
