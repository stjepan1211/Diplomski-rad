using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IMatchService
    {
        Task<int> Add(IMatchDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(IMatchDomain entry);
        Task<IMatchDomain> Read(Guid id);
        Task<IEnumerable<IMatchDomain>> ReadAll();
        Task<int> Update(IMatchDomain entry);
    }
}
