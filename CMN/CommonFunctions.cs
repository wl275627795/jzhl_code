using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Reflection;

namespace MH.CMN
{
    public abstract class CommonFunctions
    {
        /// <summary>
        /// Convert from List(int) to "11,2,333,44,5"
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string IntListToStringList(List<int> list)
        {
            return ToStringList<int>(list);
        }

        /// <summary>
        /// Convert from List(T) to "11,2,333,44,5"
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToStringList<T>(List<T> list, bool withspace = false)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            string res = string.Empty;

            foreach (T item in list)
            {
                res += (res.Length == 0 ? string.Empty : "," + (withspace ? " " : "")) + item.ToString();
            }

            return res;
        }

        public static List<string> ToStringList(string strList)
        {
            if (string.IsNullOrEmpty(strList) == true)
                return new List<string>();

            var list = new List<string>();

            string[] strItems = strList.Split(',');
            foreach (var item in strItems)
                list.Add(item);
            return list;
        }

        /// <summary>
        /// Convert from "11,2,333,44,5"(string) to List(int)
        /// </summary>
        /// <param name="strList">String List</param>
        /// <returns></returns>
        public static List<int> StringListToIntList(string strList)
        {
            if (string.IsNullOrEmpty(strList) == true)
                return new List<int>();

            List<int> list = new List<int>();

            string[] strItems = strList.Split(',');

            foreach (string item in strItems)
            {
                int n = 0;
                if (int.TryParse(item, out n) == false)
                    continue;

                list.Add(n);
            }

            return list;
        }

        /// <summary>
        /// 获得较短的字符串
        /// </summary>
        /// <param name="objString">Original String object</param>
        /// <param name="maxLength">Max Length of Short String</param>
        /// <returns></returns>
        public static string GetShortString(object objString, int maxLength)
        {
            if (objString == null)
                return string.Empty;

            string str = objString.ToString();

            if (string.IsNullOrEmpty(str) == true)
                return string.Empty;

            str = str.Trim();

            if (str.Length > maxLength)
                return str.Substring(0, maxLength).Trim() + "...";

            return str;
        }

        #region 保留格式（包含HTML Tag）的文本内容截断方法

        /// <summary>
        /// 获得有限长度的包含HTML Tag的文本内容。
        /// </summary>
        /// <param name="fullHtml">全文内容</param>
        /// <param name="textLength">文字长度</param>
        /// <returns></returns>
        public static string GetSubHtmlString(string fullHtml, int textLength)
        {
            int fullLength = RemoveHtml(fullHtml).Length;
            string subString = GetPageContent("<div>" + fullHtml + "</div>", textLength, false)[0].ToString();
            int subLength = RemoveHtml(subString).Length;

            if (subLength < fullLength)
            {
                if (subString.EndsWith("</p></div>") == true)
                    return subString.Substring(0, subString.Length - 10) + "...</p></div>";
                if (subString.EndsWith("</p></div>") == true)
                    return subString.Substring(0, subString.Length - 10) + "...</p></div>";
                if (subString.EndsWith("</div>") == true)
                    return subString.Substring(0, subString.Length - 6) + "...</div>";

                return subString + "...";
            }
            return subString;
        }

        /// <summary>
        /// 内容分页
        /// </summary>
        /// <param name="strContent">要分页的字符串内容</param>
        /// <param name="intPageSize">分页大小</param>
        /// <param name="isOpen">最后一页字符小于intPageSize的1/4加到上一页</param>
        /// <returns></returns>
        public static ArrayList GetPageContent(string strContent, int intPageSize, bool isOpen)
        {
            ArrayList arrlist = new ArrayList();
            string strp = strContent;
            int num = RemoveHtml(strp.ToString()).Length; // 除html标记后的字符长度
            int bp = (intPageSize + (intPageSize / 5));

            for (int i = 0; i < ((num % bp == 0) ? (num / bp) : ((num / bp) + 1)); i++)
            {
                arrlist.Add(SubString(intPageSize, ref strp));
                num = RemoveHtml(strp.ToString()).Length;
                if (isOpen && num < (intPageSize / 4))
                {
                    // 小于分页1/4字符加到上一页
                    arrlist[arrlist.Count - 1] = arrlist[arrlist.Count - 1] + strp;
                    strp = string.Empty;
                }
                i = 0;
            }

            if (strp.Length > 0)
                arrlist.Add(strp); // 大于1/4字符 小于intPageSize 

            return arrlist;
        }

        /// <summary>
        /// &lt; 符号搜索
        /// </summary>
        /// <param name="cr"></param>
        /// <returns></returns>
        private static bool IsBegin(char cr)
        {
            return cr.Equals('<');
        }

        /// <summary>
        /// &gt; 符号搜索
        /// </summary>
        /// <param name="cr"></param>
        /// <returns></returns>
        private static bool IsEnd(char cr)
        {
            return cr.Equals('>');
        }

