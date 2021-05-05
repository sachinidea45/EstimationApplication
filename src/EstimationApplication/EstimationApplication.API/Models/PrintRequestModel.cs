using System.ComponentModel.DataAnnotations;

namespace EstimationApplication.API.Models
{
    public class PrintRequestModel
    {
        [Required(ErrorMessage = "Estimation is required")]
        public EstimateRequestModel Estimation { get; set; }

        [Required(ErrorMessage = "PrintType is required")]
        public PrintType PrintType { get; set; }
    }

    public enum PrintType
    {
        PrintToScreen,
        PrintToFile,
        PrintToPaper
    }
}
