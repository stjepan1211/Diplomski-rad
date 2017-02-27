using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class GalleryDomain : IGalleryDomain
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Url { get; set; }

        public virtual ITournamentDomain Tournament { get; set; }
    }
}
