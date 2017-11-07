using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class EliteCaseController : BaseController
    {
        /// <summary>
        /// 显示会诊病例列表，只显示公开状态（已审核&已关闭）的项目
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            var list = PatientCaseMgr.GetPatientCases().Where(t => t.status == PatientCaseStatusType.疑难病例).ToList();
            return View(list);
        }

        


        /// <summary>
        /// 显示查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 提交会诊病例查询操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSearch()
        {
            var title = Request.Form["title"];
            var fromDT = DateTime.Parse(Request.Form["fromDT"]);
            var toDT = DateTime.Parse(Request.Form["toDT"]);
            var name = Request.Form["author_name"];
            var org = Request.Form["author_org"];

            var list = PatientCaseMgr.QueryPatientCases(title, fromDT, toDT, name, org);

            return View("SearchResult", list);
        }

        /// <summary>
        /// 显示病例详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            if (CurrentUser != null)
                ViewBag.Collection = AccountMgr.GetUserCollection(CurrentUser.id, ContentType.病例, id);

            var obj = PatientCaseMgr.GetPatientCase(id);

            #region 将病例图片列表自定义序列化为json字符串
            if (obj.patientcase_image.Count > 0)
            {
                var json = string.Empty;
                foreach (var img in obj.patientcase_image)
                    json += string.Format(@"{{ url: '/FileUpload/patientcase/{0}/{1}', caption: '{2}' }}, ",
                        id.ToString(), img.cmn_image.path, img.description);
                ViewBag.ImagesJson = "[" + json + "]";
            }
            #endregion

            return View(obj);
        }

        


        

        #region 讨论意见管理

        /// <summary>
        /// 提交会诊病例讨论意见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitConclusion(int id, string content)
        {
            if (string.IsNullOrEmpty(content) == true)
                return Content("会诊讨论意见内容不可为空。");

            var obj = new patientcase_conclusion()
            {
                case_id = id,
                conclusion = content,
                user_id = CurrentUser.id,
                created_dt = DateTime.Now
            };
            PatientCaseMgr.InsertConclusion(obj);

            return Content("OK");
        }

        /// <summary>
        /// 删除会诊病例讨论意见
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteConclusion(int id)
        {
            PatientCaseMgr.DeleteConclusion(id);
            return Content("OK");
        }
        #endregion
    }
}