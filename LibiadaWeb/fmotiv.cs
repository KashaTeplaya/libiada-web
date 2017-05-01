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
    
    public partial class Fmotiv
    {
        public Fmotiv()
        {
            BinaryCharacteristicValue = new HashSet<BinaryCharacteristicValue>();
            CongenericCharacteristicValue = new HashSet<CongenericCharacteristicValue>();
            CharacteristicValue = new HashSet<CharacteristicValue>();
        }
    
        public long Id { get; set; }
        public Notation Notation { get; set; }
        public System.DateTimeOffset Created { get; set; }
        public long MatterId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public LibiadaMusic.ScoreModel.FmotivType FmotivType { get; set; }
        public Nullable<RemoteDb> RemoteDb { get; set; }
        public string RemoteId { get; set; }
        public System.DateTimeOffset Modified { get; set; }
    
        public virtual Matter Matter { get; set; }
        public virtual ICollection<BinaryCharacteristicValue> BinaryCharacteristicValue { get; set; }
        public virtual ICollection<CongenericCharacteristicValue> CongenericCharacteristicValue { get; set; }
        public virtual ICollection<CharacteristicValue> CharacteristicValue { get; set; }
    }
}
