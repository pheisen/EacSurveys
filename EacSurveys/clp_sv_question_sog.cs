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
    
    public partial class clp_sv_question_sog
    {
        public int pk1 { get; set; }
        public int clp_sv_question_pk1 { get; set; }
        public int clp_sog_pk1 { get; set; }
    
        public virtual clp_sv_question clp_sv_question { get; set; }
    }
}
