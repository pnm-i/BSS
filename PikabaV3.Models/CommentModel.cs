using System.ComponentModel.DataAnnotations;

namespace PikabaV3.Models
{
    public class CommentModel
    {
        [Required(ErrorMessage = "This field is required to fill")]
        [StringLength(200, ErrorMessage = "This field must be no more than 200 characters")]
        public string Text { get; set; }
    }
}