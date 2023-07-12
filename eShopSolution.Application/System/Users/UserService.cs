using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolutiion.Data.Entities;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.Application.System.Users
{
	public class UserService:IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IConfiguration _config;

		public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
		}
		public async Task<string> Authenticate(LoginRequest request)
		{
			var user = await _userManager.FindByNameAsync(request.UserName);

			if (user == null)
			{
				return null;
			}

			var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
			if (!result.Succeeded)
			{
				return null;
			}

			var role = await _userManager.GetRolesAsync(user);
			var lstClaims = new[]
			{
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.GivenName, user.LastName),
				new Claim(ClaimTypes.Role, string.Join(";", role))
			};

			//Mã hóa Claim
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config["Tokens:Issuer"],
				audience: _config["Tokens:Issuer"],
				signingCredentials: creds,
				claims: lstClaims,
				expires: DateTime.Now.AddHours(3)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);

		}

		public async Task<bool> Register(RegisterRequest request)
		{
			var user = new AppUser()
			{
				DateOfBirth = request.DateOfBirth,
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
				UserName = request.UserName,
				PhoneNumber = request.PhoneNumber
			};
			var result = await _userManager.CreateAsync(user, request.Password);

			return result.Succeeded;
		}
	}
}
