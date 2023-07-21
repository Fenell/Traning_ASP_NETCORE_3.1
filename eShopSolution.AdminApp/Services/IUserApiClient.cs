using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;

namespace eShopSolution.AdminApp.Services
{
	public interface IUserApiClient
	{
		Task<Response<string>> Authenticate(LoginRequest request);

		Task<Response<bool>> RegisterUser(RegisterRequest request);

        Task<Response<bool>> UpdateUser(Guid id, UserUpdateRequest request);

		Task<Response<PagedResult<UserViewModel>>> GetUserPagings(GetUserPagingRequest  request);

		Task<Response<UserViewModel>> GetUserById(Guid id);
    }
}
