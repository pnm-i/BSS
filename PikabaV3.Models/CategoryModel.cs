using System.ComponentModel.DataAnnotations;

namespace PikabaV3.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string Name { get; set; }
        public string Parent_Id { get; set; }
    }
}