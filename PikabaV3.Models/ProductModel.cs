using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PikabaV3.Models
{
    public class ProductModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length 3, maximum 30")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        [RegularExpression("([0-9]+)", ErrorMessage = "It not number")]
        [Range(1, 1000000, ErrorMessage = "The value may be from 1 to 1 000 000.")]
        [StringLength(30, ErrorMessage = "This field must be no more than 30 characters")]
        public decimal Price { get; set; }

        [StringLength(200, ErrorMessage = "This field must be no more than 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required to fill")]
        public List<string> Category_Ids { get; set; }
    }
}