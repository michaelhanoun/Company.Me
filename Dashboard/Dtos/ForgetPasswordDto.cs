using System.ComponentModel.DataAnnotations;

namespace Dashboard.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
