using System.ComponentModel.DataAnnotations;

namespace PikabaV3.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string CurentPassword { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}