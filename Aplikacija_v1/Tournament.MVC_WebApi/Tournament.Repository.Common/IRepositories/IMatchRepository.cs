using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IMatchRepository
    {
        Task<int> Add(IMatchDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(IMatchDomain entity);
        Task<IMatchDomain> Get(Guid id);
        Task<IEnumerable<IMatchDomain>> GetAll();
        Task<int> Update(IMatchDomain entity);
        Task<IEnumerable<IMatchDomain>> GetMatchesByTournament(Guid tournamentId);
        Task<IEnumerable<IMatchDomain>> GetMatchesByRounds(Guid tournamentId, int round);
    }
}
