﻿using EstimationApplication.API.Authentication;
using EstimationApplication.API.Models;
using EstimationApplication.BusinessRule;
using EstimationApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EstimationApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserBusiness userBusiness;
        private readonly ILogger logger;

        public UserController(IUserBusiness _userBusiness, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<UserController> _logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            userBusiness = _userBusiness;
            logger = _logger;
        }

        [HttpPost]
        [Route("authentication")]
        public async Task<ActionResult> Login([FromForm] LoginRequestModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                logger.LogInformation("Authentication Succesfull, Started Generating Token");
                var userRoles = await userManager.GetRolesAsync(user);

                var authAdditionalClaims = new Claim[userRoles.Count];
                for (int i = 0; i < userRoles.Count; i++)
                {
                    authAdditionalClaims[i] = new Claim(ClaimTypes.Role, userRoles[i]);
                }

                var signingKey = _configuration["JWT:Secret"];
                var issuer = _configuration["JWT:ValidIssuer"];
                var audience = _configuration["JWT:ValidAudience"];
                var expiration = new TimeSpan(3,0,0);
                
                var token = JwtHelper.GetJwtToken(user.UserName, signingKey, issuer, audience, expiration, authAdditionalClaims);

                //var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //var token = new JwtSecurityToken(
                //    issuer: _configuration["JWT:ValidIssuer"],
                //    audience: _configuration["JWT:ValidAudience"],
                //    expires: DateTime.Now.AddHours(3),
                //    claims: authClaims,
                //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                //    );

                return Ok(new LoginResponseModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    Customer = userBusiness.FindCustomerByUserName(model.Username),
                    Status = EstimationApplicationConstant.OkStatus,
                    Message = EstimationApplicationConstant.UserAuthenticationSuccessful,
                });
            }
            logger.LogWarning("Invalid Login Credentials");
            return Unauthorized();
        }
    }
}
