using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class ResultView
    {
        public Guid Id { get; set; }
        public int TeamOneGoals { get; set; }
        public int TeamTwoGoals { get; set; }
        public System.Guid MatchId { get; set; }
    }
}