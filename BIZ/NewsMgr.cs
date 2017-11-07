using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 新闻
    /// </summary>
    public static class NewsMgr
    {
        public static List<news> GetNewsList()
        {
            using (var db = new RescueEntities())
            {
                return db.news.Include("news_image.cmn_image").OrderByDescending(t => t.post_dt).ToList();
            }
        }

        public static news GetNews(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.news.Include("news_comments.user_account").Include("news_image.cmn_image").FirstOrDefault(t => t.id == id);
            }
        }

        public static news UpdateNews(news obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static news InsertNews(news obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static void DeleteNews(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<news> QueryNews(string keyword, DateTime from, DateTime to, string author, string dept)
        {
            using (var db = new RescueEntities())
            {
                to = to.AddDays(1).AddSeconds(-1);
                var list = db.news.AsQueryable();
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.title.Contains(keyword));
                list = list.Where(t => t.post_dt >= from && t.post_dt <= to);
                if (string.IsNullOrEmpty(author) == false)
                    list = list.Where(t => t.author_name.Contains(author));
                if (string.IsNullOrEmpty(dept) == false)
                    list = list.Where(t => t.author_org.Contains(dept));
                return list.ToList();
            }
        }

        public static news_comments InsertNewsComments(news_comments obj)
        {
            using (var db = new RescueEntities())
            {
                db.news_comments.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeleteNewsComments(int comments_id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.news_comments.Find(comments_id);
                db.news_comments.Remove(temp);
                db.SaveChanges();
            }
        }
    }
}
