using System;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;
using Microsoft.Extensions.Logging;

namespace EstimationApplication.BusinessRule
{
    public class EstimateBusiness : IEstimateBusiness
    {
        private readonly ILogger logger;

        public EstimateBusiness(ILogger<EstimateBusiness> _logger)
        {
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
                    calculatedEstimate = CalculateEstimateWithDiscount(calculatedEstimate, estimate.Customer.DiscountPercentApplicable);
                }
                else
                {
                    calculatedEstimate = CalculateEstimateWithDiscount(calculatedEstimate, estimate.Customer.DiscountPercentApplicable);
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
