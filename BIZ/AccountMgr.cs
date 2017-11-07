using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MH.CMN;

namespace MH.Rescue.BIZ
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public static class AccountMgr
    {
        #region 用户登录

        public static user_account CheckUserLogin(string mobile, string password)
        {
            using (var db = new RescueEntities())
            {
                // 获取密码散列值
                password = Cryptography.GetMD5Hash(mobile + password);

                return db.user_account.FirstOrDefault(t => t.mobile_number == mobile && t.password == password && t.status != UserStatusType.已删除);
            }
        }

        #endregion

        #region 查询、获取用户信息

        public static user_account GetUser(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.user_account.Find(id);
            }
        }

        public static user_account GetUser(string mobile_number)
        {
            using (var db = new RescueEntities())
            {
                return db.user_account.FirstOrDefault(t => t.mobile_number == mobile_number);
            }
        }

        #endregion

        #region 用户注册、审核、修改个人信息

        public static user_account InsertUser(user_account user)
        {
            using (var db = new RescueEntities())
            {
                user.password = Cryptography.GetMD5Hash(user.mobile_number + user.password);
                user.created_dt = DateTime.Now;
                db.user_account.Add(user);
                db.SaveChanges();
                return user;
            }
        }

        public static user_account UpdateUser(user_account user)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.Find(user.id);
              //  temp.password = Cryptography.GetMD5Hash(user.mobile_number + user.password);
              //  temp.mobile_number = user.mobile_number;
                temp.hospital_name = user.hospital_name;
                temp.department_name = user.department_name;
                temp.gender = user.gender;
                temp.birthday = user.birthday;
                temp.name = user.name;
                temp.title = user.title;
                temp.email = user.email;
                db.SaveChanges();
                return temp;
            }
        }

        public static void ChangePassword(int id, string newpassword)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.Find(id);
                temp.password = Cryptography.GetMD5Hash(temp.mobile_number + newpassword);
                db.SaveChanges();
            }
        }

        public static void UpdateAvatar(int id, string img)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_account.Find(id);
                temp.avatar = img;
                db.SaveChanges();
            }

        }

        #endregion

        #region 收藏内容

        public static List<user_collect_record> GetUserCollections(int userid, ContentType? type = null)
        {
            using (var db = new RescueEntities())
            {
                var list = db.user_collect_record.Where(t => t.user_id == userid);
                if (type != null)
                    list = list.Where(t => t.content_type == type);

                return list.ToList();
            }
        }

        public static List<article> GetUserArticleCollections(int userid)
        {
            using (var db = new RescueEntities())
            {
                var list = from a in db.article
                           join b in db.user_collect_record on a.id equals b.content_id
                           where b.content_type == ContentType.文章 && b.user_id == userid
                           orderby b.collect_dt descending
                           select a;
                return list.ToList();
            }
        }

        public static List<decision_support> GetUserCdsCollections(int userid)
        {
            using (var db = new RescueEntities())
            {
                var list = from a in db.decision_support
                           join b in db.user_collect_record on a.id equals b.content_id
                           where b.content_type == ContentType.指导 && b.user_id == userid
                           orderby b.collect_dt descending
                           select a;
                return list.ToList();
            }
        }

        public static List<patientcase> GetUserPatientCaseCollections(int userid)
        {
            using (var db = new RescueEntities())
            {
                var list = from a in db.patientcase
                           join b in db.user_collect_record on a.id equals b.content_id
                           where b.content_type == ContentType.病例 && b.user_id == userid
                           orderby b.collect_dt descending
                           select a;
                return list.ToList();
            }
        }

        public static List<teaching_vod> GetUserTeachingCollections(int userid)
        {
            using (var db = new RescueEntities())
            {
                var list = from a in db.teaching_vod
                           join b in db.user_collect_record on a.id equals b.content_id
                           where b.content_type == ContentType.回放 && b.user_id == userid
                           orderby b.collect_dt descending
                           select a;
                return list.ToList();
            }
        }

        public static List<news> GetUserNewsCollections(int userid)
        {
            using (var db = new RescueEntities())
            {
                var list = from a in db.news
                           join b in db.user_collect_record on a.id equals b.content_id
                           where b.content_type == ContentType.新闻 && b.user_id == userid
                           orderby b.collect_dt descending
                           select a;
                return list.ToList();
            }
        }

        public static user_collect_record GetUserCollection(int userid, ContentType type, int contentid)
        {
            using (var db = new RescueEntities())
            {
                return db.user_collect_record.FirstOrDefault(t => t.user_id == userid && t.content_type == type && t.content_id == contentid);
            }
        }

        public static void AddUserCollection(int userid, ContentType type, int contentid)
        {
            using (var db = new RescueEntities())
            {
                if (GetUserCollection(userid, type, contentid) == null)
                {
                    var obj = new user_collect_record()
                    {
                        user_id = userid,
                        content_type = type,
                        content_id = contentid,
                        collect_dt = DateTime.Now
                    };

                    db.user_collect_record.Add(obj);
                    db.SaveChanges();
                }
            }
        }

        public static void RemoveUserCollection(int userid, ContentType type, int contentid)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.user_collect_record.FirstOrDefault(t => t.user_id == userid && t.content_type == type && t.content_id == contentid);
                if (temp != null)
                {
                    db.user_collect_record.Remove(temp);
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region 消息（Message）

        //获取用户收到的消息列表
        public static List<message> GetUserMessages(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.message.Include("send_user").Include("receive_user").Where(t => t.message_receiver_id == id && t.is_deleted == false).OrderByDescending(t => t.sent_dt).ToList();
            }
        }

        //获取用户发送的消息列表
        public static List<message> GetUserSentMessages(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.message.Include("send_user").Include("receive_user").Where(t => t.message_sender_id == id && t.is_deleted == false).OrderByDescending(t => t.sent_dt).ToList();
            }
        }

        //获取用户发送和收到的消息列表
        public static List<message> GetUserAllMessages(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.message.Include("send_user").Include("receive_user").Where(t => t.message_sender_id == id && t.is_deleted == false || t.message_receiver_id == id && t.is_deleted == false).OrderByDescending(t => t.sent_dt).ToList();
            }
        }

        //获取两个用户的对话列表
        public static List<message> GetUserSessionMessages(int sender_id, int receiver_id)
        {
            using (var db = new RescueEntities())
            {
                return db.message.Include("send_user").Include("receive_user").Where(t => t.message_sender_id == sender_id &&  t.message_receiver_id == receiver_id && t.is_deleted == false || t.message_sender_id == receiver_id && t.message_receiver_id == sender_id && t.is_deleted == false).OrderByDescending(t => t.sent_dt).ToList();
            }
        }

        public static message GetMessage(int id)
        {
            using (var db = new RescueEntities())
            {
                return db.message.Include("send_user").FirstOrDefault(t => t.id == id);
            }
        }

        public static message InsertMessage(message obj)
        {
            using (var db = new RescueEntities())
            {
                obj.sent_dt = DateTime.Now;
                obj.is_deleted = false;
                obj.is_read = false;

                db.message.Add(obj);
                db.SaveChanges();
                return obj;
            }
        }

        public static void UpdateMessageStatus(int id, bool? is_read, bool? is_deleted)
        {
            using (var db = new RescueEntities())
            {
                var temp = db.message.Find(id);
                if (temp != null)
                {
                    if (is_read != null)
                        temp.is_read = is_read.Value;
                    if (is_deleted != null)
                        temp.is_deleted = is_deleted.Value;

                    db.SaveChanges();
                }
            }
        }

        public static void MessageAllRead(int userid)
        {
            using (var db = new RescueEntities())
            {
                var sql = @"UPDATE message SET is_read = 1 
                    WHERE is_deleted = 0 AND message_receiver_id = " + userid;
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        #endregion
    }
}

