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
    
    public partial class domain_organization
    {
        public int domain_pk1 { get; set; }
        public int course_main_pk1 { get; set; }
    
        public virtual domain domain { get; set; }
    }
}
