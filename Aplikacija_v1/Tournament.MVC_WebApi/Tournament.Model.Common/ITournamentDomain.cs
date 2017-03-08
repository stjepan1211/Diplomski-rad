using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface ITournamentDomain
    {
        System.Guid Id { get; set; }
        string AspNetUserId { get; set; }
        System.DateTime StartTime { get; set; }
        System.DateTime EndTime { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        Nullable<int> NumberOfMatches { get; set; }
        Nullable<int> NumberOfTeams { get; set; }
        Nullable<int> Rounds { get; set; }
        Nullable<bool> IsScheduled { get; set; }

        ICollection<IGalleryDomain> Galleries { get; set; }
        ICollection<ILocationDomain> Locations { get; set; }
        ICollection<IMatchDomain> Matches { get; set; }
        ICollection<IRefereeDomain> Referees { get; set; }
        ICollection<ITeamDomain> Teams { get; set; }
    }
}
