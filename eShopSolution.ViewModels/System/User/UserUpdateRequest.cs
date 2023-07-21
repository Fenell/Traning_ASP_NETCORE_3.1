using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.User
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui long nhap ho")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Vui long nhap ten")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
       
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
       
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận lại mật khẩu")]
        public string ComfirmPassword { get; set; }
    }
}
