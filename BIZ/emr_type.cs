//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MH.Rescue.BIZ
{
    using System;
    using System.Collections.Generic;
    
    public partial class emr_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public emr_type()
        {
            this.emr_index = new HashSet<emr_index>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int sorting_no { get; set; }
        public int init_no { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<emr_index> emr_index { get; set; }
    }
}
