using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using EstimationApplication.BusinessRule;
using EstimationApplication.Entities;

namespace EstimationApplication.Test
{
    public class EstimateBusinessTest
    {
        IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            var configurationSettings = new Dictionary<string, string>
            {
                {EstimationApplicationConstant.DiscountPercentageRegular, "NotAvailableNow"},
                {EstimationApplicationConstant.DiscountPercentagePrivlieged, "2" }
            };

            configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationSettings).Build();
        }

        [TestCase("TestUser", "Regular", 4000, 8, 32000)]
        [TestCase("TestUser", "Privileged", 4000, 8, 31360)]
        public void TestCalculateEstimate(string userName, string role, decimal goldPricePerGram, decimal weightInGram, decimal expectedTotalPrice)
        {
            var estimate = new EstimationModel
            {
                Customer = new CustomerModel { UserName = userName, UserCategories = new List<string> { role } },
                GoldPricePerGram = goldPricePerGram,
                WeightInGram = weightInGram
            };
            var mock = new Mock<ILogger<EstimateBusiness>>();

            IEstimateBusiness estimationBusiness = new EstimateBusiness(configuration, mock.Object);
            estimationBusiness.CalculateEstimate(estimate);
            Assert.AreEqual(expectedTotalPrice, estimate.TotalPrice);
        }
    }
}