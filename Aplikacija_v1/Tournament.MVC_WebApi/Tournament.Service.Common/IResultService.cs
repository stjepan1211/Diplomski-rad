using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IResultService
    {
        Task<int> Add(IResultDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(IResultDomain entry);
        Task<IResultDomain> Read(Guid id);
        Task<IEnumerable<IResultDomain>> ReadAll();
        Task<int> Update(IResultDomain entry);
    }
}
