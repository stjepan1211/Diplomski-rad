using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface ITournamentService
    {
        Task<int> Add(ITournamentDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(ITournamentDomain entry);
        Task<ITournamentDomain> Read(Guid id);
        Task<IEnumerable<ITournamentDomain>> ReadAll();
        Task<int> Update(ITournamentDomain entry);
    }
}
