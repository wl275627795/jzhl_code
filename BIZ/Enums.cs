using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Rescue.BIZ
{
    // 所有的业务枚举类型定义

    public enum YesNoType
    {
        否 = 0,
        是 = 1,
    }

    public enum UserRoleType
    {
        未认证医生 = 0,
        患者 = 1,
        已认证医生 = 2,
        专家 = 100,
        客服 = 190,
        管理员 = 200,
    }

    public enum GenderType
    {
        全部=00,
        未知 = 0,
        男 = 1,
        女 = 2,
    }

    public enum UserStatusType
    {
        待审核 = 0,
        已审核 = 1,
        已拒绝 = 2,
        已删除 = 9,
    }

    public enum VodVideoType
    {
        流媒体地址 = 0,
        嵌入网页代码 = 1,
    }

    public enum ArticleType
    {
        学术类 = 0,
        科普类 = 1,
    }

    public enum ArticleApplyStatusType
    {
        待审批 = 0,
        已批准 = 1,
        已拒绝 = 2,
    }

    public enum ContentType
    {
        回放 = 0,
        新闻 = 1,
        病例 = 2,
        指导 = 3,
        文章 = 4,
    }
    public enum ShareType
    {
        微信联系人 = 0,
        微信朋友圈 = 1,
        邮件 = 2,
        短信 = 3,
    }

    public enum MessageReadType
    {
        未读 = 0,
        已读 = 1,
    }

    public enum MessageDeletedType
    {
        未删除 = 0,
        已删除 = 1,
    }

    public enum PatientCaseStatusType
    {
        待审核 = 0,
        已审核 = 1,
        已拒绝 = 2,
        已关闭 = 3,
        草稿 = 4,
        疑难病例 = 5,
    }

    public enum QueryDateRangeType
    {
        全部 = 0,
        一周 = 7,
        一个月 = 30,
        半年 = 180,
        一年 = 365,
    }
}
