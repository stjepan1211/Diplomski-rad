using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IRefereeService
    {
        Task<int> Add(IRefereeDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(IRefereeDomain entry);
        Task<IRefereeDomain> Read(Guid id);
        Task<IEnumerable<IRefereeDomain>> ReadAll();
        Task<int> Update(IRefereeDomain entry);
    }
}
