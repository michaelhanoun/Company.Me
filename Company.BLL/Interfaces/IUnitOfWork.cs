using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entites;

namespace Company.BLL.Interfaces
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        Task<int> Complete();
        IGenericRepositories<T> Repository<T>()where T:BaseEntity;
    }
}
