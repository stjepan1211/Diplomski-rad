using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface ILocationService
    {
        Task<int> Add(ILocationDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(ILocationDomain entry);
        Task<ILocationDomain> Read(Guid id);
        Task<IEnumerable<ILocationDomain>> ReadAll();
        Task<int> Update(ILocationDomain entry);
        Task<IEnumerable<ILocationDomain>> ReadByTournament(Guid tournamentId);
    }
}
