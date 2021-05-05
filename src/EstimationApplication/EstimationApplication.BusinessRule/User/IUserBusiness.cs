using System.Collections.Generic;
using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public interface IUserBusiness
    {
        CustomerModel FindCustomerByUserName(string userName);
        bool ValidateUserNamePassword(UserAuthenticationModel userAuthentication);
        IList<string> GetUserRoles(string userName);
    }
}
