using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpClientFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            HttpClient client = _factory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var jsonContext = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContext, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Users/authenticate", content);

            if (!response.IsSuccessStatusCode)  
            {
                return "Login fail";
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<PagedResult<UserViewModel>> GetUserPagings(GetUserPagingRequest request)
        {
            HttpClient httpClient = _factory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //set JWT vào request header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);

            var response = await httpClient.GetAsync($"/api/Users/paging?keyword={request.Keyword}&pageindex={request.PageIndex}&pagesize={request.PageSize}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var pageResult = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(jsonString);

            return pageResult;
        }

        public async Task<bool> RegisterUser(RegisterRequest request)
        {
	        HttpClient httpClient = _factory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("/api/Users", content);

            return result.IsSuccessStatusCode;

        }
    }
}
