using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;

namespace EstimationApplication.Data
{
    public class UserData : IUserData
    {
        private readonly string xmlDataPath;
        private IList<UserModel> _user;

        public UserData(IConfiguration _configuration)
        {
            var connectionString = _configuration.GetConnectionString(EstimationApplicationConstant.XMLConnectionString);
            var csValues = connectionString.Split(';');
            var uri = csValues.FirstOrDefault(x => x.StartsWith("URI"));
            xmlDataPath = uri.Split('=')[1];
            _user = LoadAllUserData();
        }

        public UserModel GetUserByUserName(string userName)
        {
            return _user.FirstOrDefault(x => x.UserName == userName);
        }

        public IList<UserModel> GetAllUser()
        {
            return _user;
        }

        private IList<UserModel> LoadAllUserData()
        {
            List<UserModel> users = new List<UserModel>();
            XDocument doc = XDocument.Load(xmlDataPath);
            foreach (XElement element in doc.Descendants(EstimationApplicationConstant.User))
            {
                UserModel user = new UserModel
                {
                    UserName = element.Element(EstimationApplicationConstant.UserName).Value,
                    Password = element.Element(EstimationApplicationConstant.Password).Value,
                    UserCategories = new List<Entities.UserCategory>()
                };
                foreach (XElement innerElement in element.Descendants(EstimationApplicationConstant.Categories))
                {
                    var userCategory = innerElement.Element(EstimationApplicationConstant.UserCategory).Value;
                    switch (userCategory)
                    {
                        case EstimationApplicationConstant.Privileged:
                            user.UserCategories.Add(Entities.UserCategory.Privileged);
                            break;
                        case EstimationApplicationConstant.Regular:
                            user.UserCategories.Add(Entities.UserCategory.Regular);
                            break;
                    }
                }
                users.Add(user);
            }
            return users;
        }
    }
}
