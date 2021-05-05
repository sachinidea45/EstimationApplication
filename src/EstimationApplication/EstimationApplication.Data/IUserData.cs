using System.Collections.Generic;
using EstimationApplication.Entities;

namespace EstimationApplication.Data
{
    public interface IUserData
    {
        UserModel GetUserByUserName(string userName);
        IList<UserModel> GetAllUser();
    }
}
