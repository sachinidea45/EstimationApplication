using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;
using EstimationApplication.Data;

namespace EstimationApplication.BusinessRule
{
    public class EstimateBusiness : IEstimateBusiness
    {
        private readonly IUserData userData;
        protected IConfiguration configuration;

        public EstimateBusiness(IUserData _userData, IConfiguration _configuration)
        {
            userData = _userData;
            configuration = _configuration;
        }

        public void CalculateEstimate(EstimationModel estimate)
        {
            var calculatedEstimate = CalculateEstimateWithoutDiscount(estimate.GoldPricePerGram, estimate.WeightInGram);
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
