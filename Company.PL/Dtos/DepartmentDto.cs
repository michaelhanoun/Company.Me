using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class DepartmentDto
    {
        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Create Date is Required")]
        public DateTime CratedAt { get; set; }
    }
}
