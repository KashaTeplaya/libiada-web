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
    
    public partial class Gene
    {
        public Gene()
        {
            this.Piece = new HashSet<Piece>();
        }
    
        public long Id { get; set; }
        public Nullable<System.DateTimeOffset> Created { get; set; }
        public Nullable<System.DateTimeOffset> Modified { get; set; }
        public long SequenceId { get; set; }
        public int PieceTypeId { get; set; }
        public string Description { get; set; }
        public Nullable<int> WebApiId { get; set; }
        public bool Complementary { get; set; }
        public bool Partial { get; set; }
        public Nullable<int> ProductId { get; set; }
    
        public virtual DnaSequence DnaSequence { get; set; }
        public virtual PieceType PieceType { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Piece> Piece { get; set; }
    }
}
