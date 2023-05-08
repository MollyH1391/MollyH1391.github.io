using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowDin.Frontstage.Models.Dto.Home
{
    [Serializable]
    public class GetIndexFiguresDto
    {
        public IEnumerable<PictureDto> BigPictures { get; set; }
        public IEnumerable<PictureDto> SmallPictures { get; set; }
    }

    public class PictureDto
    {
        public string PictureFile { get; set; }
        public int BrandId { get; set; }
        public string Code { get; set; }
    }
}
