namespace WowDin.Backstage.Models.Dto.Member
{
    public class UpdateCardDto
    {
        public int CardTypeId { get; set; }
        public string Name { get; set; }
        public int CardLevel { get; set; }
        public int Range { get; set; }
        public string CardImgUrl { get; set; }
    }
}
