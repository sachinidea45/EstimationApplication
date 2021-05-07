using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using EstimationApplication.Entities;
using EstimationApplication.BusinessRule;
using EstimationApplication.Data;

namespace EstimationApplication.Test
{
    class UserBusinessTest
    {
        [TestCase("TestUser")]
        public void TestFindCustomerByUserName(string userName)
        {
            var expectedCustomer = new CustomerModel()
            {
                UserName = userName,
                UserCategories = new List<string> { EstimationApplicationConstant.Regular } 
            };
            Mock<IUserData> mockUserData = new Mock<IUserData>();
            mockUserData.Setup(x => x.GetUserByUserName(userName))
                .Returns(
                new UserModel
                {
                    UserName = userName,
                    UserCategories = new List<UserCategory> { UserCategory.Regular }
                });

            IUserBusiness userBusiness = new UserBusiness(mockUserData.Object, null);
            var customer = userBusiness.FindCustomerByUserName(userName);
            Assert.NotNull(customer);
            Assert.AreEqual(expectedCustomer.UserName, customer.UserName);
        }

        [TestCase("TestUser", "TestPassword", true)]
        [TestCase("TestUser", "WrongPassword", false)]
        [TestCase("WrongUserName", "TestPassword", false)]
        [TestCase("WrongUserName", "WrongPassword", false)]
        public void TestValidateUserNamePassword(string userName, string password, bool isValid)
        {
            Mock<IUserData> mockUserData = new Mock<IUserData>();
            mockUserData.Setup(x => x.GetAllUser())
                .Returns(new List<UserModel>()
                {
                    new UserModel { UserName = "TestUser", Password = "TestPassword" } 
                });

            IUserBusiness userBusiness = new UserBusiness(mockUserData.Object, null);
            var isValidate = userBusiness.ValidateUserNamePassword(new UserAuthenticationModel { UserName = userName, Password = password });
            if (isValid)
            {
                Assert.True(isValidate);
            }
            else
            {
                Assert.False(isValidate);
            }
        }

        [TestCase("TestUser")]
        public void TestGetUserRoles(string userName)
        {
            Mock<IUserData> mockUserData = new Mock<IUserData>();
            mockUserData.Setup(x => x.GetUserByUserName(userName))
                .Returns(
                new UserModel
                {
                    UserName = userName,
                    UserCategories = new List<UserCategory> { UserCategory.Regular }
                });
            IUserBusiness userBusiness = new UserBusiness(mockUserData.Object, null);
            var roles = userBusiness.GetUserRoles(userName);
            Assert.NotNull(roles);
        }
    }
}
