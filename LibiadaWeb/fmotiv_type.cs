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
    
    public partial class fmotiv_type
    {
        public fmotiv_type()
        {
            this.fmotiv = new HashSet<fmotiv>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<fmotiv> fmotiv { get; set; }
    }
}