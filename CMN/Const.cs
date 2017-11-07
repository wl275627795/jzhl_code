using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MH.CMN
{
    public static class Const
    {
        // 系统查询等用途中，最小的日期(MinValue)
        public static readonly DateTime DefaultMinDateTime = new DateTime(1900, 1, 1);

        /// <summary>
        /// 枚举中“All”选项的value值。
        /// </summary>
        public static readonly int EnumOptionAllKey = -1;

        #region 默认使用的文化和语言设置

        //public static readonly string DefaultCurrentUICulture = "en";
        //public static readonly string DefaultCurrentCulture = "en-US";
        public static readonly string DefaultCurrentUICulture = "zh";
        public static readonly string DefaultCurrentCulture = "zh-CN";
        #endregion

        #region 日期、时间格式
        public static readonly string TimeFormat = "HH:mm";
        public static readonly string DateFormat = "yyyy/MM/dd";
        public static readonly string DateTimeFormat = "yyyy/MM/dd HH:mm";
        public static readonly string DateTimeSecFormat = "yyyy/MM/dd HH:mm:ss";
        #endregion
    }
}
