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
    
    public partial class consultation_conclusion
    {
        public int id { get; set; }
        public int consultation_id { get; set; }
        public int specialist_id { get; set; }
        public int sorting_no { get; set; }
        public string conclusion { get; set; }
        public Nullable<System.DateTime> conclude_dt { get; set; }
        public int status { get; set; }
    
        public virtual consultation consultation { get; set; }
    }
}