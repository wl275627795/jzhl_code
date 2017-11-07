using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MH.Rescue.Web.Controllers
{
    public class KioskController : BaseController
    {
        /// <summary>
        /// 显示自助问诊医生端页面（指定科室的病人列表）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (CurrentUser != null)
            {
                return View();
            }
            else
                return View("Guest"); // 未登录用户，显示guest页面
        }

        /// <summary>
        /// 显示自助问诊的用户端页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Patient()
        {
            return View();
        }

        /// <summary>
        /// 显示Guest页面，提示医生登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Guest()
        {
            return View();
        }
    }
}