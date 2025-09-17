using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Specification.Interfaces;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore.Query;

namespace Company.BLL.Specification.Class
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; set; } = new();
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> Criteria)
        {
            this.Criteria = Criteria;   
        }
        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        public void ApplyPagination(int skip , int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
