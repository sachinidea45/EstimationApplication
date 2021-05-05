using EstimationApplication.Entities;
using System;

namespace EstimationApplication.API.Models
{
    public class LoginResponseModel : ResponseModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public CustomerModel Customer { get; set; }
    }
}
