using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class GalleryView
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        //public virtual TournamentView Tournament { get; set; }
    }
}