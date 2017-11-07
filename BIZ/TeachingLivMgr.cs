using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 教学视频直播
    /// </summary>
    public static class TeachingLivMgr
    {
        public static List<teaching_liv> GetTeachingLivs()
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_liv.Include("cmn_image").OrderByDescending(t => t.start_dt).ToList();
            }
        }

        public static teaching_liv GetTeachingLiv(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_liv.Include("specialist_user").Include("cmn_image").Include("teaching_apply").FirstOrDefault(t => t.id == id);
            }
        }


        public static List<teaching_apply> GetTeachingLivApplyList(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_apply.Include("user_account").Where(t => t.teaching_id == id).OrderBy(t => t.created_dt).ToList();
            }
        }

        public static teaching_liv UpdateTeachingLiv(teaching_liv obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static teaching_liv InsertTeachingLiv(teaching_liv obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static void DeleteTeachingLiv(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<teaching_liv> QueryTeachingLivs(string keyword, DateTime from, DateTime to, string author, string dept)
        {
            using (var db = new RescueEntities())
            {
                to = to.AddDays(1).AddSeconds(-1);
                var list = db.teaching_liv.AsQueryable();
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.title.Contains(keyword));
                list = list.Where(t => t.start_dt >= from && t.start_dt <= to);
                if (string.IsNullOrEmpty(author) == false)
                    list = list.Where(t => t.teacher_name.Contains(author));
                if (string.IsNullOrEmpty(dept) == false)
                    list = list.Where(t => t.teacher_org.Contains(dept) || t.teacher_dept.Contains(dept));
                return list.ToList();
            }
        }

        public static List<teaching_comments> GetTeachingComments(int id, int isLiv)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_comments.Include("user_account").Where(t => t.teaching_id == id && t.is_liv == isLiv).OrderBy(t => t.created_dt).ToList();
            }
        }

        public static teaching_comments InsertTeachingComments(teaching_comments obj)
        {
            using (var db = new RescueEntities())
            {
                db.teaching_comments.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeleteTeachingComments(int comments_id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.teaching_comments.Find(comments_id);
                db.teaching_comments.Remove(temp);
                db.SaveChanges();
            }
        }

        public static teaching_apply InsertTeachingApply(teaching_apply obj)
        {
            using (var db = new RescueEntities())
            {
                db.teaching_apply.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeleteTeachingApply(int apply_id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.teaching_apply.Find(apply_id);
                db.teaching_apply.Remove(temp);
                db.SaveChanges();
            }
        }

    }
}
