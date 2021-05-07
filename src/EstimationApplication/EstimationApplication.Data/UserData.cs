using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EstimationApplication.Entities;
using System;

namespace EstimationApplication.Data
{
    public class UserData : IUserData
    {
        private readonly string xmlDataPath;
        ILogger<UserData> logger;
        private IList<UserModel> _users;

        public UserData(IConfiguration _configuration, ILogger<UserData> _logger)
        {
            logger = _logger;
            var connectionString = _configuration.GetConnectionString(EstimationApplicationConstant.XMLConnectionString);
            try
            {
                if (connectionString != null)
                {
                    var csValues = connectionString.Split(';');
                    var uri = csValues.FirstOrDefault(x => x.StartsWith("URI"));
                    xmlDataPath = uri.Split('=')[1];
                    logger.LogInformation("Started Loading Users Data");
                    _users = LoadAllUserData();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ExstimationApplicationDataException(ex.Message); ;
            }
        }

        public UserModel GetUserByUserName(string userName)
        {
            return (_users != null && _users.Count>0) ? _users.FirstOrDefault(x => x.UserName == userName) : null;
        }

        public IList<UserModel> GetAllUser()
        {
            return _users;
        }

        private IList<UserModel> LoadAllUserData()
        {
            List<UserModel> users = new List<UserModel>();
            try
            {
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
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ExstimationApplicationDataException(ex.Message);
            }
            return users;
        }
    }
}
