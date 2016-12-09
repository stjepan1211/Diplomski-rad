using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class RefereeView
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<MatchView> Matches { get; set; }
    }
}