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
    
    public partial class dic_hospital
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string description { get; set; }
        public Nullable<double> longitude { get; set; }
        public Nullable<double> latitude { get; set; }
        public string level { get; set; }
        public YesNoType is_governmental { get; set; }
        public YesNoType is_specialized { get; set; }
        public int sorting_no { get; set; }
        public Nullable<int> image_id { get; set; }
        public int province_id { get; set; }
        public int county_id { get; set; }
    
        public virtual cmn_image cmn_image { get; set; }
        public virtual dic_province dic_province { get; set; }
        public virtual dic_county dic_county { get; set; }
    }
}
