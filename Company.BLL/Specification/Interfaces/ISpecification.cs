using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore.Query;

namespace Company.BLL.Specification.Interfaces
{
    public interface ISpecification<T> where T:BaseEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; }
        public List<Func<IQueryable<T>,IIncludableQueryable<T,object>>> Includes { get; set; }
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T,object>> OrderByDesc { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
