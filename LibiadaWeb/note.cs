//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibiadaWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class note
    {
        public note()
        {
            this.pitch = new HashSet<pitch>();
        }
    
        public long id { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int notation_id { get; set; }
        public System.DateTimeOffset created { get; set; }
        public int numerator { get; set; }
        public int denominator { get; set; }
        public Nullable<int> ticks { get; set; }
        public int onumerator { get; set; }
        public int odenominator { get; set; }
        public bool triplet { get; set; }
        public Nullable<int> priority { get; set; }
        public int tie_id { get; set; }
        public Nullable<System.DateTimeOffset> modified { get; set; }
    
        public virtual notation notation { get; set; }
        public virtual ICollection<pitch> pitch { get; set; }
        public virtual tie tie { get; set; }
    }
}