using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class ResultDomain : IResultDomain
    {
        public Guid Id { get; set; }
        public int TeamOneGoals { get; set; }
        public int TeamTwoGoals { get; set; }
        public System.Guid MatchId { get; set; }
    }
}
