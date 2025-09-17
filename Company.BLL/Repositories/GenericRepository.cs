using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Specification;
using Company.BLL.Specification.Interfaces;
using Company.DAL.Data;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepositories<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _companyDbContext;

        public GenericRepository(CompanyDbContext companyDbContext)
        {
            _companyDbContext = companyDbContext;
        }


        public async Task<IReadOnlyList<T>> GetAllDataWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetDataWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _companyDbContext.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _companyDbContext.Update(entity);
        }
        public void Delete(T entity)
        {
            _companyDbContext.Remove(entity);
        }
        private IQueryable<T> ApplySpecifications(ISpecification<T> specification)
        {
            return SpecificationEvacuator<T>.Evaluate(_companyDbContext.Set<T>(),specification);
        }
    }
}
