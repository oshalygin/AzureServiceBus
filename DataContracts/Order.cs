namespace ASB.Models
{
    public class Order
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Region { get; set; }
        public int Items { get; set; }
        public bool HasLoyaltyCard { get; set; }
    }
}
