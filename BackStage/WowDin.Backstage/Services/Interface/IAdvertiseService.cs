using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Advertise;

namespace WowDin.Backstage.Services.Interface
{
    public interface IAdvertiseService
    {
        AllAdvertiseDto GetAllAdvertise(int brandId);
        APIResult SubmitApplication(AdvertiseRequestDto request);
        APIResult ReSubmit(int adId);
    }
}
