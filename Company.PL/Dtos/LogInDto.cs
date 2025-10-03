using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class LogInDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberME { get; set; }

    }
}
