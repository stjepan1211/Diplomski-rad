using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Model.Common;

namespace Tournament.Model
{
    public class AspNetRoleDomain : IAspNetRoleDomain
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<IAspNetUserDomain> AspNetUsers { get; set; }
    }
}
