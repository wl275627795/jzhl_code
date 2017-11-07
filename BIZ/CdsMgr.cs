using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 决策支持
    /// </summary>
    public static class CdsMgr
    {
        public static List<decision_support> GetCdsList()
        {
            using (var db = new RescueEntities())
            {
                return db.decision_support.OrderBy(t => t.disease_name).ToList();
            }
        }

        public static decision_support GetCds(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.decision_support.FirstOrDefault(t => t.id == id);
            }
        }

        public static decision_support UpdateCds(decision_support obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static decision_support InsertCds(decision_support obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static void DeleteCds(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<decision_support> QueryCds(string keyword)
        {
            using (var db = new RescueEntities())
            {
                var list = db.decision_support.AsQueryable();
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.disease_name.Contains(keyword));
                return list.OrderBy(t => t.disease_name).ToList();
            }
        }
    }
}
