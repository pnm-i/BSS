using System.ComponentModel.DataAnnotations;
using PikabaV3.Models.Entities;

namespace PikabaV3.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [RegularExpression(@"([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)", ErrorMessage = "Invalid Display Name")]
        [StringLength(30, ErrorMessage = "This field must be no more than 30 characters")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(100, ErrorMessage = "This field must be no more than 30 characters")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterSellerModel : RegisterUserModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, ErrorMessage = "This field must be no more than 30 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(20, ErrorMessage = "This field must be no more than 20 characters")]
        public string Phone { get; set; }
    }
}