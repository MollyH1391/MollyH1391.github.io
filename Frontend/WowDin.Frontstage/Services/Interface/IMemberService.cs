using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.Dto.Home;
using WowDin.Frontstage.Models.Dto.Member;

namespace WowDin.Frontstage.Services.Member.Interface
{
    public interface IMemberService
    {
        GetUserDataByIdDto GetUserDataById(int id);
        IEnumerable<GetCardListByIdDto> GetCardListById(int id);
        bool UpdateUserData(UpdateUserDataDto userData);
        Task<UploadImgOutputDto> UploadImg(IFormFile file);
        public ResponseDto InitialResponse(int UserId);
        public void AddReponse(AddResponseDto addResponseData);
    }
}
