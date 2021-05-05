using System;
using System.Collections.Generic;
using System.Linq;
using EstimationApplication.Data;
using EstimationApplication.Entities;
using Microsoft.Extensions.Logging;

namespace EstimationApplication.BusinessRule
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserData userData;
        private readonly ILogger logger;

        public UserBusiness(IUserData _userData, ILogger<UserBusiness> _logger)
        {
            userData = _userData;
            logger = _logger;
        }

        public CustomerModel FindCustomerByUserName(string userName)
        {
            var user = userData.GetUserByUserName(userName);
            if (user != null)
            {
                return GetCustomerFromUser(user); ;
            }
            return null;
        }

        public bool ValidateUserNamePassword(UserAuthenticationModel userAuthentication)
        {
            var result = userData.GetAllUser()
                .FirstOrDefault(x => x.UserName == userAuthentication.UserName && x.Password == userAuthentication.Password);
            return result != null;
        }

        public IList<string> GetUserRoles(string userName)
        {
            var user = userData.GetUserByUserName(userName);
            if (user != null)
            {
                var customer = GetCustomerFromUser(user);
                return customer.UserCategories;
            }
            return null;
        }

        private CustomerModel GetCustomerFromUser(UserModel user)
        {
            var customer = new CustomerModel() { UserName = user.UserName, UserCategories = new List<string>() };
            try
            {
                foreach (var item in user.UserCategories)
                {
                    customer.UserCategories.Add(item.ToString());
                }
                return customer;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ExstimationApplicationBusinessException(ex.Message);
            }
        }
    }
}
