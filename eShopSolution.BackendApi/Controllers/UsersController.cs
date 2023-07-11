﻿using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromForm] LoginRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var resultToken = await _userService.Authenticate(request);

			if (string.IsNullOrEmpty(resultToken))
			{
				return BadRequest("User or password incorrect");
			}

			return Ok(new { token = resultToken });

		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromForm] RegisterRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _userService.Register(request);

			if (!result)
			{
				return BadRequest("Register unsuccessful");
			}

			return Ok();

		}

	}
}
