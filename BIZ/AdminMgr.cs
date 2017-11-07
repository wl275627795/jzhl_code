using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MH.CMN;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 管理员业
    /// </summary>
    public static class AdminMgr
    {
        #region 用户管理

        //获取用户列表
        public static List<user_account> QueryUsers(string name, string mobile , string hosp , string dept , UserStatusType? status , QueryDateRangeType range ,int page,out string Count)
        {
            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.user_account select t;
                if (!string.IsNullOrEmpty(name))
                {
                    Querysql = Querysql.Where(t => t.name.Contains(name));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    Querysql = Querysql.Where(t => t.mobile_number.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(hosp))
                {
                    Querysql = Querysql.Where(t => t.hospital_name.Contains(hosp));
                }
                if (!string.IsNullOrEmpty(dept))
                {
                    Querysql = Querysql.Where(t => t.department_name.Contains(dept));
                }
                if (status!=null && status.Value.ToString() != "全部")
                {
                    Querysql = Querysql.Where(t => t.status != UserStatusType.已删除 && t.status== status);
                }
                
                Count = Querysql.Count().ToString();
                return Querysql.OrderBy(t => t.status).Take(page * 15).Skip((page - 1) * 15).ToList();
            }
        }

        //获取用户详细信息
        public static user_account GetUserDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.user_account.FirstOrDefault(t => t.id == id);
            }
        }
        //用户注册批准
        public static user_account SetUserConfirm(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.FirstOrDefault(t => t.id == id);
                if (temp.status == 0)
                {
                    temp.status = temp.status + 1;
                    db.SaveChanges();

                }
                return temp;
            }
        }
        //用户注册拒绝
        public static user_account SetUserReject(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.FirstOrDefault(t => t.id == id);
                if (temp.status == 0)
                {
                    temp.status = temp.status + 2;
                    db.SaveChanges();

                }
                return temp;

            }
        }

        //删除用户信息 (状态设置为已删除状�?
        public static void DeleteUser(int user_id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.FirstOrDefault(t => t.id == user_id);
                if (temp.status != UserStatusType.已删除)
                {
                    temp.status = UserStatusType.已删除;
                    db.SaveChanges();
                }
            }
        }

        //用户登录
        public static user_account GetLoin(string Name,string password)
        {
            using (var db = new RescueEntities())
            {
                return db.user_account.FirstOrDefault(t => t.mobile_number == Name && t.password == password && t.role== UserRoleType.管理员);
            }
        }

        #endregion

        #region 文章管理
        //获取文章列表
        public static List<article_listview> QueryArticleListView()
        {
            using (var db = new RescueEntities())
            {
                return db.article_listview.OrderByDescending(t => t.publish_dt).ToList();
            }
        }
        //查询条件获取文章列表
        public static List<article_listview> QueryArticleListViewbyConditions(string title, int page, string commit, string writer, string stutes, out string Count,string DateT)
        {

            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.article_listview select t;
                if (!string.IsNullOrEmpty(title))
                {
                    Querysql = Querysql.Where(t => t.title.Contains(title));
                }
                if (!string.IsNullOrEmpty(commit))
                {
                    Querysql = Querysql.Where(t => t.apply_user_name.Contains(commit));
                }
                if (!string.IsNullOrEmpty(writer))
                {
                    Querysql = Querysql.Where(t => t.author_name.Contains(writer));
                }
                if (Convert.ToInt32(stutes) != 00)
                {
                    int lm_stutes = Convert.ToInt32(stutes);
                    Querysql = Querysql.Where(t => t.type == lm_stutes);
                }
                if (DateT != "00")
                {
                    string Datet=DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    switch (DateT)
                    {
                        case "0":
                            Datet = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd hh:mm:ss");
                            break;
                        case "1":
                            Datet = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd hh:mm:ss");
                            break;
                        case "2":
                            Datet= DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd hh:mm:ss");
                            break;
                    }
     
                    DateTime dt= Convert.ToDateTime(Datet);
                    Querysql = Querysql.Where(t => t.publish_dt >= dt && t.publish_dt <= DateTime.Now);
                }
                Count = Querysql.Count().ToString();  //回去行数

                return Querysql.OrderByDescending(t => t.publish_dt).Take(page * 15).Skip((page - 1) * 15).ToList();
            }
        }

        //获取文章详细信息
        public static article_listview GetArticleDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.article_listview.FirstOrDefault(t => t.apply_id == id);
            }
        }
        //文章审核通过
        public static article_apply SetArticleConfirm(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.article_apply.FirstOrDefault(t => t.id == id);
                if (temp.apply_status == 0)
                {
                    temp.apply_status = temp.apply_status + 1;
                    db.SaveChanges();

                }
                return temp;
            }
        }
        //文章审核不通过
        public static article_apply SetArticleReject(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.article_apply.FirstOrDefault(t => t.id == id);
                if (temp.apply_status == 0)
                {
                    temp.apply_status = temp.apply_status + 2;
                    db.SaveChanges();

                }
                return temp;
            }
        }

        //删除文章
        public static void DeleteArticle(int app_id)
        {

            using (var db = new RescueEntities())
            {
                var temp = db.article.FirstOrDefault(t => t.apply_id == app_id);
                //DeleteArticleFile(); //删除文章附件
                DeleteArticleComment(temp.id); // 删除文章评论
                DeleteUserCollect(ContentType.文章, temp.id); //删除用户收藏
                db.article.Remove(temp);
                db.SaveChanges();
            }
        }

        // 删除文章评论
        public static void DeleteArticleComment(int aid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.article_comments.Where(t => t.article_id == aid);
                foreach (var item in temp)
                {
                    db.article_comments.Remove(item);
                }
                db.SaveChanges();
            }
        }
        #endregion

        #region 会诊管理
        //获取会诊列表
        public static List<patientcase> QueryCaseList(string Title,string Proposer, GenderType Sex1, PatientCaseStatusType? status, string DateT, int page,out string Count)
        {
            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.patientcase select t;
                if (!string.IsNullOrEmpty(Title))
                {
                    Querysql = Querysql.Where(t => t.title == Title);
                }
                if (!string.IsNullOrEmpty(Proposer))
                {
                    Querysql = Querysql.Where(t => t.owner_name == Proposer);
                }
                if (Sex1 != GenderType.全部)
                {
                    Querysql = Querysql.Where(t => t.gender == Sex1);
                }

                if (!string.IsNullOrEmpty(status.ToString()))
                {
                    Querysql = Querysql.Where(t => t.status == status);
                }
                
                Count =Convert.ToString(Querysql.Count());
                return Querysql.OrderBy(t => t.status).Take(page * 15).Skip((page - 1) * 15).ToList().ToList();
            }
        }
        //获取会诊详细信息
        public static patientcase GetCaseDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase.FirstOrDefault(t => t.id == id);
            }
        }
        //获取会诊图片
        public static List<patientcase_imageview> QueryCaseImageView(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.patientcase_imageview.Where(t => t.case_id == id).OrderBy(t => t.sorting_no).ToList();
            }
        }
        //会诊审核通过
        public static patientcase SetCaseConfirm(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase.FirstOrDefault(t => t.id == id);
                if (temp.status == 0)
                {
                    temp.status = temp.status + 1;
                    db.SaveChanges();

                }
                return temp;

            }
        }
        //会诊审核不通过
        public static patientcase SetCaseReject(int id)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase.FirstOrDefault(t => t.id == id);
                if (temp.status == 0)
                {
                    temp.status = temp.status + 2;
                    db.SaveChanges();

                }
                return temp;
            }
        }

        //删除会诊
        public static void DeleteCase(int case_id)
        {
            // 删除会诊结论
            DeleteCaseConclusion(case_id);
            // 删除会诊图片
            DeleteCaseImage(case_id);
            // 删除用户收藏
            DeleteUserCollect(ContentType.病例, case_id);

            using (var db = new RescueEntities())
            {
                // 删除会诊
                var temp = db.patientcase.Find(case_id);
                db.patientcase.Remove(temp);
                db.SaveChanges();
            }
        }

        //删除会诊结论
        public static void DeleteCaseConclusion(int pcid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase_conclusion.Where(t => t.case_id == pcid);
                foreach (var item in temp)
                {
                    db.patientcase_conclusion.Remove(item);
                }
                db.SaveChanges();
            }
        }

        //删除会诊图片
        public static void DeleteCaseImage(int pcid)
        {
            //删除会诊图片文件
            // DeleteCaseImageFile();
            using (var db = new RescueEntities())
            {
                var temp = db.patientcase_image.Where(t => t.case_id == pcid);
                foreach (var item in temp)
                {
                    db.patientcase_image.Remove(item);
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region 教学回放管理
        //获取教学回放列表
        public static DataTable QueryVodList(string title,string Proposer,string Grade,string Unit,string Section ,string Date_t,int page ,out string Count)
        {
            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.teaching_vod select new {t.id,t.title,t.category_id,t.teacher_name,t.teacher_title,t.teacher_org,t.teacher_dept,t.creator_id,t.created_dt};
                if (!string.IsNullOrEmpty(title))
                {
                    Querysql = Querysql.Where(t => t.title.Contains(title));
                }
                if (!string.IsNullOrEmpty(Proposer))
                {
                     Querysql = Querysql.Where(t => t.teacher_name.Contains(Proposer));
                }
                if (!string.IsNullOrEmpty(Grade))
                {
                    Querysql = Querysql.Where(t => t.teacher_title.Contains(Grade));
                }
                if (!string.IsNullOrEmpty(Unit))
                {
                    Querysql = Querysql.Where(t => t.teacher_org.Contains(Unit));
                }
                if (!string.IsNullOrEmpty(Section))
                {
                    Querysql = Querysql.Where(t => t.teacher_dept.Contains(Section));
                }
                Count = Querysql.Count().ToString();
                return CommonFunctions.ToDataTable(Querysql.OrderByDescending(t => t.created_dt).Take(page * 15).Skip((page - 1) * 15).ToList());
            }
        }

        //添加教学回放
        public static teaching_vod InsertVod(teaching_vod vod)
        {
            using (var db = new RescueEntities())
            {
                db.teaching_vod.Add(vod);
                db.SaveChanges();
                return vod;
            }
        }

        //获取回放详细信息
        public static teaching_vod GetVodDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_vod.FirstOrDefault(t => t.id == id);
            }
        }

        //删除回放
        public static void DeleteVod(int Vod_id)
        {
            //删除回放评论
            DeleteVodComment(Vod_id);
            //删除回放图片
            //DeleteVodImage(Vod_id);

            // 删除用户收藏
            DeleteUserCollect(ContentType.回放, Vod_id);

            using (var db = new RescueEntities())
            {
                // 删除新闻
                var temp = db.teaching_vod.Find(Vod_id);
                db.teaching_vod.Remove(temp);
                db.SaveChanges();
            }
        }

        //删除回放评论
        public static void DeleteVodComment(int nid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.teaching_comments.Where(t => t.teaching_id == nid).Where(t => t.is_liv == 0);

                foreach (var item in temp)
                {
                    db.teaching_comments.Remove(item);
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region 直播课程管理
        //获取直播课程列表
        public static DataTable QueryLiveList(string Title,string Speaker,string Section,string Publisher,string Channel,string Date_t, int page, out string Count)
        {

            using (var db = new RescueEntities())
            {
                //主外键的表报错
                var Querysql = from t in db.teaching_liv select new  { t.id,t.title,t.teacher_name,t.teacher_title, t.teacher_org,t.teacher_dept, t.start_dt, t.end_dt, t.channel_id, t.creator_id,  t.created_dt } ;
                if (!string.IsNullOrEmpty(Title))
                {
                    Querysql = Querysql.Where(t => t.title.Contains(Title));
                }
                if (!string.IsNullOrEmpty(Speaker))
                {
                    Querysql = Querysql.Where(t => t.teacher_name.Contains(Speaker));
                }
                if (!string.IsNullOrEmpty(Section))
                {
                    Querysql = Querysql.Where(t => t.teacher_dept.Contains(Section));
                }
                if (!string.IsNullOrEmpty(Publisher))
                {
                    int int_Publisher = Convert.ToInt32(Publisher) ;
                    Querysql = Querysql.Where(t => t.creator_id == int_Publisher);
                }
                if (!string.IsNullOrEmpty(Channel) && Channel!="全部")
                {
                    int int_Channel = Convert.ToInt32(Channel);
                    Querysql = Querysql.Where(t => t.channel_id == int_Channel);
                }
                
                Count = Querysql.Count().ToString();   //获取行数
                return CommonFunctions.ToDataTable(Querysql.OrderByDescending(t => t.created_dt).Take(page * 15).Skip((page - 1) * 15).ToList())  ;
            }
                 
        }
        //添加直播课程
        public static teaching_liv InsertLive(teaching_liv live)
        {
            using (var db = new RescueEntities())
            {
                db.teaching_liv.Add(live);
                db.SaveChanges();
                return live;
            }
        }

        //获取直播详细信息
        public static teaching_liv GetLiveDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.teaching_liv.FirstOrDefault(t => t.id == id);
            }
        }

        //删除直播
        public static void DeleteLive(int Live_id)
        {
            //删除直播评论
            DeleteLiveComment(Live_id);
            //删除回放图片
            //DeleteVodImage(Vod_id);
            using (var db = new RescueEntities())
            {
                // 删除新闻
                var temp = db.teaching_liv.Find(Live_id);
                db.teaching_liv.Remove(temp);
                db.SaveChanges();
            }
        }

        //删除直播评论
        public static void DeleteLiveComment(int nid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.teaching_comments.Where(t => t.teaching_id == nid).Where(t => t.is_liv == 1);

                foreach (var item in temp)
                {
                    db.teaching_comments.Remove(item);
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region 新闻管理
        //获取新闻列表
        public static DataTable QueryNewsList(string Title,string AuthorName,string AuthorOrg,string Post_Name,string Post_Dt,int Page,out string Count)
        {
            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.news select new { t.id, t.title, t.author_name, t.author_org, t.post_user_name, t.post_dt };
                if (!string.IsNullOrEmpty(Title))
                {
                    Querysql = Querysql.Where(t => t.title.Contains(Title));
                }
                if (!string.IsNullOrEmpty(AuthorName))
                {
                    Querysql = Querysql.Where(t => t.author_name.Contains(AuthorName));
                }
                if (!string.IsNullOrEmpty(AuthorOrg))
                {
                    Querysql = Querysql.Where(t => t.author_org.Contains(AuthorOrg));
                }
                if (!string.IsNullOrEmpty(Post_Name))
                {
                    Querysql = Querysql.Where(t => t.post_user_name.Contains(Post_Name));
                }
                Count = Querysql.Count().ToString();
                return
                    CommonFunctions.ToDataTable(
                        Querysql.OrderByDescending(t => t.post_dt).Take(Page*15).Skip((Page - 1)*15));
            }
        }

        //添加新闻
        public static news InsertNews(news news)
        {
            using (var db = new RescueEntities())
            {
                db.news.Add(news);
                db.SaveChanges();
                return news;
            }
        }

        //获取新闻详细信息
        public static news GetNewsDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.news.FirstOrDefault(t => t.id == id);
            }
        }

        //获取新闻图片
        public static List<news_imageview> QueryNewsImageView(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.news_imageview.Where(t => t.news_id == id).OrderBy(t => t.sorting_no).ToList();
            }
        }

        //添加新闻图片
        public static news_image InsertNewsImage(news_image obj)
        {
            using (var db = new RescueEntities())
            {
                // 序列号自动加1
                if (db.news_image.Count(t => t.news_id == obj.news_id) > 0)
                {
                    var max = db.news_image.Where(t => t.news_id == obj.news_id).Max(t => t.sorting_no);
                    obj.sorting_no = max + 1;
                }

                db.news_image.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        //删除新闻
        public static void DeleteNews(int news_id)
        {
            //删除新闻评论
            DeleteNewsComment(news_id);
            //删除新闻图片
            DeleteNewsImage(news_id);

            //删除用户收藏
            DeleteUserCollect(ContentType.新闻, news_id);

            using (var db = new RescueEntities())
            {
                // 删除新闻
                var temp = db.news.Find(news_id);
                db.news.Remove(temp);
                db.SaveChanges();
            }
        }

        //删除新闻图片
        public static void DeleteNewsImage(int nid)
        {
            //删除新闻图片文件
            //DeleteNewsImageFile();
            using (var db = new RescueEntities())
            {
                var temp = db.news_image.Where(t => t.news_id == nid);
                foreach (var item in temp)
                {
                    db.news_image.Remove(item);
                }
                db.SaveChanges();
            }
        }
        //删除新闻评论
        public static void DeleteNewsComment(int nid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.news_comments.Where(t => t.news_id == nid);
                foreach (var item in temp)
                {
                    db.news_comments.Remove(item);
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region 决策指导管理
        //获取决策指导列表
        public static DataTable QueryCdsList(string disease_name,string disease_position,string symptoms,int page,out string Count)
        {
            using (var db = new RescueEntities())
            {
                var Querysql = from t in db.decision_support
                    select new {t.id, t.disease_name, t.disease_position, t.symptoms};
                if (!string.IsNullOrEmpty(disease_name))
                {
                    Querysql = Querysql.Where(t => t.disease_name.Contains(disease_name));
                }
                if (!string.IsNullOrEmpty(disease_position))
                {
                    Querysql = Querysql.Where(t => t.disease_position.Contains(disease_position));
                }
                if (!string.IsNullOrEmpty(symptoms))
                {
                    Querysql = Querysql.Where(t => t.symptoms.Contains(symptoms));
                }
                Count = Querysql.Count().ToString();
                return CommonFunctions.ToDataTable(Querysql.OrderBy(t => t.disease_name));
            }
        }

        //添加决策指导
        public static decision_support InsertCds(decision_support cds)
        {
            using (var db = new RescueEntities())
            {
                db.decision_support.Add(cds);
                db.SaveChanges();
                return cds;
            }
        }

        //获取cds详细信息
        public static decision_support GetCdsDetail(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.decision_support.FirstOrDefault(t => t.id == id);
            }
        }

        //删除cds
        public static void DeleteCds(int cd_id)
        {
            //删除用户收藏
            DeleteUserCollect(ContentType.指导, cd_id);

            using (var db = new RescueEntities())
            {
                var temp = db.decision_support.FirstOrDefault(t => t.id == cd_id);
                db.decision_support.Remove(temp);
                db.SaveChanges();
            }
        }

        #endregion

        #region 字典管理（省市区、医院等�?
        public static List<dic_hospital> GetHospitals()
        {
            using (var db = new RescueEntities())
            {
                return null;
            }
        }
        #endregion

        //删除用户收藏
        public static void DeleteUserCollect(ContentType ctype, int cid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_collect_record.Where(t => t.content_type == ctype && t.content_id == cid);
                foreach (var item in temp)
                {
                    db.user_collect_record.Remove(item);
                }
                db.SaveChanges();
            }
        }

    }
}
