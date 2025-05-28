using System.ComponentModel.DataAnnotations;
using MindSpace.Presentation.Mvc.Helpers.Validation;

namespace MindSpace.Presentation.Mvc.Models.Identity
{
    public class RegisterModel
    {
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }

        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email can only contain 100 characters")]
        public string? Email { get; set; }

        [Display(Name = "I want to receive the newsletter")]
        public bool NewsLetter { get; set; }

        [Display(Name = "Image")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? ImageFile { get; set; }

        

    }
}
