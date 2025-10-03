using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First name")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum Password Length is 6")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum Password Length is 6")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
