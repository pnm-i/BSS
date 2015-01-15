using System.ComponentModel.DataAnnotations;

namespace PikabaV3.Models
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, ErrorMessage = "This field must be no more than 30 characters")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(100, ErrorMessage = "This field must be no more than 100 characters")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
    }

    public class UpdateSellerModel : UpdateUserModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, ErrorMessage = "This field must be no more than 30 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(20, ErrorMessage = "This field must be no more than 20 characters")]
        public string Phone { get; set; }
    }
}