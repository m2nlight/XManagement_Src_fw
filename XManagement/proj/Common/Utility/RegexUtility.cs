using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BobWei.CSharp.Common.Utility
{
    /// <summary>
    /// 正则运算
    /// </summary>
    public static class RegexUtility
    {
        /// <summary>
        /// 判断一个字符串是否匹配某正则表达式
        /// </summary>
        /// <param name="input">输入一个字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>是否匹配</returns>
        public static bool RegexMatch(string input, string pattern)
        {
            var r = new Regex(pattern);
            Match m = r.Match(input);
            return m.Success;
        }

        /// <summary>
        /// 从一个字符串内得到匹配正则表达式的结果集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IList<string> RegexSearch(string input, string pattern)
        {
            IList<string> results = new List<string>();
            var r = new Regex(pattern);
            MatchCollection mc = r.Matches(input);
            foreach (Match m in mc)
            {
                if (!results.Contains(m.Value))
                {
                    results.Add(m.Value);
                }
            }
            return results;
        }

        /// <summary>
        /// 普通电话
        /// </summary>
        /// <param name="input">输入一个字符串</param>
        /// <returns>是否匹配</returns>
        public static bool RegexTelephoneNumber(string input)
        {
            return RegexMatch(input, @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
        }

        /// <summary>
        /// 移动电话
        /// </summary>
        /// <param name="input">输入一个字符串</param>
        /// <returns>是否匹配</returns>
        public static bool RegexMobilePhoneNumber(string input)
        {
            //return RegexMatch(input, @"^[+]{0,1}(\d){1,3,5,8}[ ]?([-]?((\d)|[ ]){1,12})+$");
            return RegexMatch(input, @"^(13[0-9]|15[0-9]|18[8|9|6])\d{8}$");
        }
    }
}