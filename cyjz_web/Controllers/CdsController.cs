using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class CdsController : BaseController
    {
        /// <summary>
        /// 显示决策指导列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var all = CdsMgr.GetCdsList();
            return View(all);
        }

        /// <summary>
        /// 显示决策支持详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            if (CurrentUser != null)
                ViewBag.Collection = AccountMgr.GetUserCollection(CurrentUser.id, ContentType.指导, id);
            var obj = CdsMgr.GetCds(id);
            return View(obj);
        }
    }
}