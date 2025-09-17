using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Specification.Interfaces;
using Company.DAL.Entites;

namespace Company.BLL.Specification
{
    public static class SpecificationEvacuator<T> where T:BaseEntity
    {
        public static IQueryable<T> Evaluate(IQueryable<T> query,ISpecification<T>specification)
        {
            var queryToReturn = query;
            if(specification.Criteria is not null)
                queryToReturn = queryToReturn.Where(specification.Criteria);
            if (specification.OrderBy is not null)
                queryToReturn = queryToReturn.OrderBy(specification.OrderBy);
            else if (specification.OrderByDesc is not null)
                queryToReturn = queryToReturn.OrderByDescending(specification.OrderByDesc);
            if(specification.IsPaginationEnabled)
                queryToReturn = queryToReturn.Skip(specification.Skip).Take(specification.Take);
            queryToReturn = specification.Includes.Aggregate(queryToReturn,(q,i)=>i(q));
            return queryToReturn;
        }
    }
}
