using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IAspNetUserLoginRepository
    {
        Task<int> Add(IAspNetUserLoginDomain entity);
        Task<int> Delete(string id);
        Task<int> Delete(IAspNetUserLoginDomain entity);
        Task<IAspNetUserLoginDomain> Get(string id);
        Task<IEnumerable<IAspNetUserLoginDomain>> GetAll();
        Task<int> Update(IAspNetUserLoginDomain entity);
    }
}
