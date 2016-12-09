using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class PlayerView
    {
        public System.Guid Id { get; set; }
        public System.Guid TeamId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public Nullable<int> GamesPlayed { get; set; }
    }
}