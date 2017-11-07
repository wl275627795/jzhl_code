using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MH.Rescue.BIZ;

namespace MH.Rescue.Web.Controllers
{
    public class PatientCaseController : BaseController
    {
        /// <summary>
        /// 显示会诊病例列表，只显示公开状态（已审核&已关闭）的项目
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            //var list = PatientCaseMgr.GetPatientCases().Where(t => t.status == PatientCaseStatusType.已审核 || t.status == PatientCaseStatusType.已关闭 || t.status == PatientCaseStatusType.待审核).ToList();
            var list = PatientCaseMgr.GetPatientCases().Where(t => t.status != PatientCaseStatusType.草稿).ToList();
            return View(list);
        }

        public ActionResult CaseList(int tab)
        {
            ViewBag.CurrentUser = CurrentUser;
            //var list = PatientCaseMgr.GetPatientCases().Where(t => t.status == PatientCaseStatusType.已审核 || t.status == PatientCaseStatusType.已关闭 || t.status == PatientCaseStatusType.待审核 ).ToList();
            var list = PatientCaseMgr.GetPatientCases().Where(t => t.status != PatientCaseStatusType.草稿).ToList();
            switch (tab)
            {
                case 1:
                    list = list.Where(t => t.owner_id == CurrentUser.id).ToList();
                    break;
                case 2:
                    list = list.Where(t => t.status == PatientCaseStatusType.已审核 || t.status == PatientCaseStatusType.已关闭).ToList();
                    list = list.Where(t => t.patientcase_conclusion.Any(c => c.user_id == CurrentUser.id)).ToList();
                    break;
                case 3:
                    list = list.Where(t => t.status == PatientCaseStatusType.已审核 || t.status == PatientCaseStatusType.已关闭).ToList();
                    break;
            }

            return View(new { List = list, Tab = tab });
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

        /// <summary>
        /// 显示新建会诊病例页面
        /// 从指定用户的草稿箱中试图获取，若无则新建（为了patientcase_id，用于提前保存附件）
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            ViewBag.CurrentUser = CurrentUser;
            var draft = PatientCaseMgr.GetDraft(CurrentUser.id);
            if (draft == null) // 尚无草稿，则创建
            {
                draft = new patientcase()
                {
                    owner_id = CurrentUser.id,
                    owner_name = CurrentUser.name,
                    title = string.Empty,
                    gender = GenderType.未知,
                    created_dt = DateTime.Now,
                    status = PatientCaseStatusType.草稿
                };
                PatientCaseMgr.InsertPatientCase(draft);
            }

            return View("Edit", draft);
        }

        /// <summary>
        /// 显示指定会诊病例的编辑界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            var obj = PatientCaseMgr.GetPatientCase(id);
            return View(obj);
        }

        /// <summary>
        /// 提交对会诊病例的编辑，支持保存草稿或是正式提交（进入待审核状态）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mode">1=存草稿；2=提交；3=更新</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitPatientCase([Bind]patientcase obj, int mode)
        {
            if (string.IsNullOrEmpty(obj.title) == true)
                return Content("会诊病例的标题不可为空。");

            obj.modified_dt = DateTime.Now;
            obj.owner_name = CurrentUser.name;

            if (mode == 1)
                obj.status = PatientCaseStatusType.草稿;
            if (mode == 2 || mode == 3)
                obj.status = PatientCaseStatusType.待审核; // 测试：自动审核通过  // for Test
            PatientCaseMgr.UpdatePatientCase(obj);

            return Content("OK");
        }

        #region 会诊病例附件照片管理

        /// <summary>
        /// 显示附件照片编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ImageEdit(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            var obj = PatientCaseMgr.GetImage(id);
            return View("ImageEdit", obj);
        }

        /// <summary>
        /// 显示添加附件照片页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ImageNew(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            return View(id);
        }

        /// <summary>
        /// 提交新的会诊病例附件照片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descrip"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddImage(int id, string descrip, HttpPostedFileBase file)
        {
            if (file == null)
                return Content("未选择图片");

           
            
            // 文件格式判断，支持三种图片格式
            /*
            var ext = System.IO.Path.GetExtension(file.FileName);
            if ((ext != ".bmp") & (ext != ".png") & (ext != ".jpg") & (ext != ".BMP") & (ext != ".PNG") & (ext != ".JPG"))
            {
                return Content("图片文件格式有误，请重新选择，支持 jpg/png/bmp 格式");
            }
            else
            { */

                // 先保存附件到服务器文件夹
                var folder = "~/FileUpload/patientcase/" + id.ToString() + "/";
                System.IO.Directory.CreateDirectory(Server.MapPath(folder));
                var filepath = Server.MapPath(folder + file.FileName);
                file.SaveAs(filepath);

                // 生成450px的缩略图
                //var ext = System.IO.Path.GetExtension(file.FileName);
                var filename = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                var ext = System.IO.Path.GetExtension(file.FileName);
                var thumbnail_filename = filename + "-small" + ext;
                var thumbnail_path = Server.MapPath(folder + thumbnail_filename);
                MH.CMN.CommonFunctions.MakeThumbnail(filepath, thumbnail_path, 450, 0, "W");

                // 再新建cmn_image记录
                var img = new cmn_image()
                {
                    path = file.FileName,
                    thumbnail = thumbnail_filename
                };
                SystemMgr.InsertCmnImage(img);

                // 再新建patientcase_image记录
                var obj = new patientcase_image()
                {
                    case_id = id,
                    image_id = img.id,
                    description = descrip
                };
                PatientCaseMgr.InsertImage(obj);

                return Content("OK");
           // }
        }

        /// <summary>
        /// 提交附件照片的描述内容修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descrip"></param>
        /// <returns></returns>
        public ActionResult SubmitImageEdit(int id, string descrip)
        {
            PatientCaseMgr.UpdateImageDescrip(id, descrip);
            return Content("OK");
        }

        /// <summary>
        /// 删除附件照片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteImage(int id)
        {
            PatientCaseMgr.DeleteImage(id);
            return Content("OK");
        }
        #endregion

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