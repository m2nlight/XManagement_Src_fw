using System.Runtime.InteropServices;
using System.Text;

namespace BobWei.CSharp.Common.IO
{
    /// <summary>
    /// INI配置文件
    /// </summary>
    public class IniFile
    {
        /// <summary>
        /// 文件全名
        /// </summary>
        public string Path { get; set; }

        #region API

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                          int size, string filePath);

        #endregion

        #region ctor

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="path">文件全名</param>
        public IniFile(string path)
        {
            Path = path;
        }

        #endregion

        #region function

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string Read(string section, string key)
        {
            var sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, string.Empty, sb, 255, Path);
            return sb.ToString();
        }

        #endregion
    }
}