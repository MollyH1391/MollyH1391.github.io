using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WowDin.Backstage.Models.Dto;
using WowDin.Backstage.Models.Dto.Information;

namespace WowDin.Backstage.Services.Interface
{
    public interface IInformationService
    {
        public Task<List<UploadImageDto>> UploadImages(List<IFormFile> files);
        public Task<UploadImageDto> UploadImage(IFormFile file);
        public BrandManagementDto BrandManagementInit();
        public BrandFacadeDto GetBrandFacade();
        public BrandIntroduceDto GetBrandIntroduce();
        public BrandWebDto GetBrandWeb();
        public BrandImagesDto GetBrandImages();
        public ShopImagesDto GetShopImages();
        public SaveDto SaveBrandFacade(BrandFacadeDto brandFacade);
        public SaveDto SaveBrandIntroduce(BrandIntroduceDto brandIntroduce);
        public SaveDto SaveBrandWeb(BrandWebDto brandWeb);
        public SaveDto SaveBrandImages(BrandImagesDto brandImages);
        public SaveDto SaveShopImages(ShopImagesDto shopImages);
        public ShopManagementDto ShopManagementInit();
        public ShopInfoDto GetShopInfo(int id);
        public ShopBusinessDto GetShopBusiness(int id);
        public ShopTakeMethodDto GetShopTakeMethod(int id);
        public SaveDto CreateShop(int id, ShopInfoDto shopInfo);
        public SaveDto RemoveShop(int id);
        public SaveDto SaveShopInfo(int id, ShopInfoDto shopInfo);
        public SaveDto SaveShopBusiness(int id, ShopBusinessDto shopBusiness);
        public SaveDto SaveShopTakeMethod(int id, ShopTakeMethodDto shopTakeMethod);

    }
}
