using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IPlayerRepository
    {
        Task<int> Add(IPlayerDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(IPlayerDomain entity);
        Task<IPlayerDomain> Get(Guid id);
        Task<IEnumerable<IPlayerDomain>> GetAll();
        Task<int> Update(IPlayerDomain entity);
        Task<IEnumerable<IPlayerDomain>> GetPlayersByTeam(Guid teamId);
        Task<IEnumerable<IPlayerDomain>> GetAllPlayersByTournament(Guid tournamentId);
    }
}
