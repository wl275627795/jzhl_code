using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 系统、基础数据操作
    /// </summary>
    public static class SystemMgr
    {
        public static cmn_image InsertCmnImage(cmn_image obj)
        {
            using (var db = new RescueEntities())
            {
                db.cmn_image.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static message InsertMessage(message obj)
        {
            using (var db = new RescueEntities())
            {
                obj.is_read = false;
                obj.is_deleted = false;
                obj.sent_dt = DateTime.Now;

                db.message.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }
    }
}
