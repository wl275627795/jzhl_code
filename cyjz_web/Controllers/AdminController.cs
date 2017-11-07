using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Text;

using MH.CMN;
using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class AdminController : BaseController
    {


        #region 管理员/专家等用户登录后台&退出登录

        public ActionResult Signin()
        {
            return View();
        }

        public string OutLogin()
        {
            HttpCookie cookieNew = new HttpCookie("LogingName");
            cookieNew.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookieNew);
            return "/admin/Signin";
        }
        //登录
        public string LoginSignin(string LoginName,string LoginPassword)
        {
            string Rt_url = "";
            user_account user = AdminMgr.GetLoin(LoginName, Cryptography.GetMD5Hash(LoginName + LoginPassword));
            if (user != null)
            {
                HttpCookie cookie = new HttpCookie("LogingName",user.account);
                Response.Cookies.Add(cookie);
                Rt_url= "/admin/index";
            }
            return Rt_url;
        }


        #endregion

        #region 用户管理
        //获取用户列表
        [MyAuthorizeAttribute]
        public ActionResult UserList(string name = null, string mobile = null, string hosp = null, string dept = null, UserStatusType? status = null, QueryDateRangeType range = QueryDateRangeType.全部,string page="1")
        {
            string Count =String.Empty;
            var list = AdminMgr.QueryUsers(name, mobile, hosp, dept, status, range,Convert.ToInt32(page) ,out Count);
            ViewBag.Count = Count;
            return View(list);
        }

      
        public ActionResult UserListtable(string name = null, string mobile = null, string hosp = null, string dept = null, UserStatusType? status = null, QueryDateRangeType range = QueryDateRangeType.全部, string page = "1")
        {
            string Count = String.Empty;
            var list = AdminMgr.QueryUsers(name, mobile, hosp, dept, status, range, Convert.ToInt32(page), out Count);
            ViewBag.Count = Count;
            return PartialView("UserListtable", list);
        }
        [MyAuthorizeAttribute]
        //查看用户详细信息
        public ActionResult UserDetail(int id)
        {
            var ud = AdminMgr.GetUserDetail(id);

            if (ud != null)
            {
                return View("UserDetail", ud);
            }
            else
            {
                return RedirectToAction("UserList");
            }
        }

        // 注册批准
        public ActionResult UserConfirm(int id)
        {
            AdminMgr.SetUserConfirm(id);
            var ud = AdminMgr.GetUserDetail(id);

            if (ud != null)
            {
                return View("UserConfirm", ud);
            }
            else
            {
                return RedirectToAction("UserList");
            }
        }


        //注册拒绝
        public ActionResult UserReject(int id)
        {
            AdminMgr.SetUserReject(id);
            var ud = AdminMgr.GetUserDetail(id);

            if (ud != null)
            {
                return View("UserReject", ud);
            }
            else
            {
                return RedirectToAction("UserList");
            }

        }

        //删除用户
        public ActionResult UserDelete(int id)
        {
            var ud = AdminMgr.GetUserDetail(id);

            if (ud != null)
            {
                AdminMgr.DeleteUser(id);
                return View("UserDelete", ud);
            }
            else
            {
                return RedirectToAction("UserList");
            }
        }



        #endregion
        [MyAuthorizeAttribute]
        #region 会诊管理
        public ActionResult CaseList(string Title, string Proposer, PatientCaseStatusType? status, string DateT, string  page)
        {
            string Count = String.Empty;
            var list = AdminMgr.QueryCaseList(Title, Proposer, GenderType.全部, status,  DateT, 1, out  Count);
            ViewBag.Count = Count;
            return View(list);
        }
        public ActionResult CaseListTable(string Title, string Proposer, GenderType Sex, PatientCaseStatusType? status, string DateT, int page)
        {
            string Count = String.Empty;
            var list = AdminMgr.QueryCaseList(Title, Proposer, Sex, status, DateT, page, out Count);
            ViewBag.Count = Count;
            return PartialView("CaseListTable", list);
        }
        [MyAuthorizeAttribute]
        //查看会诊信息
        public ActionResult CaseDetail(int id)
        {
            var cd = AdminMgr.GetCaseDetail(id);

            if (cd != null)
            {
                return View("CaseDetail", cd);
            }
            else
            {
                return RedirectToAction("CaseList");
            }

        }

        [MyAuthorizeAttribute]
        //查看会诊图片
        public ActionResult CaseImage(int id)
        {
            var list = AdminMgr.QueryCaseImageView(id);
            return View(list);

        }

        //会诊审核批准
        public ActionResult CaseConfirm(int id)
        {
            AdminMgr.SetCaseConfirm(id);
            var cd = AdminMgr.GetCaseDetail(id);

            if (cd != null)
            {
                return View("CaseConfirm", cd);
            }
            else
            {
                return RedirectToAction("CaseList");
            }
        }

        //会诊审核拒绝
        public ActionResult CaseReject(int id)
        {
            AdminMgr.SetCaseReject(id);
            var cd = AdminMgr.GetCaseDetail(id);

            if (cd != null)
            {
                return View("CaseReject", cd);
            }
            else
            {
                return RedirectToAction("CaseList");
            }

        }

        //删除会诊
        public ActionResult CaseDelete(int id)
        {
            var cd = AdminMgr.GetCaseDetail(id);

            if (cd != null)
            {
                AdminMgr.DeleteCase(id);
                return View("CaseDelete", cd);
            }
            else
            {
                return RedirectToAction("UserList");
            }
        }

        #endregion


        #region 教学回放（Vod）管理
        [MyAuthorizeAttribute]
        //获取教学回放列表
        public ActionResult VodList()
        {
            return View();
        }

        public string VodListT(string title, string Proposer, string Grade, string Unit, string Section, string Date_t, string page = "1")
        {
            string Count = String.Empty;
            var list = AdminMgr.QueryVodList(title, Proposer, Grade, Unit, Section, Date_t, Convert.ToInt32(page), out Count);
            return CommonFunctions.DataTableToJsonWithJavaScriptSerializer(list, Count);
        }
        //添加新教学回放
        public ActionResult CreateVod()
        {
            return View();
        }

        //回放提交
        [HttpPost]
        public ActionResult CreateVod(teaching_vod vod)
        {
            if (string.IsNullOrEmpty(vod.title) == true)
                return Content("视频标题不可为空!");
            //    if (string.IsNullOrEmpty(vod.video_type) == true)
            //      return Content("视频类型不可为空");

            vod.category_id = 1;
            vod.specialist_id = 1;
            vod.view_count = 0;
            vod.like_count = 0;
            vod.creator_id = 0;
            vod.created_dt = DateTime.Now;

            AdminMgr.InsertVod(vod);
            //return View();
            return Content("视频已提交!");
        }

        //查看回放详细信息
        public ActionResult VodDetail(int id)
        {
            var vd = AdminMgr.GetVodDetail(id);

            if (vd != null)
            {
                return View("VodDetail", vd);
            }
            else
            {
                return RedirectToAction("VodList");
            }
        }

        // 删除教学回放
        public ActionResult VodDelete(int id)
        {
            var ad = AdminMgr.GetVodDetail(id);

            if (ad != null)
            {
                AdminMgr.DeleteVod(id);

                return View("VodDelete", ad); ;
            }
            else
            {
                return RedirectToAction("VodList");
            }

        }

        #endregion

        #region 直播课程（Live）管理
        //获取直播课程列表
        [MyAuthorizeAttribute]
        public ActionResult LiveList()
        {
            return View();
        }

          //页面加载
        public string  LiveListT(string Title, string Speaker, string Section, string Publisher, string Channel, string Date_t, string page = "1")
        {
            
            string Count = String.Empty;
            DataTable list = AdminMgr.QueryLiveList(Title, Speaker, Section, Publisher, Channel, Date_t, Convert.ToInt32(page), out Count);
            return CommonFunctions.DataTableToJsonWithJavaScriptSerializer(list, Count);
        }
     
        //添加新直播课程
        public ActionResult CreateLive()
        {
            return View();
        }

        //直播提交
        [HttpPost]
        public ActionResult CreateLive(teaching_liv live)
        {
            if (string.IsNullOrEmpty(live.title) == true)
                return Content("课程标题不可为空!");
            //    if (string.IsNullOrEmpty(vod.video_type) == true)
            //      return Content("视频类型不可为空");

            // live.category_id = 1;
            live.specialist_id = 1;
            live.view_count = 0;
            live.like_count = 0;
            live.apply_count = 0;

            live.creator_id = 0;
            live.created_dt = DateTime.Now;

            AdminMgr.InsertLive(live);
            //return View();
            return Content("视频已提交!");
        }


        //查看直播详细信息
        public ActionResult LiveDetail(int id)
        {
            var vd = AdminMgr.GetLiveDetail(id);

            if (vd != null)
            {
                return View("LiveDetail", vd);
            }
            else
            {
                return RedirectToAction("LiveList");
            }
        }

        // 删除直播课程
        public ActionResult LiveDelete(int id)
        {
            var ad = AdminMgr.GetLiveDetail(id);

            if (ad != null)
            {
                AdminMgr.DeleteLive(id);

                return View("LiveDelete", ad); ;
            }
            else
            {
                return RedirectToAction("LiveList");
            }

        }

        #endregion

        #region 新闻（News）管理
        //获取新闻列表
        [MyAuthorizeAttribute]
        public ActionResult NewsList()
        {
            return View();
        }
        [MyAuthorizeAttribute]
        public string NewsListT(string Title, string AuthorName, string AuthorOrg, string Post_Name, string Post_Dt, string Page = "1")
        {
            string Count = String.Empty;
            DataTable list = AdminMgr.QueryNewsList(Title, AuthorName, AuthorOrg, Post_Name, Post_Dt, Convert.ToInt32(Page),
                out Count);
            return CommonFunctions.DataTableToJsonWithJavaScriptSerializer(list, Count);
        }
        //添加新闻
        public ActionResult CreateNews()
        {
            return View();
        }
        //新闻提交
        [HttpPost]
        public ActionResult CreateNews(news news1)
        {
            if (string.IsNullOrEmpty(news1.title) == true)
                return Content("新闻标题不可为空!");
            if (string.IsNullOrEmpty(news1.contents) == true)
                return Content("新闻内容不可为空");
            news1.post_user_id = 0;
            news1.post_user_name = "内容管理员";
            news1.post_dt = DateTime.Now;

            AdminMgr.InsertNews(news1);
            //return View();
            return Content("新闻已提交!");
        }

        //新闻图片
        public ActionResult CreateNewsImage(int news_id)
        {
            return View(news_id);
        }

        //提交新的新闻图片病例附件照片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descrip"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewsImage(int news_id, string descrip, HttpPostedFileBase file)
        {

            if (file == null)
                return Content("未选择图片");

            var ext = System.IO.Path.GetExtension(file.FileName);


            // 文件格式判断，支持三种图片格式
            if ((ext != ".bmp") & (ext != ".png") & (ext != ".jpg") & (ext != ".BMP") & (ext != ".PNG") & (ext != ".JPG"))
            {
                return Content("图片文件格式有误，请重新选择，支持 jpg/png/bmp 格式");
            }
            else
            {
                // 先保存附件到服务器文件夹
                var folder = "~/FileUpload/news/" + news_id.ToString() + "/";
                System.IO.Directory.CreateDirectory(Server.MapPath(folder));
                var filepath = Server.MapPath(folder + file.FileName);
                file.SaveAs(filepath);

                // 生成450px的缩略图
                //var ext = System.IO.Path.GetExtension(file.FileName);
                var filename = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                var thumbnail_filename = filename + "-small" + ext;
                var thumbnail_path = Server.MapPath(folder + thumbnail_filename);
                MH.CMN.CommonFunctions.MakeThumbnail(filepath, thumbnail_path, 450, 0, "W");

                // 新建cmn_image记录
                var img = new cmn_image()
                {
                    path = file.FileName,
                    thumbnail = thumbnail_filename
                };
                SystemMgr.InsertCmnImage(img);

                // 新建news_image记录
                var obj = new news_image()
                {
                    news_id = news_id,
                    image_id = img.id,
                    description = descrip
                };
                AdminMgr.InsertNewsImage(obj);

                return Content("新闻图片已保存");
            }
        }

        // 新闻详情
        public ActionResult NewsDetail(int id)
        {
            var nd = AdminMgr.GetNewsDetail(id);

            if (nd != null)
            {
                return View("NewsDetail", nd);
            }
            else
            {
                return RedirectToAction("NewsList");
            }
        }

        // 查看新闻图片
        public ActionResult NewsImage(int id)
        {
            var list = AdminMgr.QueryNewsImageView(id);
            return View(list);

        }


        // 删除新闻
        public ActionResult NewsDelete(int id)
        {
            var ad = AdminMgr.GetNewsDetail(id);

            if (ad != null)
            {
                AdminMgr.DeleteNews(id);
                return Content("新闻已删除!");
            }
            else
            {
                return RedirectToAction("NewsList");
            }

        }

        #endregion

        #region 决策支持（CDS）管理

        public ActionResult CdsList()
        {
            return View();
        }
        // 获取CDS列表
        [MyAuthorizeAttribute]
        public string CdsListT(string disease_name, string disease_position, string symptoms, string page="1")
        {
            string Count = String.Empty;
            DataTable list = AdminMgr.QueryCdsList(disease_name, disease_position, symptoms, Convert.ToInt32(page),out Count);
            return CommonFunctions.DataTableToJsonWithJavaScriptSerializer(list, Count);
        }
        //添加新CDS
        public ActionResult CreateCds()
        {
            return View();
        }

        //CDS提交
        [HttpPost]
        public ActionResult CreateCds(decision_support cds)
        {
            if (string.IsNullOrEmpty(cds.disease_name) == true)
                return Content("疾病名称不可为空!");
            AdminMgr.InsertCds(cds);
            //return View();
            return Content("决策指导已提交!");
        }

        // 决策支持详情
        public ActionResult CdsDetail(int id)
        {
            var nd = AdminMgr.GetCdsDetail(id);

            if (nd != null)
            {
                return View("CdsDetail", nd);
            }
            else
            {
                return RedirectToAction("CdsList");
            }
        }

        // 删除决策支持
        public ActionResult CdsDelete(int id)
        {
            var ad = AdminMgr.GetCdsDetail(id);

            if (ad != null)
            {
                AdminMgr.DeleteCds(id);
                return Content("决策指导已删除!");
            }
            else
            {
                return RedirectToAction("CdsList");
            }

        }

        #endregion

        #region 典型病例管理
        [MyAuthorizeAttribute]
        public ActionResult EliteCaseList()
        {
            return View();
        }

        #endregion


        #region 文章管理

        // 获取文章列表
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            string Count = string.Empty;
            var list = AdminMgr.QueryArticleListViewbyConditions("", 1, "", "", "00", out Count,"00");
            ViewBag.Count = Count;
            return View(list);
        }

        // 查询文章列表
        public ActionResult SearchIndex(string T_title,string H_page,string T_commit,string T_writer, string S_stutes,string T_Datetime)
        {
            string Count = string.Empty;
            var list = AdminMgr.QueryArticleListViewbyConditions(T_title,Convert.ToInt32(H_page) , T_commit, T_writer, S_stutes,out Count, T_Datetime);
            ViewBag.Count = Count;
            return View("Index", list);
        }

        //查看文章详细信息
        public ActionResult ArticleDetail(int id)
        {
            var ad = AdminMgr.GetArticleDetail(id);

            if (ad != null)
            {
                return View("ArticleDetail", ad);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }


        // 文章发布批准
        public ActionResult ArticleConfirm(int id)
        {
            AdminMgr.SetArticleConfirm(id);
            var ad = AdminMgr.GetArticleDetail(id);

            if (ad != null)
            {
                return View("ArticleConfirm", ad);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        //文章发布拒绝
        public ActionResult ArticleReject(int id)
        {
            AdminMgr.SetArticleReject(id);
            var ad = AdminMgr.GetUserDetail(id);

            if (ad != null)
            {
                return View("ArticleReject", ad);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // 文章删除
        public ActionResult ArticleDelete(int id)
        {
            var ad = AdminMgr.GetArticleDetail(id);

            if (ad != null)
            {
                AdminMgr.DeleteArticle(id);
                return View("ArticleDelete", ad);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        #endregion





        #region 系统设置
        public ActionResult SysSetup()
        {
            return View();
        }

        #endregion


    }
}