namespace WowDin.Frontstage.Models.Dto.Member
{
    public class UploadImgOutputDto
    {
        public UploadImgOutputDto(bool isSuccess, string msg, string imgUrl)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            ImgUrl = imgUrl;
        }
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public string ImgUrl { get; set; }
    }
}
