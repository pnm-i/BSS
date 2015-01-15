using System.ComponentModel.DataAnnotations;

namespace PikabaV3.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(100, ErrorMessage = "This field must be no more than 100 characters")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string Password { get; set; }
    }
}