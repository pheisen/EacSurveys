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
    
    public partial class clp_sv_ac_template
    {
        public int pk1 { get; set; }
        public int clp_sv_sq_template_pk1 { get; set; }
        public int display_order { get; set; }
        public string name { get; set; }
        public Nullable<int> points { get; set; }
        public string open_ended_ind { get; set; }
        public string open_ended_text { get; set; }
    
        public virtual clp_sv_sq_template clp_sv_sq_template { get; set; }
    }
}
