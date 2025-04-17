using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tuổi")]
        [Range(1, 120, ErrorMessage = "Tuổi phải từ 1 đến 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string Gender { get; set; }
    }
} 