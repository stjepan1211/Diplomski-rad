using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DAL;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class MatchDomain : IMatchDomain
    {
        public System.Guid Id { get; set; }
        public System.Guid TournametId { get; set; }
        public System.Guid RefereeId { get; set; }
        public System.Guid TeamOneId { get; set; }
        public System.Guid TeamTwoId { get; set; }
        public System.DateTime DateTime { get; set; }

        public virtual IRefereeDomain Referee { get; set; }
        public virtual ITeamDomain Team { get; set; }
        public virtual ITeamDomain Team1 { get; set; }
        public virtual ICollection<IResultDomain> Results { get; set; }
    }
}
