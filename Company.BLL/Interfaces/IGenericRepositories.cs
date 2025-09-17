using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Specification.Interfaces;
using Company.DAL.Entites;

namespace Company.BLL.Interfaces
{
    public interface IGenericRepositories<T> where T:BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllDataWithSpec(ISpecification<T> spec);
        Task<T?> GetDataWithSpec(ISpecification<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
