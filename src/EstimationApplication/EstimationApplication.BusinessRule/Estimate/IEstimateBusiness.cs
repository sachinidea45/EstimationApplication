using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public interface IEstimateBusiness
    {
        void CalculateEstimate(EstimationModel estimate);
    }
}
