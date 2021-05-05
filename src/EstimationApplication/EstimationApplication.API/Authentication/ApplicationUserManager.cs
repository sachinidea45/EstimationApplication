using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EstimationApplication.BusinessRule;

namespace EstimationApplication.API.Authentication
{
    public class ApplicationUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser
    {
        private readonly IUserBusiness userBusiness;

        public ApplicationUserManager(IUserBusiness _userBusiness, IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            userBusiness = _userBusiness;
        }

        public override Task<TUser> FindByNameAsync(string userName)
        {
            var customer = userBusiness.FindCustomerByUserName(userName);
            return Task.Run(() =>
                {
                    if (customer != null)
                    {
                        return (TUser)new ApplicationUser { UserName = customer.UserName };
                    }
                    return null;
                });
        }

        public override Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            return Task.Run(() =>
            {
                return userBusiness.ValidateUserNamePassword(new Entities.UserAuthenticationModel { UserName = user.UserName, Password = password });
            });
        }

        public override Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.Run(() => { return userBusiness.GetUserRoles(user.UserName); });
        }
    }
}
