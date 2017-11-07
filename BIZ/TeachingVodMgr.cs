using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 教学视频回放
    /// </summary>
    public static class TeachingVodMgr
    {
        public static List<teaching_vod> GetTeachingVods()
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_vod.Include("teaching_category").Include("cmn_image").OrderByDescending(t => t.created_dt).ToList();
            }
        }

        public static teaching_vod GetTeachingVod(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_vod.Include("specialist_user").Include("cmn_image").Include("teaching_category").FirstOrDefault(t => t.id == id);
            }
        }

        public static teaching_vod UpdateTeachingVod(teaching_vod obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static teaching_vod InsertTeachingVod(teaching_vod obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static void DeleteTeachingVod(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<teaching_vod> QueryTeachingVods(string keyword, DateTime from, DateTime to, string author, string dept)
        {
            using (var db = new RescueEntities())
            {
                to = to.AddDays(1).AddSeconds(-1);
                var list = db.teaching_vod.AsQueryable();
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.title.Contains(keyword));
                list = list.Where(t => t.created_dt >= from && t.created_dt <= to);
                if (string.IsNullOrEmpty(author) == false)
                    list = list.Where(t => t.teacher_name.Contains(author));
                if (string.IsNullOrEmpty(dept) == false)
                    list = list.Where(t => t.teacher_org.Contains(dept) || t.teacher_dept.Contains(dept));
                return list.ToList();
            }
        }
    }
}
