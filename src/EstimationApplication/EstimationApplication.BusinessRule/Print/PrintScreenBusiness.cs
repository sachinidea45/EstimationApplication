using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public class PrintScreenBusiness:PrintBusinessBase
    {
        public PrintScreenBusiness(IConfiguration _configuraiton) : base(_configuraiton)
        {
        }

        public override PrintModel Print(EstimationModel estimation)
        {
            return new PrintModel { Estimation = estimation, PrintMessageOutput = GetEstimationDataText(estimation) };
        }
    }
}
