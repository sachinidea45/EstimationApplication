using System;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public class PrintPaperBusiness : PrintBusinessBase
    {
        public PrintPaperBusiness(IConfiguration _configuraiton) : base(_configuraiton)
        {

        }
        public override PrintModel Print(EstimationModel estimation)
        {
            throw new NotImplementedException();
        }
    }
}
