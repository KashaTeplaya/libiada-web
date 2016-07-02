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
    
    public partial class LiteratureSequence
    {
        public LiteratureSequence()
        {
            BinaryCharacteristic = new HashSet<BinaryCharacteristic>();
            CongenericCharacteristic = new HashSet<CongenericCharacteristic>();
            Characteristic = new HashSet<Characteristic>();
        }
    
        public long Id { get; set; }
        public int NotationId { get; set; }
        public System.DateTimeOffset Created { get; set; }
        public long MatterId { get; set; }
        public int FeatureId { get; set; }
        public Nullable<int> TranslatorId { get; set; }
        public bool Original { get; set; }
        public int LanguageId { get; set; }
        public Nullable<int> RemoteDbId { get; set; }
        public string RemoteId { get; set; }
        public System.DateTimeOffset Modified { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<BinaryCharacteristic> BinaryCharacteristic { get; set; }
        public virtual ICollection<CongenericCharacteristic> CongenericCharacteristic { get; set; }
        public virtual ICollection<Characteristic> Characteristic { get; set; }
        public virtual Language Language { get; set; }
        public virtual Matter Matter { get; set; }
        public virtual Notation Notation { get; set; }
        public virtual Feature Feature { get; set; }
        public virtual Translator Translator { get; set; }
        public virtual RemoteDb RemoteDb { get; set; }
    }
}
