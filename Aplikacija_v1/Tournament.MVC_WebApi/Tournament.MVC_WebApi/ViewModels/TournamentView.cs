﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class TournamentView
    {
        public System.Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Type { get; set; }

        public virtual ICollection<LocationView> Locations { get; set; }
        public virtual ICollection<MatchView> Matches { get; set; }
        public virtual ICollection<RefereeView> Referees { get; set; }
        public virtual ICollection<TeamView> Teams { get; set; }
    }
}