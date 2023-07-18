using System.Threading.Tasks;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;

namespace eShopSolution.AdminApp.Services
{
	public interface IUserApiClient
	{
		Task<string> Authenticate(LoginRequest request);

		Task<PagedResult<UserViewModel>> GetUserPagings(GetUserPagingRequest  request);

		Task<bool> RegisterUser(RegisterRequest  request);
	}
}
