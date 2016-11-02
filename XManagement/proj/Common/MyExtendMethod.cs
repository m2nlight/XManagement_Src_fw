using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BobWei.CSharp.Common.Security;

namespace BobWei.CSharp.Common
{
    /// <summary>
    /// 提供扩展方法的类
    /// </summary>
    public static class MyExtendMethod
    {
        #region FormatFileSizeUnit enum

        public enum FormatFileSizeUnit
        {
            Bytes = 0,
            Kb,
            Mb,
            Gb,
            Tb
        }

        #endregion

        private const char DefaultSeparator = ',';  //缺省分隔符（用于SplitIt和MergeIt方法）

        /// <summary>
        /// 比较两个数组是否相等
        /// </summary>
        /// <param name="array1">源数组</param>
        /// <param name="array2">比较数组</param>
        /// <returns></returns>
        public static bool IsEquals(this Array array1, Array array2)
        {
            //比较类型是否一样
            if (!ReferenceEquals(array1.GetType(), array2.GetType()))
            {
                return false;
            }

            //比较长度是否一样
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            //比较成员是否对应相等
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                var v1 = (ValueType) array1.GetValue(i);
                var v2 = (ValueType) array2.GetValue(i);

                if (!v1.Equals(v2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 过滤掉某些字符
        /// </summary>
        /// <param name="string"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string FilterIt(this string @string, char[] characters)
        {
            if (characters == null)
            {
                throw new ArgumentNullException("characters");
            }
            if (string.IsNullOrEmpty(@string))
            {
                return "";
            }

            var chars = new char[@string.Length];
            int i = 0;
            foreach (char @ch in @string)
            {
                if (!characters.Contains(ch))
                {
                    chars[i++] = ch;
                }
            }
            return new String(chars).Substring(0, i);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key">向量（8位字符）</param>
        /// <returns></returns>
        public static string DesEncrypt(this String text, String key)
        {
            var d = new DesEncryptor {InputString = text, EncryptKey = key};
            d.DesEncrypt();
            return d.OutString;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key">向量（8位字符）</param>
        /// <returns></returns>
        public static string DesDecrypt(this String text, String key)
        {
            var d = new DesEncryptor {InputString = text, DecryptKey = key};
            d.DesDecrypt();
            return d.OutString;
        }

        /// <summary>
        /// 分隔字符串（按缺省分隔符分隔）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitIt(this String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.SplitIt(DefaultSeparator);
        }

        /// <summary>
        /// 分隔字符串(指定一个分隔符号)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string[] SplitIt(this String text, char sperator)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.SplitIt(new[] {sperator});
        }

        /// <summary>
        /// 分隔字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sperators"></param>
        /// <returns></returns>
        public static string[] SplitIt(this String text, params char[] sperators)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            string[] splited = text.Split(sperators, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<string> q = from ss in splited
                                    let s = ss.Trim()
                                    where s.Length > 0
                                    select s;
            return q.ToArray();
        }

        /// <summary>
        /// 合并为字符串（用缺省分隔符作为合并分隔符）
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string MergeIt(this String[] stringArray)
        {
            return MergeIt(stringArray, DefaultSeparator);
        }

        /// <summary>
        /// 合并为字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string MergeIt(this String[] stringArray, char sperator)
        {
            return MergeIt(stringArray, sperator.ToString());
        }

        /// <summary>
        /// 合并为字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string MergeIt(this String[] stringArray, string sperator)
        {
            if (stringArray.Length == 0)
            {
                return "";
            }
            var sb = new StringBuilder();
            sb.Append(stringArray[0]);
            for (int i = 1; i < stringArray.Length; i++)
            {
                sb.AppendFormat("{0}{1}", sperator, stringArray[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 输出为多行字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string ToMulitLineString(this IList<String> stringArray)
        {
            if (stringArray.Count == 0)
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (string t in stringArray)
            {
                sb.AppendLine(t);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化为字符串
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static string FormatTimeSpan(this TimeSpan span)
        {
            return (span.Days != 0 ? span.Days.ToString("0") + "天" : "") +
                   span.Hours.ToString("0") + "小时" +
                   span.Minutes.ToString("00") + "分" +
                   span.Seconds.ToString("00") + "秒";
        }

        /// <summary>
        /// 将文件大小格式化为指定单位的字符串（结果不带单位符号）
        /// </summary>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string FormatFileSizeWithoutUnit(this long size, FormatFileSizeUnit unit)
        {
            string result;
            switch (unit)
            {
                case FormatFileSizeUnit.Kb:
                    {
                        double d = size/1024f;
                        result = string.Format("{0:#,##0.##}", d > 1 ? d : 1);
                    }
                    break;
                case FormatFileSizeUnit.Mb:
                    {
                        double d = size/1048576f;
                        result = string.Format("{0:#,##0.##}", d > 1 ? d : 1);
                    }
                    break;
                case FormatFileSizeUnit.Gb:
                    {
                        double d = size/1073741824f;
                        result = string.Format("{0:#,##0.##}", d > 1 ? d : 1);
                    }
                    break;
                case FormatFileSizeUnit.Tb:
                    {
                        double d = size/1099511627776f;
                        result = string.Format("{0:#,##0.##}", d > 1 ? d : 1);
                    }
                    break;
                    //case FormatFileSizeUnit.Bytes:
                default:
                    result = string.Format("{0:#,##0}", size);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 将文件大小格式化为指定单位的字符串
        /// </summary>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string FormatFileSize(this long size, FormatFileSizeUnit unit)
        {
            string result = FormatFileSizeWithoutUnit(size, unit);
            switch (unit)
            {
                case FormatFileSizeUnit.Kb:
                    result += " KB";
                    break;
                case FormatFileSizeUnit.Mb:
                    result += " MB";
                    break;
                case FormatFileSizeUnit.Gb:
                    result += " GB";
                    break;
                case FormatFileSizeUnit.Tb:
                    result += " TB";
                    break;
                    //case FormatFileSizeUnit.Bytes:
                default:
                    result += " 字节";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 将文件大小自动格式化为字符串
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FormatFileSize(this long size)
        {
            if (size < 1024) return FormatFileSize(size, FormatFileSizeUnit.Bytes);
            if (size < 1048576) return FormatFileSize(size, FormatFileSizeUnit.Kb);
            if (size < 1073741824) return FormatFileSize(size, FormatFileSizeUnit.Mb);
            if (size < 1099511627776) return FormatFileSize(size, FormatFileSizeUnit.Gb);
            return FormatFileSize(size, FormatFileSizeUnit.Tb);
        }
    }
}