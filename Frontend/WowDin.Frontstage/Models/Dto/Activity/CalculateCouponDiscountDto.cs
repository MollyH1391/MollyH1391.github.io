namespace WowDin.Frontstage.Models.Dto.Activity
{
    public class CalculateCouponDiscountDto
    {
        public bool IsValid { get; set; }
        public string Hint { get; set; }
        public decimal Discount { get; set; }
    }
}
