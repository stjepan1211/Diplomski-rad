using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class TournamentDomain : ITournamentDomain
    {
        public System.Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ILocationDomain> Locations { get; set; }
        public virtual ICollection<IMatchDomain> Matches { get; set; }
        public virtual ICollection<IRefereeDomain> Referees { get; set; }
        public virtual ICollection<ITeamDomain> Teams { get; set; }
    }
}
