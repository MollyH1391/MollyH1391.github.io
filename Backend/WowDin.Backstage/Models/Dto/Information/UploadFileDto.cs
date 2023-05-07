namespace WowDin.Backstage.Models.Dto.Information
{
    public class UploadFileDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FileUrl { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
