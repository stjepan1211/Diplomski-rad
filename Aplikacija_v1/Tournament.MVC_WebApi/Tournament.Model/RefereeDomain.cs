using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class RefereeDomain : IRefereeDomain
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<IMatchDomain> Matches { get; set; }
    }
}
