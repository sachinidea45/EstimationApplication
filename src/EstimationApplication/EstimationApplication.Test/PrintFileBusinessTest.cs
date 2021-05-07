using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using EstimationApplication.BusinessRule;

namespace EstimationApplication.Test
{
    public class PrintFileBusinessTest : PrintBusinessBaseTest
    {
        [TestCase("TestUser", "Regular", 4000, 8, 32000)]
        [TestCase("TestUser", "Privileged", 4000, 8, 31360)]
        public void TestPrint(string userName, string role, decimal goldPricePerGram, decimal weightInGram, decimal expectedTotalPrice)
        {
            var estimate = GetEstimationModel(userName, role, goldPricePerGram, weightInGram, expectedTotalPrice);
            var mock = new Mock<ILogger<PrintFileBusiness>>();

            IPrintBusiness printScreenBusiness = new PrintFileBusiness(configuration, mock.Object);
            var print = printScreenBusiness.Print(estimate);
            Assert.NotNull(print);
        }
    }
}