        /// <summary>
        /// 截取分页内容
        /// </summary>
        /// <param name="index">每页字符长度</param>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string SubString(int index, ref string str)
        {
            ArrayList arrlistB = new ArrayList();
            ArrayList arrlistE = new ArrayList();
            string strTag = string.Empty;
            char strend = '0';
            bool isBg = false;
            bool IsSuEndTag = false;

            index = Gindex(str, index);
            string substr = CutString(str, 0, index); // 截取分页长度
            string substr1 = CutString(str, index, str.Length - substr.Length); // 剩余字符
            int iof = substr.LastIndexOf("<"), iof1 = 0;

            #region 防止标记截断

            if (iof > 0) iof1 = CutString(substr, iof, substr.Length - iof).IndexOf(">"); // 标记是否被截断
            if (iof1 < 0) // 完整标记被截断，则重新截取
            {
                index = index + substr1.IndexOf(">") + 1;
                substr = CutString(str, 0, index);
                substr1 = CutString(str, index, str.Length - substr.Length);
            }

            int indexendtb = substr.LastIndexOf("</tr>");
            if (indexendtb >= 0)
            {
                substr = CutString(str, 0, indexendtb);
                substr1 = CutString(str, indexendtb, str.Length - indexendtb);
            }

            int intsubstr = substr.LastIndexOf("/>") + 1;
            int intsubstr1 = substr1.IndexOf("</");

            // <xx /> 标记与 </xx>结束标记间是否字符 
            // 如：<a href="#"><img src="abc.jpg" />文字文字文字文字</a>
            if (intsubstr >= 0 && intsubstr1 >= 0)
            {
                string substr2 = CutString(substr, intsubstr, substr.Length - intsubstr) + CutString(substr1, 0, intsubstr1);
                if (substr2.IndexOf('>') == -1 && substr2.IndexOf('<') == -1)
                {
                    substr += CutString(substr1, 0, intsubstr1);
                    substr2 = CutString(substr1, intsubstr1, substr1.Length - intsubstr1);
                    int sub2idf = substr2.IndexOf('>');
                    substr += CutString(substr2, 0, sub2idf);
                    substr1 = CutString(substr2, sub2idf, substr2.Length - sub2idf);
                }
            }
            #endregion

            #region 分析截取字符内容提取标记

            foreach (char cr in substr)
            {
                if (IsBegin(cr))
                    isBg = true;
                if (isBg)
                    strTag += cr;

                if (isBg && cr.Equals('/') && strend.Equals('<'))
                    IsSuEndTag = true;

                if (IsEnd(cr))
                {
                    if (strend.Equals('/')) //跳出 <XX />标记
                    {
                        isBg = false;
                        IsSuEndTag = false;
                        strTag = string.Empty;
                    }

                    if (isBg)
                    {
                        if (!CutString(strTag.ToLower(), 0, 3).Equals("<br"))
                        {
                            if (IsSuEndTag)
                                arrlistE.Add(strTag); // 结束标记
                            else
                                arrlistB.Add(strTag); // 开始标记
                        }
                        IsSuEndTag = false;
                        strTag = string.Empty;
                        isBg = false;
                    }
                }
                strend = cr;
            }
            #endregion

            #region 找到未关闭标记

            for (int b = 0; b < arrlistB.Count; b++)
            {
                for (int e = 0; e < arrlistE.Count; e++)
                {
                    string strb = arrlistB[b].ToString().ToLower();
                    int num = strb.IndexOf(' ');
                    if (num > 0)
                        strb = CutString(strb, 0, num) + ">";
                    if (strb.ToLower().Replace("<", "</").Equals(arrlistE[e].ToString().ToLower()))
                    {
                        arrlistB.RemoveAt(b);
                        arrlistE.RemoveAt(e);
                        b = -1;
                        break;
                    }
                }
            }
            #endregion

            #region 关闭被截断标记

            for (int i = arrlistB.Count; i > 0; i--)
            {
                string stral = arrlistB[i - 1].ToString();
                substr += (stral.IndexOf(" ") == -1 ? stral.Replace("<", "</") : CutString(stral, 0, stral.IndexOf(" ")) + "/>");
            }
            #endregion

            #region 补全上页截断的标签
            string strtag = "";
            for (int i = 0; i < arrlistB.Count; i++)
                strtag += arrlistB[i].ToString();

            #endregion

            str = strtag + substr1; // 更改原始字符串
            return substr; // 返回截取内容
        }

