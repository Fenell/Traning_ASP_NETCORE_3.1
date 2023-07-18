using System.Threading.Tasks;
using eShopSolution.Application.System.Users;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Authorization;
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
		public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
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

			return Ok( resultToken );

		}

		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
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
		[Authorize]
		// http://localhost/api/users/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("paging")]
		public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
		{
			var users = await _userService.GetUserList(request);
			 
			return Ok(users);
		}

	}
}
