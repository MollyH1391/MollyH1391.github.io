using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Member;

namespace WowDin.Backstage.Services.Interface
{
    public interface IMemberService
    {
        APIResult GetBrandMemberData(int brandId);
        IEnumerable<GetCardGradingDto> GetCardGradingData(int brandId);
        //APIResult GetCardGradingData(int brandId);
        APIResult CreateCardGrading(CreateCardCradingDto input, int brandId);
        APIResult DeleteCard(DeleteCardDto input);
        APIResult UpdateCard(UpdateCardDto input);
        Task<APIResult> UploadImg(IFormFile file);
    }
}