        /// <summary>
        /// 返回真实字符长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int Gindex(string str, int index)
        {
            bool isBg = false;
            bool isSuEndTag = false;
            bool isNbsp = false, isRnbsp = false; ;
            string strnbsp = "";
            int i = 0, c = 0;

            foreach (char cr in str)
            {
                if (!isBg && IsBegin(cr))
                {
                    isBg = true;
                    isSuEndTag = false;
                }
                if (isBg && IsEnd(cr))
                {
                    isBg = false;
                    isSuEndTag = true;
                }

                if (isSuEndTag && !isBg)
                {
                    // 不在html标记内
                    if (cr.Equals('&'))
                        isNbsp = true;

                    if (isNbsp)
                    {
                        strnbsp += cr.ToString();
                        if (strnbsp.Length > 6)
                        {
                            isNbsp = false;
                            strnbsp = string.Empty;
                        }
                        if (cr.Equals(';'))
                            isNbsp = false;
                    }
                    if (!isNbsp && !"".Equals(strnbsp))
                        isRnbsp = strnbsp.ToLower().Equals("&nbsp;");
                }

                if (isSuEndTag && !cr.Equals('n') && !cr.Equals('r') && !cr.Equals(' '))
                    c++;
                if (isRnbsp)
                {
                    c = c - 6;
                    isRnbsp = false;
                    strnbsp = string.Empty;
                }

                i++;

                if (c == index)
                    return i;
            }
            return i;
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            content = Regex.Replace(content, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "&nbsp;", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }

                if (startIndex > str.Length)
                    return string.Empty;
            }
            else
            {
                if (length < 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            try
            {
                return str.Substring(startIndex, length);
            }
            catch
            {
                return str;
            }
        }

        #endregion

        /// <summary>
        /// 尝试获得int值，失败则返回null，支持传入默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值，如果解析失败则使用该值</param>
        /// <returns></returns>
        public static int? TryGetInt(string str, int? defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(str) == true)
                return defaultValue;
            int n = 0;
            if (int.TryParse(str, out n) == false)
                return defaultValue;
            return n;
        }

        /// <summary>
        /// 尝试获得DateTime值，失败则返回null。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? TryGetDateTime(object obj)
        {
            try { return DateTime.Parse(obj.ToString()); }
            catch { return null; }
        }

        /// <summary>
        /// 尝试获得double值，失败则返回null。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? TryGetDouble(object obj)
        {
            if (obj == null)
                return null;
            if (string.IsNullOrWhiteSpace(obj.ToString()) == true)
                return null;
            double n = 0;
            if (double.TryParse(obj.ToString(), out n) == false)
                return null;
            return n;
        }

        /// <summary>
        /// 根据出生日期获得其年龄的描述文字（xxY, xxM, xxD）
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        public static string GetAgeDescrip(object birthdate)
        {
            try
            {
                DateTime temp = Convert.ToDateTime(birthdate);
                var year = DateTime.Now.Year - temp.Year;
                if (year > 0)
                    return year + "岁";
                else
                {
                    var month = DateTime.Now.Month - temp.Month;
                    if (month > 0)
                        return month + "月";
                    else
                        return (DateTime.Now - temp).TotalDays + "天";
                }
            }
            catch
            { return birthdate.ToString(); }
        }

        // 获取友好的时间范围描述
        public static string GetFriendlyDateTimeDescrip(object a, DateTime current)
        {
            var date = TryGetDateTime(a);

            if (date == null)
                return string.Empty;

            if (date.Value > current)
                return "将来";

            var range = current - date.Value;
            if (range.TotalMinutes <= 1)
                return "刚刚";
            else if (range.TotalMinutes <= 60)
                return (int)range.TotalMinutes + "分钟前";
            else if (range.TotalHours < 24)
                return (int)range.TotalHours + "小时前";
            else
                return GetDateTimeDescrip(a);
        }

        public static string GetFileSizeDescrip(object size)
        {
            var fileSize = TryGetInt(size.ToString());
            if (fileSize == null)
                return string.Empty;

            if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} 字节", fileSize);
            }
        }

        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns></returns>  
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        public static string GetTimeDescrip(object date)
        {
            try
            {
                DateTime d = DateTime.Parse(date.ToString());
                if (d.Year < 1900)
                    return string.Empty;

                return d.ToString(Const.TimeFormat);
            }
            catch
            { return string.Empty; }
        }

        public static string GetDateDescrip(object date)
        {
            try
            {
                DateTime d = DateTime.Parse(date.ToString());
                if (d.Year < 1900)
                    return string.Empty;

                return d.ToString(Const.DateFormat);
            }
            catch
            { return string.Empty; }
        }

        public static string GetDateTimeDescrip(object date)
        {
            try
            {
                DateTime d = DateTime.Parse(date.ToString());
                if (d.Year < 1900)
                    return string.Empty;

                return d.ToString(Const.DateTimeFormat);
            }
            catch
            { return string.Empty; }
        }

        public static DateTime? GetSafeDateTimeForSQLSrv(DateTime value)
        {
            if (value.Year <= 1900 || value.Year >= 2099)
                return null;
            return value;
        }

        /// <summary>
        /// 获得文件尺寸的描述文字。
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetFileSizeDescrip(long size)
        {
            if (size < 1024)
                return size + "字节";
            if (size < 1024 * 1024)
                return size / 1024 + "KB";
            else
                return size / (1024 * 1024) + "MB";
        }

        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        //把table转换成josn
        public static string DataTableToJsonWithJavaScriptSerializer(DataTable table,string T_Count)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
        
            return jsSerializer.Serialize(new { Count = T_Count, data = parentRow });
        }


        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names  
            PropertyInfo[] oProps = null;

            if (varlist == null)
                return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }

    public static class MyExtensions
    {
        /// <summary>
        /// 将DateTime格式转为安全的数据库DateTime内容，超出范围则为NULL。
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime? ToSafeDB(this DateTime datetime)
        {
            if (datetime.Year < 1900 || datetime.Year > 2999)
                return null;
            return datetime;
        }
    }




}
