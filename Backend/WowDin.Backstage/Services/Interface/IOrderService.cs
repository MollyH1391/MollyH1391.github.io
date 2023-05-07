using System.Collections.Generic;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Order;
using WowDin.Backstage.Models.ViewModel.Order;

namespace WowDin.Backstage.Services.Interface
{
    public interface IOrderService
    {
        public IEnumerable<GetShopsByBrandVM> GetShopsByBrand(int brandId);
        public IEnumerable<GetAllOrderDetailsByShopVM> GetAllOrderDetailsByShop(int shopId);
        public IEnumerable<GetAllOrderDetailsByBrandVM> GetAllOrderDetailsByBrand(int brandId);
        public void UpdateOrderState_Accept(GetAllOrderDetailsByShopVM query);
        public APIResult UpdateOrderState_AcceptALL(IEnumerable<GetAllOrderDetailsByShopVM> query);
        public APIResult UpdateOrderState_CancelALL(IEnumerable<GetAllOrderDetailsByShopVM> query);
        public APIResult UpdateOrderState_CompleteALL(IEnumerable<GetAllOrderDetailsByShopVM> query);
        public void UpdateOrderState_Cancel(GetAllOrderDetailsByShopVM query);
        public void UpdateOrderState_Complete(GetAllOrderDetailsByShopVM query);
    }
}
