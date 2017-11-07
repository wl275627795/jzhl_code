using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 学术文章
    /// </summary>
    public static class ArticleMgr
    {
        public static List<article> GetArticles()
        {
            using (var db = new RescueEntities())
            {
                return db.article.OrderByDescending(t => t.publish_dt).ToList();
            }
        }

        public static article GetArticle(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.article.Include("article_apply").Include("article_comments.user_account").Include("article_like.user_account").FirstOrDefault(t => t.id == id);
            }
        }

        public static article UpdateArticle(article obj)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static article InsertArticle(article obj)
        {
            using (var db = new RescueEntities())
            {
                db.article.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static article_apply InsertArticleApply(article_apply obj)
        {
            using (var db = new RescueEntities())
            {
                db.article_apply.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static article AuditArticle(article obj, bool isApproved)
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }

        public static void DeleteArticle(int id)
        {
            using (var db = new RescueEntities())
            {
            }
        }

        public static List<article> QueryArticles(string keyword, ArticleType? type, DateTime from, DateTime to, string author, string dept)
        {
            using (var db = new RescueEntities())
            {
                to = to.AddDays(1).AddSeconds(-1);
                var list = db.article.AsQueryable();
                if (string.IsNullOrEmpty(keyword) == false)
                    list = list.Where(t => t.title.Contains(keyword));
                if (type != null)
                    list = list.Where(t => t.type == type);
                list = list.Where(t => t.publish_dt >= from && t.publish_dt <= to);
                if (string.IsNullOrEmpty(author) == false)
                    list = list.Where(t => t.author_name.Contains(author));
                if (string.IsNullOrEmpty(dept) == false)
                    list = list.Where(t => t.author_dept.Contains(dept));
                return list.ToList();
            }
        }

        public static article_comments InsertArticleComments(article_comments obj)
        {
            using (var db = new RescueEntities())
            {
                db.article_comments.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void DeleteArticleComments(int comments_id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.article_comments.Find(comments_id);
                db.article_comments.Remove(temp);
                db.SaveChanges();
            }
        }
    }
}
