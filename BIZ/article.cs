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
    
    public partial class article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public article()
        {
            this.article_comments = new HashSet<article_comments>();
            this.article_like = new HashSet<article_like>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public MH.Rescue.BIZ.ArticleType type { get; set; }
        public string keyword { get; set; }
        public string author_name { get; set; }
        public string author_dept { get; set; }
        public string author_org { get; set; }
        public string file_type { get; set; }
        public string file_path { get; set; }
        public Nullable<int> file_size { get; set; }
        public int download_count { get; set; }
        public int like_count { get; set; }
        public Nullable<int> apply_id { get; set; }
        public System.DateTime publish_dt { get; set; }
        public string summary { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<article_comments> article_comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<article_like> article_like { get; set; }
        public virtual article_apply article_apply { get; set; }
    }
}
