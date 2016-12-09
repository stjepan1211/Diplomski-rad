using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournament.MVC_WebApi.ViewModels
{
    public class AspNetRoleView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUserView> AspNetUsers { get; set; }
    }
}