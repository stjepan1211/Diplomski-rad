using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IRefereeRepository
    {
        Task<int> Add(IRefereeDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(IRefereeDomain entity);
        Task<IRefereeDomain> Get(Guid id);
        Task<IEnumerable<IRefereeDomain>> GetAll();
        Task<int> Update(IRefereeDomain entity);
        Task<IEnumerable<IRefereeDomain>> GetRefereesByTournament(Guid tournamentId);
    }
}
