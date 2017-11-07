using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class TeachingController : BaseController
    {
        #region 直播（Teaching_Liv）


        /// <summary>
        /// 显示直播（Liv）详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LivDetail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.Comments = TeachingLivMgr.GetTeachingComments(id, 1);
            var obj = TeachingLivMgr.GetTeachingLiv(id);
            return View(obj);
        }

        /// <summary>
        /// 显示直播的已报名用户列表页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LivApplyList(int id)
        {
            var list = TeachingLivMgr.GetTeachingLivApplyList(id);
            return View(list);
        }

        /// <summary>
        /// 提交教学直播报名申请
        /// </summary>
        /// <param name="liv_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitTeachingApply(int liv_id, int user_id)
        {
            // 获得直播课程的最大报名数 和 已报名数
            //var obj = TeachingLivMgr.GetTeachingLive(liv_id);
            //int a_count = obj.apply_count;
            //int a_max = obj.apply_max;
            // 已报名数 小于 最大报名数
            //if (a_count < a_max)
           // {
                var obj1 = new teaching_apply()
                {
                    teaching_id = liv_id,
                    user_id = user_id,
                    created_dt = DateTime.Now
                };
                TeachingLivMgr.InsertTeachingApply(obj1);
                return Content("OK");

           // }
           // else return Content("KO");  // 已报名数 大于等于 最大报名数
            
        }

        /// <summary>
        /// 删除教学直播报名申请
        /// </summary>
        /// <param name="apply_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTeachingApply(int apply_id)
        {
            TeachingLivMgr.DeleteTeachingApply(apply_id);
            return Content("OK");
        }

        #endregion

        #region 视频回放、录播（Teaching_vod）


        /// <summary>
        /// 显示视频回放（Vod）详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult VodDetail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.Comments = TeachingLivMgr.GetTeachingComments(id, 0);
            if (CurrentUser != null)
                ViewBag.Collection = AccountMgr.GetUserCollection(CurrentUser.id, ContentType.回放, id);

            var obj = TeachingVodMgr.GetTeachingVod(id);
            return View(obj);

        }
        #endregion

        #region 教学（Liv&Vod）公共部分

        /// <summary>
        /// 显示包含Framework7框架的完整首页（教学为默认页面）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int id = 0)
        {
            return View();
        }

        /// <summary>
        /// 显示教学直播、回放列表
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public ActionResult TeachingList(int tab = 1)
        {
            var list = TeachingLivMgr.GetTeachingLivs();
            ViewBag.TeachingVods = TeachingVodMgr.GetTeachingVods();
            ViewBag.TabIndex = tab;
            return View(list);
        }

        /// <summary>
        /// 显示教学视频查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 提交教学视频查询操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSearch()
        {
            var title = Request.Form["title"];
            var fromDT = DateTime.Parse(Request.Form["fromDT"]);
            var toDT = DateTime.Parse(Request.Form["toDT"]);
            var teacher_name = Request.Form["teacher_name"];
            var teacher_org = Request.Form["teacher_org"];

            ViewBag.LivList = TeachingLivMgr.QueryTeachingLivs(title, fromDT, toDT, teacher_name, teacher_org);
            ViewBag.VodList = TeachingVodMgr.QueryTeachingVods(title, fromDT, toDT, teacher_name, teacher_org);

            return View("SearchResult");
        }

        /// <summary>
        /// 提交教学视频评论（Liv&Vod）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLiv"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitTeachingComments(int id, int isLiv, string content)
        {
            if (string.IsNullOrEmpty(content) == true)
                return Content("评论内容不可为空。");

            var obj = new teaching_comments()
            {
                teaching_id = id,
                is_liv = isLiv,
                comments = content,
                user_id = CurrentUser.id,
                user_name = CurrentUser.name,
                created_dt = DateTime.Now
            };
            TeachingLivMgr.InsertTeachingComments(obj);

            return Content("OK");
        }

        /// <summary>
        /// 删除教学视频评论（Liv&Vod）
        /// </summary>
        /// <param name="comments_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTeachingComments(int comments_id)
        {
            TeachingLivMgr.DeleteTeachingComments(comments_id);
            return Content("OK");
        }
        #endregion

    }
}