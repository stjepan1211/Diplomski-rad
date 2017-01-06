using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IAspNetUserLoginService
    {
        Task<int> Add(IAspNetUserLoginDomain entry);
        Task<int> Delete(string id);
        Task<int> Delete(IAspNetUserLoginDomain entry);
        Task<IAspNetUserLoginDomain> Read(string id);
        Task<IEnumerable<IAspNetUserLoginDomain>> ReadAll();
        Task<int> Update(IAspNetUserLoginDomain entry);
    }
}
