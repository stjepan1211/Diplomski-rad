using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Token
{
    public class TokenResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public Token Token { get; set; }
    }
}
