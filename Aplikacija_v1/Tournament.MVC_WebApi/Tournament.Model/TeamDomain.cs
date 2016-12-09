using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class TeamDomain : ITeamDomain
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Name { get; set; }
        public Nullable<int> MatchesPlayed { get; set; }
        public Nullable<int> Won { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> NumberOfPlayers { get; set; }

        public virtual ICollection<IMatchDomain> Matches { get; set; }
        public virtual ICollection<IMatchDomain> Matches1 { get; set; }
        public virtual ICollection<IPlayerDomain> Players { get; set; }
    }
}
