using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 会诊病例
    /// </summary>
    public static class PatientCaseMgr
    {
        public static List<patientcase> GetPatientCases()
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase.Include("owner").Include("patientcase_image.cmn_image").Include("patientcase_conclusion.user_account").OrderByDescending(t => t.admission_dt).ThenByDescending(t => t.created_dt).ToList();
            }
        }

        public static patientcase GetPatientCase(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase.Include("owner").Include("patientcase_image.cmn_image").Include("patientcase_conclusion.user_account").FirstOrDefault(t => t.id == id);
            }
        }

        public static patientcase_image GetImage(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase_image.Include("cmn_image").FirstOrDefault(t => t.id == id);
            }
        }

        public static patientcase_image InsertImage(patientcase_image obj)
        {
            using (var db = new RescueEntities())
            {
                // 序列号自动加1
                if (db.patientcase_image.Count(t => t.case_id == obj.case_id) > 0)
                {
                    var max = db.patientcase_image.Where(t => t.case_id == obj.case_id).Max(t => t.sorting_no);
                    obj.sorting_no = max + 1;
                }

                db.patientcase_image.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void UpdateImageDescrip(int id, string descrip)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase_image.Find(id);
                temp.description = descrip;
                db.SaveChanges();
            }
        }

        public static void DeleteImage(int id)
        {
            using (var db = new RescueEntities())
            {
                // 同时删除patientcase_image和cmn_image记录
                var temp = db.patientcase_image.Find(id);
                var cmn = db.cmn_image.Find(temp.image_id);
                db.cmn_image.Remove(cmn);
                db.patientcase_image.Remove(temp);
                db.SaveChanges();
            }
        }

        public static patientcase GetDraft(int user_id)
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase.Include("owner").Include("patientcase_image.cmn_image").Include("patientcase_conclusion.user_account").FirstOrDefault(t => t.owner_id == user_id && t.status == PatientCaseStatusType.草稿);
            }
        }

        public static patientcase UpdatePatientCase(patientcase obj)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase.Find(obj.id);
                temp.title = obj.title;
                temp.description = obj.description;
                temp.gender = obj.gender;
                temp.birthday = obj.birthday;
                temp.weight = obj.weight;
                temp.height = obj.height;
                temp.occupation = obj.occupation;
                temp.chief_complaint = obj.chief_complaint;
                temp.present_illness_history = obj.present_illness_history;
                temp.primary_diagnosis = obj.primary_diagnosis;
                temp.treatment_plan = obj.treatment_plan;
                temp.past_history = obj.past_history;
                temp.personal_history = obj.personal_history;
                temp.family_history = obj.family_history;
                temp.allergy_history = obj.allergy_history;
                temp.status = obj.status;
                temp.admission_dt = obj.admission_dt;
                temp.rejected_reason = obj.rejected_reason;
                temp.modified_dt = obj.modified_dt;
                temp.owner_id = obj.owner_id;
                temp.owner_name = obj.owner_name;

                db.SaveChanges();
                return temp;
            }
        }

        public static patientcase InsertPatientCase(patientcase obj)
        {
            using (var db = new RescueEntities())
            {
                db.patientcase.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeletePatientCase(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<patientcase> QueryPatientCases(string keyword, DateTime from, DateTime to, string author, string dept)
        {
            using (var db = new RescueEntities())
            {
                to = to.AddDays(1).AddSeconds(-1);
                var list = db.patientcase.Where(t => t.status == PatientCaseStatusType.已审核 || t.status == PatientCaseStatusType.已关闭).AsQueryable(); // 只返回公开可见状态的项目
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.title.Contains(keyword));
                list = list.Where(t => t.created_dt >= from && t.created_dt <= to);
                if (string.IsNullOrEmpty(author) == false)
                    list = list.Where(t => t.owner.name.Contains(author));
                if (string.IsNullOrEmpty(dept) == false)
                    list = list.Where(t => t.owner.hospital_name.Contains(dept) || t.owner.department_name.Contains(dept));
                return list.ToList();
            }
        }

        public static patientcase_conclusion InsertConclusion(patientcase_conclusion obj)
        {
            using (var db = new RescueEntities())
            {
                db.patientcase_conclusion.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeleteConclusion(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase_conclusion.Find(id);
                db.patientcase_conclusion.Remove(temp);
                db.SaveChanges();
            }
        }
    }
}
