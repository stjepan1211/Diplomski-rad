//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tournament.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gallery
    {
        public System.Guid Id { get; set; }
        public System.Guid TournamentId { get; set; }
        public string Url { get; set; }
    
        public virtual Tournament Tournament { get; set; }
    }
}