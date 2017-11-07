using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

namespace MH.CMN
{
    //用来配置权限
   public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return base.AuthorizeCore(httpContext);  
         
            if (HttpContext.Current.Request.Cookies["LogingName"] != null &&  HttpContext.Current.Request.Cookies["LogingName"].Value != "" )
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Admin/Signin");

        }
    }
}
