using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class LocationView
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}