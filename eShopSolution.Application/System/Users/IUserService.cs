using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.System.User;

namespace eShopSolution.Application.System.Users
{
	public interface IUserService
	{
		Task<string> Authenticate(LoginRequest request);

		Task<bool> Register(RegisterRequest  request);
	}
}
