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
    
    public partial class teaching_liv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public teaching_liv()
        {
            this.teaching_apply = new HashSet<teaching_apply>();
        }
    
        public int id { get; set; }
        public int specialist_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public System.DateTime created_dt { get; set; }
        public int creator_id { get; set; }
        public int view_count { get; set; }
        public int like_count { get; set; }
        public int apply_count { get; set; }
        public System.DateTime start_dt { get; set; }
        public System.DateTime end_dt { get; set; }
        public int apply_max { get; set; }
     
        public  Nullable<int> channel_id { get; set; }
        public string teacher_name { get; set; }
        public string teacher_title { get; set; }
        public string teacher_dept { get; set; }
        public string teacher_org { get; set; }
        public Nullable<int> image_id { get; set; }
    
        public virtual dic_live_channel dic_live_channel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<teaching_apply> teaching_apply { get; set; }
        public virtual user_account create_user { get; set; }
        public virtual cmn_image cmn_image { get; set; }
        public virtual user_account specialist_user { get; set; }
    }
}
