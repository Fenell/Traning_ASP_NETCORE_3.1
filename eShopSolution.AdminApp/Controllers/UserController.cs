using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace eShopSolution.AdminApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserApiClient _userApiClient;
		private readonly IConfiguration _configuration;

		public UserController(IUserApiClient userApiClient, IConfiguration configuration)
		{
			_userApiClient = userApiClient;
			_configuration = configuration;
		}

		public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
		{
			var session = HttpContext.Session.GetString("Token");
			if (string.IsNullOrEmpty(session))
            {
                return RedirectToAction("Index", "Login");
            }
			var request = new GetUserPagingRequest()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize,
				BearerToken = session
			};
			var data = await _userApiClient.GetUserPagings(request);

			return View(data);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RegisterUser(request);
            if (result)
            {
                return RedirectToAction("Index", "Login");
            }
			return View(request);
        }

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Remove("Token");
			return RedirectToAction("Index");
		}

		private void SetCacheToken(string token)
		{
			Environment.SetEnvironmentVariable("token", token);
		}

		private string RetrieveCachedToken()
		{
			return Environment.GetEnvironmentVariable("token");
		}
	}
}
