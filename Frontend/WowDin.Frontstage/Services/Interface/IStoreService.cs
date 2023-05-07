using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.Dto.PartialView;
using WowDin.Frontstage.Models.Dto.Store;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Models.ViewModel.Store;
using WowDin.Frontstage.Repositories.Interface;
using static WowDin.Frontstage.Common.ModelEnum.ShopMethodEnum;

namespace WowDin.Frontstage.Services.Interface
{
    public interface IStoreService
    {
        //public StoreShopMenuViewModel GetShopMenuBasicVM(int shopId);
        public ShopMenuDto GetShopMenu(int shopId);
        public IEnumerable<ProductClassDto> GetMenuDetail(int shopId);
        public string MsgForGroupMember(int leaderId);
        public bool ShopMenuExist(int shopId);
        public bool IsOpen(int shopId);
        public bool IsShopAvaliable(int shopId);
        public GetAllBrandDto GetAllBrand();
        public GetBrandDataDto GetBrandDataById(int id);
        public bool BrandExist(int brandId);
        public bool ShopExist(int shopId);


        public IEnumerable<ShopCardDto> GetNearbyShopCards(SearchRequestDto request);
        public IEnumerable<ShopCardDto> GetShopCards(SearchRequestDto request);
        public SearchDto InitSearchPage();
        public string UpdateFavorite(string method,int shopId);
        public List<string> GetBrandItem(string input);
        public DeliveryFeeDto GetDevliveryFee(int shopId, string address, decimal amount);
    }
}
