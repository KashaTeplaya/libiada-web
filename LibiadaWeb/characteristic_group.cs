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
    
    public partial class characteristic_group
    {
        public characteristic_group()
        {
            this.characteristic_type = new HashSet<characteristic_type>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<characteristic_type> characteristic_type { get; set; }
    }
}