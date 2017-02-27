using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface ILocationRepository
    {
        Task<int> Add(ILocationDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(ILocationDomain entity);
        Task<ILocationDomain> Get(Guid id);
        Task<IEnumerable<ILocationDomain>> GetAll();
        Task<int> Update(ILocationDomain entity);
        Task <IEnumerable<ILocationDomain>> GetWhereTournamentId(Guid tournamentId);
    }
}
