namespace EstimationApplication.Entities
{
    public class EstimationModel
    {
        public CustomerModel Customer { get; set; }
        public decimal GoldPricePerGram { get; set; }
        public decimal WeightInGram { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
