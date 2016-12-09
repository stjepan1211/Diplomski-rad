using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IPlayerDomain
    {
        System.Guid Id { get; set; }
        System.Guid TeamId { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        int Goals { get; set; }
        int YellowCards { get; set; }
        int RedCards { get; set; }
        Nullable<int> GamesPlayed { get; set; }
    }
}
