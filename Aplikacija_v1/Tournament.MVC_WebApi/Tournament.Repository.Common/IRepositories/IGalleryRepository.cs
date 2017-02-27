using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Repository.Common.IRepositories
{
    public interface IGalleryRepository
    {
        Task<int> Add(IGalleryDomain entity);
        Task<int> Delete(Guid id);
        Task<int> Delete(IGalleryDomain entity);
        Task<IGalleryDomain> Get(Guid id);
        Task<IEnumerable<IGalleryDomain>> GetAll();
        Task<int> Update(IGalleryDomain entity);
        Task<IEnumerable<IGalleryDomain>> GetAllWhereTournamentId(Guid tournamentId);
   }
}
