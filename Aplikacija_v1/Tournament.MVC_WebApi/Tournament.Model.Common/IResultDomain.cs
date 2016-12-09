using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IResultDomain
    {
        Guid Id { get; set; }
        int TeamOneGoals { get; set; }
        int TeamTwoGoals { get; set; }
        System.Guid MatchId { get; set; }
    }
}
