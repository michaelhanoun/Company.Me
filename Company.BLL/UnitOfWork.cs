using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data;
using Company.DAL.Entites;

namespace Company.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _companyDbContext;
        private Hashtable _repositories;
        public UnitOfWork(CompanyDbContext companyDbContext)
        {
            _companyDbContext = companyDbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> Complete()
        {
           return await _companyDbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _companyDbContext.DisposeAsync();
        }

        public IGenericRepositories<T> Repository<T>() where T : BaseEntity
        {
            string name = typeof(T).Name;
            if (!_repositories.ContainsKey(name))
                _repositories.Add(name, new GenericRepository<T>(_companyDbContext));
            return _repositories[name] as IGenericRepositories<T>;
        }
    }
}
