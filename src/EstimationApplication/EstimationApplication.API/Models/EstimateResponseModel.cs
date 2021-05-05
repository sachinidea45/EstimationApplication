using EstimationApplication.Entities;

namespace EstimationApplication.API.Models
{
    public class EstimateResponseModel : ResponseModel
    {
        public EstimationModel Estimate { get; set; }
    }
}
