using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;

namespace eShopSolution.Application.System.Users
{
	public interface IUserService
	{
		Task<Response<string>> Authenticate(LoginRequest request);

		Task<Response<bool>> Register(RegisterRequest  request);

		Task<Response<bool>> Update(Guid id, UserUpdateRequest  request);

		Task<Response<bool>> Delete(Guid id);

		Task<PagedResult<UserViewModel>> GetUserList(GetUserPagingRequest request);

		Task<Response<UserViewModel>> GetUserById(Guid id);
	}
}
