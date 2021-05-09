using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace EstimationApplication.Entities
{
    public class CustomerModel
    {
        protected IConfiguration configuration;
        public CustomerModel(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string UserName { get; set; }
        public IList<string> UserCategories { get; set; }
        public decimal DiscountPercentApplicable {
            get
            {
                if (UserCategories.Contains(UserCategory.Privileged.ToString()) &&
                    Decimal.TryParse(configuration[EstimationApplicationConstant.DiscountPercentagePrivlieged], out decimal discount))
                {
                    return discount;
                }
                else if (UserCategories.Contains(UserCategory.Regular.ToString()) &&
                    Decimal.TryParse(configuration[EstimationApplicationConstant.DiscountPercentageRegular], out decimal regularDiscount))
                {
                    return regularDiscount;
                }
                return 0;
            }
        }
    }
}
