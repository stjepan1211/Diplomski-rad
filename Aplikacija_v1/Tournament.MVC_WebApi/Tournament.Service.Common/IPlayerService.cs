using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IPlayerService
    {
        Task<int> Add(IPlayerDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(IPlayerDomain entry);
        Task<IPlayerDomain> Read(Guid id);
        Task<IEnumerable<IPlayerDomain>> ReadAll();
        Task<int> Update(IPlayerDomain entry);
    }
}
