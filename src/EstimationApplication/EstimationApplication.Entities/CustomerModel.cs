using System.Collections.Generic;

namespace EstimationApplication.Entities
{
    public class CustomerModel
    {
        public string UserName { get; set; }
        public IList<string> UserCategories { get; set; }
    }
}
