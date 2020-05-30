using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaymentGatewayService.Api.ViewModels.Authentication;

namespace PaymentGatewayService.Api.Controllers
{
    /*
     * This controller just simulates authentication for the application with hardcoded values.
     * Ideally we would have an authentication service that would handle authentication between
     * the different services and it would have it's own DB to keep user information.
     */
    [Route("api/v1/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserLoginRequest login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJwt();

                response = Ok(
                    new { token = $"Bearer {tokenString}" }
                );
            }

            return response;
        }

        private string GenerateJwt()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:JwtKey"]));

            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Authentication:JwtIssuer"],
                _configuration["Authentication:JwtAudience"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserLoginRequest AuthenticateUser(UserLoginRequest loginRequest)
        {
            UserLoginRequest user = null;

            if (loginRequest.Username == "checkout")
            {
                user = new UserLoginRequest
                {
                    Username = "Checkout Demo User"
                };
            }

            return user;
        }
    }
}