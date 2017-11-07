using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MH.CMN
{
    public abstract class Checker
    {
        /// <summary>
        /// Indicates whether the expression is a valid number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string value)
        {
            return Regex.IsMatch(value, @"^\d+$");
        }

        /// <summary>
        /// Indicates whether the expression is a valid email address
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(string value)
        {
            return Regex.IsMatch(value, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        /// Indicates whether the expression is a valid money
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidMoney(string value)
        {
            return Regex.IsMatch(value, "^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(.[0-9]{1,2})?$");
        }

        /// <summary>
        /// 验证是否为有效的电话号码。
        /// 现有手机号段:
        /// 移动：139   138   137   136   135   134   147   150   151   152   157   158    159   178  182   183   184   187   188  
        /// 联通：130   131   132   155   156   185   186   145   176  
        /// 电信：133   153   177   173   180   181   189 
        /// 虚拟运营商：170  171
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidTelephone(string value)
        {
            return Regex.IsMatch(value, @"^(0|86|17951)?(13[0-9]|15[012356789]|17[013678]|18[0-9]|14[57])[0-9]{8}$");
        }
    }
}
