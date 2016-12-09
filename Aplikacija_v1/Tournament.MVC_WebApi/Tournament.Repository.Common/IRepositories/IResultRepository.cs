using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IResultRepository
    {
        Task<int> Add(IResultDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(IResultDomain entity);
        Task<IResultDomain> Get(Guid id);
        Task<IEnumerable<IResultDomain>> GetAll();
        Task<int> Update(IResultDomain entity);
    }
}
