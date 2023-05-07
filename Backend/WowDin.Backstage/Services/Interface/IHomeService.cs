using System.Collections.Generic;
using WowDin.Backstage.Models.Dto.Home;
using WowDin.Backstage.Models.Dto.Member;

namespace WowDin.Backstage.Services.Interface
{
    public interface IHomeService
    {
        public GetChartDataByIdDto GetChartDataById(int brandId);

        public IEnumerable<GetCommentDataDto> GetCommentData(int brandId);
    }
}
