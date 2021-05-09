using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using EstimationApplication.BusinessRule;
using EstimationApplication.Entities;

namespace EstimationApplication.Test
{
    public abstract class PrintBusinessBaseTest
    {
        protected IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            var configurationSettings = new Dictionary<string, string>
            {
                {EstimationApplicationConstant.DiscountPercentageRegular, "NotAvailableNow"},
                {EstimationApplicationConstant.DiscountPercentagePrivlieged, "2" },
                {EstimationApplicationConstant.OutputFilePath, "C:\\Users\\sachi\\Desktop\\New folder (2)\\SiemensRoundTwo" }
            };

            configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationSettings).Build();
        }

        protected EstimationModel GetEstimationModel(string userName, string role, decimal goldPricePerGram, decimal weightInGram, decimal expectedTotalPrice)
        {
            var estimate = new EstimationModel
            {
                Customer = new CustomerModel(configuration) { UserName = userName, UserCategories = new List<string> { role } },
                GoldPricePerGram = goldPricePerGram,
                WeightInGram = weightInGram
            };

            var mock = new Mock<ILogger<EstimateBusiness>>();
            IEstimateBusiness estimationBusiness = new EstimateBusiness(mock.Object);
            estimationBusiness.CalculateEstimate(estimate);
            return estimate;
        }
    }
}
