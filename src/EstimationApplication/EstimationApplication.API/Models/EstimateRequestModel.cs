using System.ComponentModel.DataAnnotations;

namespace EstimationApplication.API.Models
{
    public class EstimateRequestModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Gold Price per Gram is required")]
        public decimal GoldPricePerGram { get; set; }

        [Required(ErrorMessage = "Weight in Gram is required")]
        public decimal WeightInGram { get; set; }
    }
}
