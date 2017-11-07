using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MH.Rescue.Web.Controllers
{
    public class SystemController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}