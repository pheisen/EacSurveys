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
    
    public partial class clp_sv_question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clp_sv_question()
        {
            this.clp_sv_question_sog = new HashSet<clp_sv_question_sog>();
            this.clp_sv_question_tag = new HashSet<clp_sv_question_tag>();
            this.clp_sv_subquestion = new HashSet<clp_sv_subquestion>();
        }
    
        public int pk1 { get; set; }
        public int clp_sv_survey_pk1 { get; set; }
        public string qtext_body { get; set; }
        public string qtext_type { get; set; }
        public int display_order { get; set; }
        public string question_type { get; set; }
        public string required_ind { get; set; }
        public string multi_line_ind { get; set; }
    
        public virtual clp_sv_survey clp_sv_survey { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clp_sv_question_sog> clp_sv_question_sog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clp_sv_question_tag> clp_sv_question_tag { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clp_sv_subquestion> clp_sv_subquestion { get; set; }
    }
}
