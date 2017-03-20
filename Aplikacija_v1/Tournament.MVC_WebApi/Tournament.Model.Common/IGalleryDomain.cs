using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IGalleryDomain
    {
        System.Guid Id { get; set; }
        System.Guid TournamentId { get; set; }
        string Url { get; set; }

        //ITournamentDomain Tournament { get; set; }
    }
}
