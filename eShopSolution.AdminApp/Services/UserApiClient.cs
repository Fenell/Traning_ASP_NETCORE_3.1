using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory factory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<string>> Authenticate(LoginRequest request)
        {
            HttpClient client = _factory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var jsonContext = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContext, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Users/authenticate", content);

            if (!response.IsSuccessStatusCode)
            {
                return new Response<string>()
                {
                    IsSuccess = false,
                    Message = await response.Content.ReadAsStringAsync()
                };
            }

            var token = await response.Content.ReadAsStringAsync();

            return new Response<string>()
            {
                IsSuccess = true,
                Data = token
            };
        }

        public async Task<Response<PagedResult<UserViewModel>>> GetUserPagings(GetUserPagingRequest request)
        {
            HttpClient httpClient = _factory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //set JWT vào request header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);

            var response = await httpClient.GetAsync($"/api/Users/paging?keyword={request.Keyword}&pageindex={request.PageIndex}&pagesize={request.PageSize}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var pageResult = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(jsonString);

            return new Response<PagedResult<UserViewModel>>()
            {
                IsSuccess = true,
                Data = pageResult
            };
        }

        public async Task<Response<UserViewModel>> GetUserById(Guid id)
        {
            HttpClient client = _factory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];

            if (string.IsNullOrEmpty(token))
            {
                return new Response<UserViewModel>()
                {
                    IsSuccess = false,
                    Message = "Phiên đăng nhập hết hạn"
                };
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/Users/{id}");
            var user = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());

            if (response.IsSuccessStatusCode)
            {
                return new Response<UserViewModel>()
                {
                    IsSuccess = true,
                    Data = user
                };
            }

            return new Response<UserViewModel>()
            {
                IsSuccess = false,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<Response<bool>> RegisterUser(RegisterRequest request)
        {
            HttpClient httpClient = _factory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("/api/Users", content);

            if (!result.IsSuccessStatusCode)
            {
                return new Response<bool>()
                {
                    IsSuccess = false,
                    Message = await result.Content.ReadAsStringAsync()
                };
            }

            return new Response<bool>()
            {
                IsSuccess = true,
                Message = await result.Content.ReadAsStringAsync()
            };
        }

        public async Task<Response<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            HttpClient client = _factory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];

            if (string.IsNullOrEmpty(token))
            {
                return new Response<bool>()
                {
                    IsSuccess = false,
                    Message = "Phiên đăng nhập hết hạn"
                };
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PutAsync($"/api/users/{id}", httpContent);

            var response = new Response<bool>(); 
            if (result.IsSuccessStatusCode)
            {
                response.IsSuccess = true;
                response.Message = await result.Content.ReadAsStringAsync();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = await result.Content.ReadAsStringAsync();
            }

            return response;
        }
    }
}
