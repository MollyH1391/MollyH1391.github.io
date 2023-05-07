namespace WowDin.Frontstage.Models.Dto.Order
{
    public class ProductInCartDto
    {
        public int ShopId { get; set; }
        public int ProductsAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
