using System;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;
using Microsoft.Extensions.Logging;

namespace EstimationApplication.BusinessRule
{
    public class EstimateBusiness : IEstimateBusiness
    {
        protected IConfiguration configuration;
        private readonly ILogger logger;

        public EstimateBusiness(IConfiguration _configuration, ILogger<EstimateBusiness> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }

        public void CalculateEstimate(EstimationModel estimate)
        {
            var calculatedEstimate = CalculateEstimateWithoutDiscount(estimate.GoldPricePerGram, estimate.WeightInGram);
            try
            {
                logger.LogInformation("Started Calculating Estimate");
                if (estimate.Customer.UserCategories.Contains(Entities.UserCategory.Privileged.ToString()))
                {
                    if (Decimal.TryParse(configuration[EstimationApplicationConstant.DiscountPercentagePrivlieged], out decimal discount))
                    {
                        estimate.Discount = discount;
                        calculatedEstimate = CalculateEstimateWithDiscount(calculatedEstimate, estimate.Discount);
                    }
                }
                else
                {
                    if (Decimal.TryParse(configuration[EstimationApplicationConstant.DiscountPercentageRegular], out decimal discount))
                    {
                        estimate.Discount = discount;
                        calculatedEstimate = CalculateEstimateWithDiscount(calculatedEstimate, estimate.Discount);
                    }
                }
                estimate.TotalPrice = calculatedEstimate;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ExstimationApplicationBusinessException(ex.Message);
            }
        }

        private decimal CalculateEstimateWithoutDiscount(decimal goldPricePerGram, decimal weightInGram)
        {
            return goldPricePerGram * weightInGram;
        }

        private decimal CalculateEstimateWithDiscount(decimal calculatedEstimateWithoutDiscount, decimal discountPercentage)
        {
            return calculatedEstimateWithoutDiscount - discountPercentage * calculatedEstimateWithoutDiscount / 100;
        }
    }
}
