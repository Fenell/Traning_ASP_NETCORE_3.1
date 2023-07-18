﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.User
{
	public class RegisterRequest
	{
		[Display(Name = "Tên")]
		public string FirstName { get; set; }
		[Display(Name = "Họ")]
		public string LastName { get; set; }
		[Display(Name = "Ngày sinh")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }
		[Display(Name = "Email")]
		public string Email { get; set; }
		[Display(Name = "Số điện thoại")]
		public string PhoneNumber { get; set; }
		[Display(Name = "Tài khoản")]
		public string UserName { get; set; }
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }
		[Display(Name = "Xác nhận lại mật khẩu")]
		public string ComfirmPassword { get; set; }
	}
}
