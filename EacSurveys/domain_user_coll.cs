//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EacSurveys
{
    using System;
    using System.Collections.Generic;
    
    public partial class domain_user_coll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public domain_user_coll()
        {
            this.domain_user_coll_uid = new HashSet<domain_user_coll_uid>();
        }
    
        public int domain_pk1 { get; set; }
        public int user_pk1 { get; set; }
        public int parent_domain_pk1 { get; set; }
        public Nullable<int> data_src_pk1 { get; set; }
    
        public virtual domain domain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<domain_user_coll_uid> domain_user_coll_uid { get; set; }
    }
}
