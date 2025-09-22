using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Entites
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public enum EmpType
    {
        [EnumMember(Value = "FullTime")]
        FullTime = 1,
        [EnumMember(Value = "PartTime")]
        PartTime = 2
    }
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }

        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = "0101253656";
        public DateTime HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmpType EmpType { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
