using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface ILocationDomain
    {
        System.Guid Id { get; set; }
        System.Guid TournamentId { get; set; }
        string Place { get; set; }
        string Description { get; set; }
        string Longitude { get; set; }
        string Latitude { get; set; }
    }
}
