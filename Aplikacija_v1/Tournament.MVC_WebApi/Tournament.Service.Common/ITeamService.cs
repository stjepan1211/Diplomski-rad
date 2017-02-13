using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface ITeamService
    {
        Task<int> Add(ITeamDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(ITeamDomain entry);
        Task<ITeamDomain> Read(Guid id);
        Task<IEnumerable<ITeamDomain>> ReadAll();
        Task<int> Update(ITeamDomain entry);
        Task<IEnumerable<ITeamDomain>> GetWhereTournamentId(Guid tournamentId);
    }
}
