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
    
    public partial class Match
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Match()
        {
            this.Results = new HashSet<Result>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid TournametId { get; set; }
        public System.Guid RefereeId { get; set; }
        public System.Guid TeamOneId { get; set; }
        public System.Guid TeamTwoId { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> Round { get; set; }
    
        public virtual Referee Referee { get; set; }
        public virtual Team Team { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Tournament Tournament { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
