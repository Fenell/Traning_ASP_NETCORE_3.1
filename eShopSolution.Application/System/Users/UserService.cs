using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolutiion.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
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
        public async Task<Response<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return new Response<string>()
                {
                    IsSuccess = false,
                    Message = "Tên tài khoản hoặc mật khẩu không đúng"
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new Response<string>()
                {
                    IsSuccess = false,
                    Message = "Đăng nhập không thành công"
                };
            }

            var role = await _userManager.GetRolesAsync(user);
            var lstClaims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.LastName),
                new Claim(ClaimTypes.Role, string.Join(";", role)),
                new Claim(ClaimTypes.Name,user.UserName )
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

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response<string>()
            {
                IsSuccess = true,
                Message = "Đăng nhập thành công",
                Data = jwtToken
            };
        }

        public async Task<Response<bool>> Register(RegisterRequest request)
        {
            var response = new Response<bool>();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                response.IsSuccess = false;
                response.Message = "Tài khoản đã tồn tại";
                return response;
            }

            user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                response.IsSuccess = false;
                response.Message = "Email đã tồn tại";
                return response;
            }

            user = new AppUser()
            {
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.Message = "Đăng ký thành công";
                return response;
            }

            response.IsSuccess = false;
            response.Message = "Đăng ký không thành công";
            return response;
        }

        public async Task<Response<bool>> Update(Guid id, UserUpdateRequest request)
        {
            var response = new Response<bool>();
            var haveUser = await _userManager.Users.AnyAsync(c => c.Email == request.Email && c.Id != id);

            if (haveUser)
            {
                response.IsSuccess = false;
                response.Message = "Email đã tồn tại";
                return response;
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy tài khoản";
                return response;
            }
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.DateOfBirth = request.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.Message = "Cập nhật thành công";
                return response;
            }

            response.IsSuccess = false;
            response.Message = "Cập nhật thất bại";
            return response;

        }

        public Task<Response<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<UserViewModel>> GetUserList(GetUserPagingRequest request)
        {
            //1 query
            var query = _userManager.Users;

            //2 filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(
                    c => c.UserName.Contains(request.Keyword) || c.PhoneNumber.Contains(request.Keyword));
            }

            //3 Paging
            var totalRow = await query.CountAsync();
            var page = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(c =>
                new UserViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    UserName = c.UserName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                }).ToListAsync();

            //4 select and projection
            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecode = totalRow,
                Items = page
            };

            return pageResult;
        }

        public async Task<Response<UserViewModel>> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new Response<UserViewModel>()
                {
                    IsSuccess = false,
                    Message = "Khong tim thay user"
                };
            }

            var userVm = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };

            return new Response<UserViewModel>()
            {
                IsSuccess = true,
                Data = userVm
            };
        }
    }
}
