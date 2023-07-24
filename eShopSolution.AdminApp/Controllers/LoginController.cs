using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using eShopSolution.AdminApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            var response = await _userApiClient.Authenticate(request);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }

            var token = response.Data;
            var userPrincipal = ValidateToken(response.Data);
            var opt = new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(10),
                HttpOnly = true,
                Secure = true
            };
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };

            //HttpContext.Session.SetString("Token", token);
            Response.Cookies.Append("Token", token, opt);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;

            //Object nhận nhận các thông tin từ JWT đã được xác thực: Issuer, Audience, Claims
            SecurityToken validatedToken;

            //Cấu hình các thông tin cần xác thực
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidIssuer = _configuration["Tokens:Issuer"],
                ValidAudience = _configuration["Tokens:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
            };


            // ClaimsPrincipal: đón các thông tin từ JWT đã được xác thực qua : new JwtSecurityTokenHandler().ValidateToken()
            // ClaimsPrincipal chứa các ClaimsIdentity mỗi ClaimsIdentity đại diện cho 1 thông tin (Claims)

            ClaimsPrincipal claimsPrincipal =
                new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);

            return claimsPrincipal;
        }

    }
}
