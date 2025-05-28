using System.ComponentModel.DataAnnotations;

namespace MindSpace.Presentation.Mvc.Models.Identity
{
    public class SignInModel
    {

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
