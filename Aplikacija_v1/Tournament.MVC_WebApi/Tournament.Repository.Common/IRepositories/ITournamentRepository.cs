using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface ITournamentRepository
    {
        Task<int> Add(ITournamentDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(ITournamentDomain entity);
        Task<ITournamentDomain> Get(Guid id);
        Task<IEnumerable<ITournamentDomain>> GetAll();
        Task<int> Update(ITournamentDomain entity);
        Task<IEnumerable<ITournamentDomain>> GetByUsername(string username);
        Task<IEnumerable<ITournamentDomain>> GetLeagueTournaments();
    }
}
