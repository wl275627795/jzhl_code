using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class NewsController : BaseController
    {
        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var all = NewsMgr.GetNewsList();
            return View(all);
        }

        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            if (CurrentUser != null)
                ViewBag.Collection = AccountMgr.GetUserCollection(CurrentUser.id, ContentType.新闻, id);

            var obj = NewsMgr.GetNews(id);

            #region 将新闻图片列表自定义序列化为json字符串
            if (obj.news_image.Count > 0)
            {
                var json = string.Empty;
                foreach (var img in obj.news_image)
                    json += string.Format(@"{{ url: '/FileUpload/news/{0}/{1}', caption: '{2}' }}, ",
                        id.ToString(), img.cmn_image.path, img.description);
                ViewBag.ImagesJson = "[" + json + "]";
            }
            #endregion

            return View(obj);
        }

        #region 添加、删除新闻评论
        [HttpPost]
        public ActionResult SubmitNewsComments(int id, string content)
        {
            if (string.IsNullOrEmpty(content) == true)
                return Content("评论内容不可为空。");

            var obj = new news_comments()
            {
                news_id = id,
                comments = content,
                user_id = CurrentUser.id,
                user_name = CurrentUser.name,
                created_dt = DateTime.Now
            };
            NewsMgr.InsertNewsComments(obj);

            return Content("OK");
        }

        [HttpPost]
        public ActionResult DeleteNewsComments(int id, int comments_id)
        {
            NewsMgr.DeleteNewsComments(comments_id);
            return Content("OK");
        } 
        #endregion
    }
}