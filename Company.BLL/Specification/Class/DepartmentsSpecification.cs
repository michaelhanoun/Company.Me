using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Specification.Class
{
    public class DepartmentsSpecification :BaseSpecification<Department>
    {
        public DepartmentsSpecification(string name):base(D=>string.IsNullOrEmpty(name)||EF.Functions.Like(D.Name,$"%{name}%"))
        {
            
        }
    }
}
