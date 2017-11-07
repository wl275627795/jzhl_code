using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MH.Rescue.BIZ;

namespace MH.Rescue.Web
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected user_account CurrentUser
        {
            get
            {
                if (Session["CurrentUser"] != null)
                    return Session["CurrentUser"] as user_account;
                return null;
            }
            set
            {
                Session["CurrentUser"] = value;
            }
        }
    }
}