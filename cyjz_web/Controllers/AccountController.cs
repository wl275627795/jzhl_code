using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.CMN;
using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        /// 获取“我的”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var userid = CurrentUser == null ? 0 : CurrentUser.id;

            ViewBag.Messages = AccountMgr.GetUserMessages(userid);
            ViewBag.UserCollections = AccountMgr.GetUserCollections(userid);

            return View(CurrentUser);
        }

        #region 用户登录、注销、新注册、更改个人信息、更改密码、更改头像等

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Signin(string mobile, string password)
        {
            var user = AccountMgr.CheckUserLogin(mobile, password);
            if (user == null)
                return Content(string.Empty);

            CurrentUser = user;

            return Content(user.id.ToString());
        }

        /// <summary>
        /// 用户退出登录（注销）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Signout()
        {
            CurrentUser = null;
            var rand = new Random();
            return Content(rand.Next().ToString());
        }

        /// <summary>
        /// 显示账户个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 显示“设置头像”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SetAvatar()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 保存用户选择的头像
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public ActionResult SubmitAvatar(string img)
        {
            img += ".png"; // 暂时固定为png格式
            AccountMgr.UpdateAvatar(CurrentUser.id, img);
            CurrentUser = AccountMgr.GetUser(CurrentUser.id); // 更新session中的user对象
            return Content("OK");
        }

        /// <summary>
        /// 显示“更改密码”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 提交密码修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitPassword()
        {
            string mobile_number = Request.Form["mobile_number"];
            string old_password = Request.Form["old_password"];
            string password = Request.Form["password"];
            string password2 = Request.Form["password2"];

            var user = AccountMgr.CheckUserLogin(mobile_number, old_password);
            if (user == null)
                return Content("目前使用的密码验证失败。");

            if (string.IsNullOrEmpty(password) == true)
                return Content("新登录密码不可为空。");
            if (string.IsNullOrEmpty(password2) == true)
                return Content("重复登录密码不可为空。");
            if (password != password2)
                return Content("两次输入的新登录密码不一致。");
            if (old_password == password)
                return Content("新密码不可以与目前使用的密码相同。");

            AccountMgr.ChangePassword(user.id, password);

            return Content("OK");
        }


        /// <summary>
        /// 显示“新注册”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 提交新用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SubmitRegister([Bind] user_account user)
        {
            var password2 = Request.Form["password2"];

            // 检查必填字段是否完整
            if (string.IsNullOrEmpty(user.mobile_number) == true)
                return Content("手机号码不可为空。");
            if (string.IsNullOrEmpty(user.name) == true)
                return Content("真实姓名不可为空。");
            if (string.IsNullOrEmpty(user.password) == true)
                return Content("登录密码不可为空。");
            if (string.IsNullOrEmpty(password2) == true)
                return Content("重复登录密码不可为空。");
            if (user.password != password2)
                return Content("两次输入的登录密码不一致。");
            if (string.IsNullOrEmpty(user.hospital_name) == true)
                return Content("所在医院不可为空。");
            if (string.IsNullOrEmpty(user.department_name) == true)
                return Content("科室名称不可为空。");

            if (Checker.IsValidTelephone(user.mobile_number) == false)
                return Content("手机号码格式无效。");

            // 检查手机号码是否已被使用
            var temp = AccountMgr.GetUser(user.mobile_number);
            if (temp != null)
                return Content("手机号码(" + user.mobile_number + ")已被注册。");

            // 根据性别，设置默认头像
            if (user.gender == GenderType.男)
                user.avatar = "user.png";
            else if (user.gender == GenderType.女)
                user.avatar = "user-female.png";

            // 保存新用户登录信息
            CurrentUser = AccountMgr.InsertUser(user);

            return Content("OK");
        }

        /// <summary>
        /// 显示“忘记密码”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// 显示“重设密码”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View();
        }
        /// <summary>
        /// 提交“个人信息更新”
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult UpdateProfile([Bind] user_account user)
        {
            if (user.name !=null )
            { 
                CurrentUser = AccountMgr.UpdateUser(user);
                return Content("OK");
            }
            else return Content("姓名不可为空！");
        }
        #endregion

        #region 我的课程、病例、收藏、消息等

        /// <summary>
        /// 显示“我的课程”列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTeaching()
        {
            ViewBag.CurrentUser = CurrentUser;
            var list = AccountMgr.GetUserTeachingCollections(CurrentUser.id);
            return View(list);
        }

        /// <summary>
        /// 显示“我的收藏”列表（文章和新闻）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyFav()
        {
            var list = AccountMgr.GetUserCollections(CurrentUser.id).Where(t => t.content_type == ContentType.文章 || t.content_type == ContentType.新闻).OrderByDescending(t => t.collect_dt).ToList();

            ViewBag.ArticleList = AccountMgr.GetUserArticleCollections(CurrentUser.id);
            ViewBag.NewsList = AccountMgr.GetUserNewsCollections(CurrentUser.id);

            return View(list);
        }

        /// <summary>
        /// 显示“常用指导”（决策支持--CDS）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCds()
        {
            ViewBag.CurrentUser = CurrentUser;
            var list = AccountMgr.GetUserCdsCollections(CurrentUser.id);
            return View(list);
        }

        /// <summary>
        /// 显示“收藏病例”（来自于会诊病例）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPatientCase()
        {
            ViewBag.CurrentUser = CurrentUser;
            var list = AccountMgr.GetUserPatientCaseCollections(CurrentUser.id);
            return View(list);
        }

        /// <summary>
        /// 显示“我的消息”列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyMessages()
        {
            ViewBag.CurrentUser = CurrentUser;
            var all = AccountMgr.GetUserMessages(CurrentUser.id);
            return View(all);
        }

        /// <summary>
        /// 显示消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Message(int id)
        {
            AccountMgr.UpdateMessageStatus(id, true, null); // 点开消息详情页面，自动标记为已读
            var obj = AccountMgr.GetMessage(id);
            return View(obj);
        }

        public ActionResult SessionMessage(int sender_id)
        {
            // AccountMgr.UpdateMessageStatus(id, true, null); // 点开消息详情页面，自动标记为已读
            ViewBag.CurrentUser = CurrentUser;
            var obj = AccountMgr.GetUserSessionMessages(sender_id, CurrentUser.id);
            return View(obj);
        }

        /// <summary>
        /// 将指定的消息标记为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MessageIsRead(int id)
        {
            AccountMgr.UpdateMessageStatus(id, true, false);
            return Content("OK");
        }

        /// <summary>
        /// 将指定的消息标记为已删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MessageIsDeleted(int id)
        {
            AccountMgr.UpdateMessageStatus(id, null, true);
            return Content("OK");
        }

        /// <summary>
        /// 将当前用户的全部未读消息标记为已读
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageAllRead()
        {
            AccountMgr.MessageAllRead(CurrentUser.id);
            return Content("OK");
        }

        #endregion

        #region 个人收藏

        /// <summary>
        /// 添加到个人收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCollection(int userid, ContentType type, int contentid)
        {
            AccountMgr.AddUserCollection(userid, type, contentid);
            return Content("OK");
        }

        /// <summary>
        /// 从个人收藏中移除
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveCollection(int userid, ContentType type, int contentid)
        {
            AccountMgr.RemoveUserCollection(userid, type, contentid);
            return Content("OK");
        }

        #endregion

        /// <summary>
        /// 显示“关于”页面
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
    }
}