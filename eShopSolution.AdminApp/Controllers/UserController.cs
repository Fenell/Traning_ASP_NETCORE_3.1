using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
            //  var session = HttpContext.Session.GetString("Token");

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");

            }

            var jwtToken = Request.Cookies["Token"];

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var request = new GetUserPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    BearerToken = jwtToken
                };
                var response = await _userApiClient.GetUserPagings(request);

                return View(response.Data);
            }
            //if (string.IsNullOrEmpty(session))
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            // var cookie = Request.Cookies["Token"];

            return RedirectToAction("Index", "Login");
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
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _userApiClient.GetUserById(id);
            if (response.IsSuccess)
            {
                var user = new UserUpdateRequest()
                {
                    Id = response.Data.Id,
                    FirstName = response.Data.FirstName,
                    LastName = response.Data.LastName,
                    Email = response.Data.Email,
                    DateOfBirth = response.Data.DateOfBirth,
                    PhoneNumber = response.Data.PhoneNumber
                };
                return View(user);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var respone = await _userApiClient.UpdateUser(request.Id, request);

            if (respone.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", respone.Message);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);

            return View(result.Data);


        }

        //private void SetCacheToken(string token)
        //{
        //    Environment.SetEnvironmentVariable("token", token);
        //}

        //private string RetrieveCachedToken()
        //{
        //    return Environment.GetEnvironmentVariable("token");
        //}
    }
}
