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
    
    public partial class clp_sv_subquestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clp_sv_subquestion()
        {
            this.clp_sv_answer_choice = new HashSet<clp_sv_answer_choice>();
        }
    
        public int pk1 { get; set; }
        public int clp_sv_question_pk1 { get; set; }
        public int display_order { get; set; }
        public string text { get; set; }
        public string label_type { get; set; }
        public string select_multiple_ind { get; set; }
        public string family_uid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clp_sv_answer_choice> clp_sv_answer_choice { get; set; }
        public virtual clp_sv_question clp_sv_question { get; set; }
    }
}
