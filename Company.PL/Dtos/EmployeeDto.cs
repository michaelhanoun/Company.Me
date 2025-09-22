using System.ComponentModel.DataAnnotations;
using Company.DAL.Entites;

namespace Company.PL.Dtos
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Name is Required!")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string Name { get; set; }
        [Range(22, 30)]
        public int? Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-z]{5,10}-[a-zA-z]{4,10}-[a-zA-z]{5,10}$", ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; } = "0101253656";
        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmpType EmpType { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
