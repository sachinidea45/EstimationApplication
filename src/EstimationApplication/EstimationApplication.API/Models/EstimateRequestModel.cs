using System.ComponentModel.DataAnnotations;

namespace EstimationApplication.API.Models
{
    public class EstimateRequestModel
    {
        [Required(ErrorMessage = "Gold Price per Gram is required")]
        public decimal GoldPricePerGram { get; set; }

        [Required(ErrorMessage = "Weight in Gram is required")]
        public decimal WeightInGram { get; set; }
    }
}
