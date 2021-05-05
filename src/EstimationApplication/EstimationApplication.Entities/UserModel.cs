using System.Collections.Generic;

namespace EstimationApplication.Entities
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public IList<UserCategory> UserCategories { get; set; }
    }

    public enum UserCategory
    {
        Privileged, 
        Regular
    }
}
