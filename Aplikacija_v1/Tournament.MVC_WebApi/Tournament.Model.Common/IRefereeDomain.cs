using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IRefereeDomain
    {
        System.Guid Id { get; set; }
        System.Guid TournamentId { get; set; }
        string Name { get; set; }
        string Surname { get; set; }

        ICollection<IMatchDomain> Matches { get; set; }
    }
}
