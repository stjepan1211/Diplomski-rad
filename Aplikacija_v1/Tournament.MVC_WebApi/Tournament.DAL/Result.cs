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
    
    public partial class Result
    {
        public System.Guid Id { get; set; }
        public int TeamOneGoals { get; set; }
        public int TeamTwoGoals { get; set; }
        public System.Guid MatchId { get; set; }
    
        public virtual Match Match { get; set; }
    }
}
