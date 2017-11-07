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
    
    public partial class emr_patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public emr_patient()
        {
            this.emr_index = new HashSet<emr_index>();
            this.consultation = new HashSet<consultation>();
        }
    
        public int user_id { get; set; }
        public string name { get; set; }
        public MH.Rescue.BIZ.GenderType gender { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public double weight { get; set; }
        public Nullable<int> avatar { get; set; }
        public Nullable<System.DateTime> created_dt { get; set; }
        public Nullable<System.DateTime> modified_dt { get; set; }
        public string phone { get; set; }
        public string emergency_contact_number { get; set; }
        public string diagnosis { get; set; }
        public string occupation { get; set; }
        public string past_history { get; set; }
        public string personal_history { get; set; }
        public string family_history { get; set; }
        public string allergy_history { get; set; }
        public int patient_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<emr_index> emr_index { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<consultation> consultation { get; set; }
    }
}