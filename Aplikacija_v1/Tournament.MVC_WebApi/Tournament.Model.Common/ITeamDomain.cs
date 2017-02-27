using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface ITeamDomain
    {
        System.Guid Id { get; set; }
        System.Guid TournamentId { get; set; }
        string Name { get; set; }
        Nullable<int> MatchesPlayed { get; set; }
        Nullable<int> Won { get; set; }
        Nullable<int> Lost { get; set; }
        Nullable<int> NumberOfPlayers { get; set; }
        Nullable<int> NumberOfMatches { get; set; }
        Nullable<int> Draw { get; set; }
        Nullable<int> Points { get; set; }
        ICollection<IMatchDomain> Matches { get; set; }
        ICollection<IMatchDomain> Matches1 { get; set; }
        ICollection<IPlayerDomain> Players { get; set; }
    }
}
