using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public interface IPrintBusiness
    {
        PrintModel Print(EstimationModel estimation);
    }
}
