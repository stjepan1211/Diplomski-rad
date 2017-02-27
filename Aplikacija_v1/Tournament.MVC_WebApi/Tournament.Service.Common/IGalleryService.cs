using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Service.Common
{
    public interface IGalleryService
    {
        Task<int> Add(IGalleryDomain entry);
        Task<int> Delete(Guid id);
        Task<int> Delete(IGalleryDomain entry);
        Task<IGalleryDomain> Read(Guid id);
        Task<IEnumerable<IGalleryDomain>> ReadAll();
        Task<int> Update(IGalleryDomain entry);
        Task<IEnumerable<IGalleryDomain>> GetWhereTournamentId(Guid tournamentId);
    }
}
