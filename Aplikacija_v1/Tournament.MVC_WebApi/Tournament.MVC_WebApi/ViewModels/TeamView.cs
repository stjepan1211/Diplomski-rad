using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class TeamView
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Name { get; set; }
        public Nullable<int> MatchesPlayed { get; set; }
        public Nullable<int> Won { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> NumberOfPlayers { get; set; }

        public virtual ICollection<MatchView> Matches { get; set; }
        public virtual ICollection<MatchView> Matches1 { get; set; }
        public virtual ICollection<PlayerView> Players { get; set; }
    }
}