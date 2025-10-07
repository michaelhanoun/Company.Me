using System.ComponentModel.DataAnnotations;

namespace Dashboard.Dtos
{
    public class RoleToCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
