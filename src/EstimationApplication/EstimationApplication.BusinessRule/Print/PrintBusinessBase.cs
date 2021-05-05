﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public abstract class PrintBusinessBase : IPrintBusiness
    {
        protected IConfiguration configuration;

        public PrintBusinessBase(IConfiguration _configuraiton)
        {
            configuration = _configuraiton;
        }

        public abstract PrintModel Print(EstimationModel estimation);

        protected string GetEstimationDataText(EstimationModel estimation)
        {
            return JsonConvert.SerializeObject(estimation);
        }
    }
}
