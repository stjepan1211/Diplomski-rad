using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IMatchDomain
    {
        System.Guid Id { get; set; }
        System.Guid TournametId { get; set; }
        System.Guid RefereeId { get; set; }
        System.Guid TeamOneId { get; set; }
        System.Guid TeamTwoId { get; set; }
        System.DateTime DateTime { get; set; }

        ICollection<IResultDomain> Results { get; set; }
    }
}
