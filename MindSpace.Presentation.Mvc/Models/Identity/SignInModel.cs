using System.ComponentModel.DataAnnotations;

namespace MindSpace.Presentation.Mvc.Models.Identity
{
    public class SignInModel
    {

        [Required]
        
        public required string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
