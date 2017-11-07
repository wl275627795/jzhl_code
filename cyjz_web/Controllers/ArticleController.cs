using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class ArticleController : BaseController
    {
        /// <summary>
        /// 显示“学术”首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }

        /// <summary>
        /// 显示学术文章列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleList()
        {
            var all = ArticleMgr.GetArticles();
            return View(all);
        }

        /// <summary>
        /// 显示搜索页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 提交学术查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSearch()
        {
            var title = Request.Form["title"];
            //var type = Request.Form["type"];
            var fromDT = DateTime.Parse(Request.Form["fromDT"]);
            var toDT = DateTime.Parse(Request.Form["toDT"]);
            var author = Request.Form["author"];
            var author_dept = Request.Form["author_dept"];

            ArticleType? artile_type = null;
            //if (type == "1")
            //    artile_type = ArticleType.学术类;
            //else if (type == "2")
            //    artile_type = ArticleType.科普类;

            //ViewBag.Article_List = ArticleMgr.QueryArticles(title, artile_type, fromDT, toDT, author, author_dept);

            ViewBag.News_List = NewsMgr.QueryNews(title, fromDT, toDT, author, author_dept);

            if (string.IsNullOrEmpty(author) == true
                && string.IsNullOrEmpty(author_dept) == true)
                ViewBag.Cds_List = CdsMgr.QueryCds(title);

            return View("SearchResult");
        }

        /// <summary>
        /// 显示学术文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            if (CurrentUser != null)
                ViewBag.Collection = AccountMgr.GetUserCollection(CurrentUser.id, ContentType.文章, id);
            var obj = ArticleMgr.GetArticle(id);
            return View("Detail", obj);
        }

        /// <summary>
        /// 显示新文章编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }

        /// <summary>
        /// 提交新文章，包括附件
        /// </summary>
        /// <param name="article"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitArticle([Bind]article article, HttpPostedFileBase file)
        {
            // 判断 文章标题是否为空
            if (string.IsNullOrEmpty(article.title) == true)
            {
                return Content("文章标题不可为空");
            }

            // 判断 是否选择了文章附件
            else if (file == null)
            {
                return Content("未选择文章附件");
            }

            else
            {
                // 判断文章的文件类型，支持 pdf/caj/doc/docx 格式  //by XW
                /*
                var ext = System.IO.Path.GetExtension(file.FileName);
                if ((ext != ".pdf") & (ext != ".caj") & (ext != ".doc") & (ext != ".docx"))
                {
                    return Content("文章附件格式有误，请重新选择，支持 pdf/caj/doc/docx 格式");
                }
                */

                // 判断文章的文件大小，支持小于20MB的
                /*var filesize = file.ContentLength;
                if (filesize >= 20971520)
                {
                    return Content("文章附件过大，请选择20MB以下的文件");
                }*/

                // 先添加发表文章申请
                var apply = new article_apply()
                {
                    apply_user_id = CurrentUser.id,
                    apply_user_name = CurrentUser.name,
                    apply_status = ArticleApplyStatusType.待审批, // 设置为待审批状态 by XW
                    apply_dt = DateTime.Now,
                };
                apply = ArticleMgr.InsertArticleApply(apply);

                // 记录申请信息
                article.apply_id = apply.id;

                article.publish_dt = DateTime.Now; // 只是为了避免数据库错误

                // 处理附件信息
                article.file_path = file.FileName;
                article.file_size = file.ContentLength;
                article.file_type = file.ContentType;
               

                ArticleMgr.InsertArticle(article); // 保存文章记录，为了获取id

                // 保存附件到服务器文件夹
                var folder = "~/FileUpload/article/" + article.id.ToString() + "/";
                System.IO.Directory.CreateDirectory(Server.MapPath(folder));
                file.SaveAs(Server.MapPath(folder + file.FileName));

                return Content("OK");
            }
        }

        /// <summary>
        /// 提交文章评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitArticleComments(int id, string content)
        {
            if (string.IsNullOrEmpty(content) == true)
                return Content("评论内容不可为空。");

            var obj = new article_comments()
            {
                article_id = id,
                comments = content,
                user_id = CurrentUser.id,
                user_name = CurrentUser.name,
                created_dt = DateTime.Now
            };
            ArticleMgr.InsertArticleComments(obj);

            #region 发送消息通知给相关用户（apply user和comments user）
            var temp_user_list = new List<int>();

            var article = ArticleMgr.GetArticle(id);
            if (CurrentUser.id != article.article_apply.apply_user_id) // 不应该通知本人
            {
                var msg = new message()
                {
                    title = "文章讨论",
                    message_content = string.Format("我回复了您发布的文章《{0}》：{1}", article.title, content),
                    message_sender_id = CurrentUser.id,
                    message_sender_name = CurrentUser.name,
                    message_receiver_id = article.article_apply.apply_user_id,
                    message_receiver_name = article.article_apply.apply_user_name,
                };
                SystemMgr.InsertMessage(msg);
                temp_user_list.Add(msg.message_receiver_id);
            }
            foreach (var comments in article.article_comments.Where(t => t.user_id != CurrentUser.id)) // 通知其他用户
            {
                if (temp_user_list.Contains(comments.user_id) == true) // 不重复通知同一个用户（回复多次的）
                    continue;

                var msg2 = new message()
                {
                    title = "文章讨论",
                    message_content = string.Format("我回复您参与讨论的文章《{0}》：{1}", article.title, content),
                    message_sender_id = CurrentUser.id,
                    message_sender_name = CurrentUser.name,
                    message_receiver_id = comments.user_id,
                    message_receiver_name = comments.user_name,
                };
                SystemMgr.InsertMessage(msg2);
                temp_user_list.Add(msg2.message_receiver_id);
            }
            #endregion

            return Content("OK");
        }

        /// <summary>
        /// 删除文章评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comments_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteArticleComments(int id, int comments_id)
        {
            ArticleMgr.DeleteArticleComments(comments_id);
            return Content("OK");
        }
    }
}