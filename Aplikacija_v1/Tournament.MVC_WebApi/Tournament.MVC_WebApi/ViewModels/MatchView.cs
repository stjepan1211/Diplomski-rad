﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class MatchView
    {
        public System.Guid Id { get; set; }
        public System.Guid TournametId { get; set; }
        public System.Guid RefereeId { get; set; }
        public System.Guid TeamOneId { get; set; }
        public System.Guid TeamTwoId { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> Round { get; set; }
        public Nullable<System.Guid> Winner { get; set; }
        public Nullable<bool> Penalties { get; set; }
        public virtual ICollection<ResultView> Results { get; set; }
        public virtual RefereeView Referee { get; set; }
        public virtual TeamView Team { get; set; }
        public virtual TeamView Team1 { get; set; }
        public virtual TeamView Team2 { get; set; }
    }
}