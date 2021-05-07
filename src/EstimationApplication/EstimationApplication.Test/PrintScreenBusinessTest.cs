using NUnit.Framework;
using EstimationApplication.BusinessRule;

namespace EstimationApplication.Test
{
    public class PrintScreenBusinessTest : PrintBusinessBaseTest
    {
        [TestCase("TestUser", "Regular", 4000, 8, 32000)]
        [TestCase("TestUser", "Privileged", 4000, 8, 31360)]
        public void TestPrint(string userName, string role, decimal goldPricePerGram, decimal weightInGram, decimal expectedTotalPrice)
        {
            var estimate = GetEstimationModel(userName, role, goldPricePerGram, weightInGram, expectedTotalPrice);

            IPrintBusiness printScreenBusiness = new PrintScreenBusiness(configuration);
            var print = printScreenBusiness.Print(estimate);
            Assert.NotNull(print);
        }
    }
}
