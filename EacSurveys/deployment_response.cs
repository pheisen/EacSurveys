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
    
    public partial class deployment_response
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public deployment_response()
        {
            this.clp_sv_question_response = new HashSet<clp_sv_question_response>();
            this.deployment_resp_crit = new HashSet<deployment_resp_crit>();
        }
    
        public int pk1 { get; set; }
        public int deployment_pk1 { get; set; }
        public Nullable<int> materialized_list_pk1 { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> received_date { get; set; }
        public string include_in_reports { get; set; }
        public Nullable<int> user_pk1 { get; set; }
        public Nullable<int> crsmain_pk1 { get; set; }
        public string crsmain_batch_uid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clp_sv_question_response> clp_sv_question_response { get; set; }
        public virtual deployment deployment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deployment_resp_crit> deployment_resp_crit { get; set; }
    }
}
