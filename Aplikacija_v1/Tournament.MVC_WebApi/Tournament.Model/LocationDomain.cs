using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class LocationDomain : ILocationDomain
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
