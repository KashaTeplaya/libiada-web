//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibiadaWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class SequenceAttribute
    {
        public long Id { get; set; }
        public long SequenceId { get; set; }
        public Attribute Attribute { get; set; }
        public string Value { get; set; }
    
        public virtual DnaSequence DnaSequence { get; set; }
        public virtual Subsequence Subsequence { get; set; }
    }
}
