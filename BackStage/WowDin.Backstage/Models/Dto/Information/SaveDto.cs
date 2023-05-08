namespace WowDin.Backstage.Models.Dto.Information
{
    public class SaveDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string UpdateTime { get; set; }
        public Shop NewShop { get; set; }

        public class Shop
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
