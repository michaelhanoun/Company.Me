using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Specification.Class
{
    public class EmployeesSpecification:BaseSpecification<Employee>
    {
        public EmployeesSpecification(string name) : base(E => string.IsNullOrEmpty(name) || EF.Functions.Like(E.Name, $"%{name}%"))
        {
            Includes.Add(Q => Q.Include(E => E.Department)); ;
        }
        public EmployeesSpecification(int id) : base( E=> E.Id == id)
        {
            Includes.Add(Q => Q.Include(E => E.Department)); ;
        }
    }
}
