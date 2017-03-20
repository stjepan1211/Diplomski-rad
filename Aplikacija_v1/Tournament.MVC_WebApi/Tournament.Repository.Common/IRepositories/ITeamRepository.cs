using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface ITeamRepository
    {
        Task<int> Add(ITeamDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(ITeamDomain entity);
        Task<ITeamDomain> Get(Guid id);
        Task<IEnumerable<ITeamDomain>> GetAll();
        Task<int> Update(ITeamDomain entity);
        Task<IEnumerable<ITeamDomain>> GetAllWhereTournamentId(Guid tournamentId);
        Task<ITeamDomain> GetLeagueTournamentWinner(Guid tournamentId);
        Task<IEnumerable<ITeamDomain>> GetLeagueTournamentFirstTwoTeams(Guid tournamentId);
        Task<IEnumerable<ITeamDomain>> MostPoints();
        Task<IEnumerable<ITeamDomain>> MostWins();
        Task<IEnumerable<ITeamDomain>> MostGoals();

    }
}
