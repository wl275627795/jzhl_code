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
  
    public partial class patientcase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patientcase()
        {
            this.patientcase_conclusion = new HashSet<patientcase_conclusion>();
            this.patientcase_image = new HashSet<patientcase_image>();
        }
    
        public int id { get; set; }
        public int owner_id { get; set; }
        public string description { get; set; }
        public MH.Rescue.BIZ.GenderType gender { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string occupation { get; set; }
        public string chief_complaint { get; set; }
        public string present_illness_history { get; set; }
        public string primary_diagnosis { get; set; }
        public string treatment_plan { get; set; }
        public string past_history { get; set; }
        public string personal_history { get; set; }
        public string family_history { get; set; }
        public string allergy_history { get; set; }
        public System.DateTime created_dt { get; set; }
        public Nullable<System.DateTime> modified_dt { get; set; }
        public PatientCaseStatusType status { get; set; }
        public Nullable<System.DateTime> admission_dt { get; set; }
        public string rejected_reason { get; set; }
        public string title { get; set; }
        public string owner_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<patientcase_conclusion> patientcase_conclusion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<patientcase_image> patientcase_image { get; set; }
        public virtual user_account owner { get; set; }
    }
}
