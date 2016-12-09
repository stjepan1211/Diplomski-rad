using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Model.Common
{
    public interface IAspNetUserLoginDomain
    {
        string LoginProvider { get; set; }
        string ProviderKey { get; set; }
        string UserId { get; set; }

        IAspNetUserDomain AspNetUser { get; set; }
    }
}
