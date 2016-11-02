using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace BobWei.CSharp.Common.Utility
{
    /// <summary>
    /// 文件IO
    /// </summary>
    public static class FileUtility
    {
        #region Delegates

        public delegate void FileOperationDelegate();

        public delegate void NamedFileOperationDelegate(string fileName);

        #endregion

        private const string FileNameRegEx = @"^([a-zA-Z]:)?[^:]+$";
        private const string Prefix = "CM$";

//        private static readonly char[] Separators = {
//                                                        Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar,
//                                                        Path.VolumeSeparatorChar
//                                                    };

        public static readonly int MaxPathLength = 260;
        public static string ApplicationRootPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 临时目录前缀
        /// </summary>
        public static string TempPrefix
        {
            get { return Prefix; }
        }

        /// <summary>
        /// 创建临时目录
        /// </summary>
        /// <returns></returns>
        public static string CreateTempPath()
        {
            string tempPath = Path.GetTempPath();
            string tempDirectory = Path.Combine(tempPath, Path.GetRandomFileName());
            while (Directory.Exists(tempDirectory))
            {
                tempDirectory = Path.Combine(tempPath, Path.GetRandomFileName());
            }
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        /// 创建临时目录(带前缀的)
        /// </summary>
        /// <returns></returns>
        public static string CreateTempPathWithPrefix(string prefix)
        {
            string tempPath = Path.GetTempPath();
            string tempDirectory = Path.Combine(tempPath, prefix + Path.GetRandomFileName());
            while (Directory.Exists(tempDirectory))
            {
                tempDirectory = Path.Combine(tempPath, prefix + Path.GetRandomFileName());
            }
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        /// 创建临时目录(带前缀的)
        /// </summary>
        /// <returns></returns>
        public static string CreateTempPathWithPrefix()
        {
            return CreateTempPathWithPrefix(Prefix);
        }

        /// <summary>
        /// 删除临时目录(带前缀的)
        /// </summary>
        public static void DeleteTempPathWithPrefix()
        {
            DeleteTempPathWithPrefix(Prefix);
        }

        /// <summary>
        /// 删除临时目录(带前缀的)
        /// </summary>
        /// <param name="prefix"></param>
        public static void DeleteTempPathWithPrefix(string prefix)
        {
            try
            {
                string tempPath = Path.GetTempPath();
                string[] dirs = Directory.GetDirectories(tempPath, prefix + "*");
                foreach (string dir in dirs)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (IOException)
                    {
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var source = new DirectoryInfo(sourceDirectory);
            var target = new DirectoryInfo(targetDirectory);

            //Determine whether the source directory exists.
            if (!source.Exists)
                return;
            if (!target.Exists)
                target.Create();

            //Copy files.
            FileInfo[] sourceFiles = source.GetFiles();
            foreach (FileInfo t in sourceFiles)
                File.Copy(t.FullName, target.FullName + "\\" + t.Name, true);

            //Copy directories.
            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            foreach (DirectoryInfo t in sourceDirectories)
                CopyDirectory(t.FullName, target.FullName + "\\" + t.Name);
        }

        /// <summary>
        /// 是否为空目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directory)
        {
            var di = new DirectoryInfo(directory);
            return di.Exists && di.GetFiles().Length == 0 && di.GetDirectories().Length == 0;
        }

        /// <summary>
        /// 得到文件基本信息
        /// </summary>
        /// <param name="filename">文件全路径名</param>
        /// <returns></returns>
        public static FileInformation GetFileInformation(string filename)
        {
            var info = new FileInformation();
            if (File.Exists(filename))
            {
                var fi = new FileInfo(filename);
                info.Filename = fi.Name;
                info.Filesize = fi.Length;
                info.LastWriteTime = fi.LastWriteTime;
                info.LastAccessTime = fi.LastAccessTime;
                info.CreationTime = fi.CreationTime;
                Icon largeIcon, smallIcon;
                string description;
                GetExtsIconAndDescription(fi.Extension, out largeIcon, out smallIcon, out description);
                info.LargeIcon = largeIcon;
                info.SmallIcon = smallIcon;
                info.Filetype = description;
            }
            return info;
        }

        /// <summary>
        /// 判断一个路径是否为类似 C: 或者 C:\Windows\System32 这样的有盘符名称的格式。
        /// 注意fileName不允许用双引号括上。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsValidAbsolutePath(string fileName)
        {
            return IsValidPath(fileName) && RegexUtility.RegexMatch(fileName, @"^[a-zA-Z]:(\\[^\\\/\?\""\<\>\|]+)*\\?$");
        }

        /// <summary>
        /// 检查文件名是否有效的代码
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsValidPath(string fileName)
        {
            // Fixme: 260 is the hardcoded maximal length for a path on my Windows XP system
            //        I can't find a .NET property or method for determining this variable.

            if (string.IsNullOrEmpty(fileName) || fileName.Length >= MaxPathLength)
            {
                return false;
            }

            // platform independend : check for invalid path chars

            if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }
            if (fileName.IndexOf('?') >= 0 || fileName.IndexOf('*') >= 0)
            {
                return false;
            }

            if (!Regex.IsMatch(fileName, FileNameRegEx))
            {
                return false;
            }

            if (fileName[fileName.Length - 1] == ' ')
            {
                return false;
            }

            if (fileName[fileName.Length - 1] == '.')
            {
                return false;
            }

            // platform dependend : Check for invalid file names (DOS)
            // this routine checks for follwing bad file names :
            // CON, PRN, AUX, NUL, COM1-9 and LPT1-9

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (nameWithoutExtension != null)
            {
                nameWithoutExtension = nameWithoutExtension.ToUpperInvariant();
            }

            if (nameWithoutExtension == "CON" ||
                nameWithoutExtension == "PRN" ||
                nameWithoutExtension == "AUX" ||
                nameWithoutExtension == "NUL")
            {
                return false;
            }

            if (nameWithoutExtension != null)
            {
                char ch = nameWithoutExtension.Length == 4 ? nameWithoutExtension[3] : '\0';

                return !((nameWithoutExtension.StartsWith("COM") ||
                          nameWithoutExtension.StartsWith("LPT")) &&
                         Char.IsDigit(ch));
            }
            return false;
        }

        /// <summary>
        /// Checks that a single directory name (not the full path) is valid.
        /// </summary>
        public static bool IsValidDirectoryName(string name)
        {
            if (!IsValidPath(name))
            {
                return false;
            }
            if (name.IndexOfAny(new[] {Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar}) >= 0)
            {
                return false;
            }
            if (name.Trim(' ').Length == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Gets the normalized version of fileName.
        /// Slashes are replaced with backslashes, backreferences "." and ".." are 'evaluated'.
        /// </summary>
        public static string NormalizePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return fileName;

            int i;

            bool isWeb = false;
            for (i = 0; i < fileName.Length; i++)
            {
                if (fileName[i] == '/' || fileName[i] == '\\')
                    break;
                if (fileName[i] == ':')
                {
                    if (i > 1)
                        isWeb = true;
                    break;
                }
            }

            char outputSeparator = isWeb ? '/' : Path.DirectorySeparatorChar;

            var result = new StringBuilder();
            if (isWeb == false && fileName.StartsWith(@"\\") || fileName.StartsWith("//"))
            {
                i = 2;
                result.Append(outputSeparator);
            }
            else
            {
                i = 0;
            }
            int segmentStartPos = i;
            for (; i <= fileName.Length; i++)
            {
                if (i == fileName.Length || fileName[i] == '/' || fileName[i] == '\\')
                {
                    int segmentLength = i - segmentStartPos;
                    switch (segmentLength)
                    {
                        case 0:
                            // ignore empty segment (if not in web mode)
                            // On unix, don't ignore empty segment if i==0
                            if (isWeb || (i == 0 && Environment.OSVersion.Platform == PlatformID.Unix))
                            {
                                result.Append(outputSeparator);
                            }
                            break;
                        case 1:
                            // ignore /./ segment, but append other one-letter segments
                            if (fileName[segmentStartPos] != '.')
                            {
                                if (result.Length > 0) result.Append(outputSeparator);
                                result.Append(fileName[segmentStartPos]);
                            }
                            break;
                        case 2:
                            if (fileName[segmentStartPos] == '.' && fileName[segmentStartPos + 1] == '.')
                            {
                                // remove previous segment
                                int j;
                                for (j = result.Length - 1; j >= 0 && result[j] != outputSeparator; j--)
                                {
                                }
                                if (j > 0)
                                {
                                    result.Length = j;
                                }
                                break;
                            }
                            // append normal segment
                            goto default;
                        default:
                            if (result.Length > 0) result.Append(outputSeparator);
                            result.Append(fileName, segmentStartPos, segmentLength);
                            break;
                    }
                    segmentStartPos = i + 1; // remember start position for next segment
                }
            }
            if (isWeb == false)
            {
                if (result.Length > 0 && result[result.Length - 1] == outputSeparator)
                {
                    result.Length -= 1;
                }
                if (result.Length == 2 && result[1] == ':')
                {
                    result.Append(outputSeparator);
                }
            }
            return result.ToString();
        }

        public static bool IsEqualFileName(string fileName1, string fileName2)
        {
            return string.Equals(NormalizePath(fileName1),
                                 NormalizePath(fileName2),
                                 StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 获得一个目录下的全部子文件和子目录信息
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static IEnumerable<FileSystemInfo> GetAllFilesAndDirectories(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            var stack = new Stack<FileSystemInfo>();

            stack.Push(dirInfo);
            //while (dirInfo != null || stack.Count > 0)
            while (stack.Count > 0)
            {
                FileSystemInfo fileSystemInfo = stack.Pop();
                var subDirectoryInfo = fileSystemInfo as DirectoryInfo;
                if (subDirectoryInfo != null)
                {
                    yield return subDirectoryInfo;
                    try
                    {
                        FileSystemInfo[] fileSystemInfos = subDirectoryInfo.GetFileSystemInfos();
                        foreach (FileSystemInfo fsi in fileSystemInfos)
                            stack.Push(fsi);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Trace.WriteLine("~~~~" + ex.Message);
                    }
// ReSharper disable RedundantAssignment
                    dirInfo = subDirectoryInfo;
// ReSharper restore RedundantAssignment
                }
                else
                {
                    yield return fileSystemInfo;
// ReSharper disable RedundantAssignment
                    dirInfo = null;
// ReSharper restore RedundantAssignment
                }
            }
        }


        /// <summary>
        /// 获取缺省图标
        /// </summary>
        /// <param name="largeIcon"></param>
        /// <param name="smallIcon"></param>
        public static void GetDefaultIcon(out Icon largeIcon, out Icon smallIcon)
        {
            largeIcon = smallIcon = null;
            try
            {
                var phiconLarge = new IntPtr();
                var phiconSmall = new IntPtr();
                NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0,
                                             ref phiconLarge, ref phiconSmall, 1);
                if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
                if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
            }
            catch (Exception ex)
            {
                Debug.Print("[获取缺省图标] {0} {1}", DateTime.Now, ex.Message);
            }
        }

        /// <summary>   
        /// 通过扩展名得到图标和描述   
        /// </summary>   
        /// <param name="ext">扩展名(如“.txt”)</param>   
        /// <param name="largeIcon">得到大图标</param>   
        /// <param name="smallIcon">得到小图标</param>   
        /// <param name="description">得到类型描述或者空字符串</param>   
        public static void GetExtsIconAndDescription(string ext, out Icon largeIcon, out Icon smallIcon,
                                                     out string description)
        {
            try
            {
                GetDefaultIcon(out largeIcon, out smallIcon); //得到缺省图标   
                description = ""; //缺省类型描述   

                if (string.IsNullOrEmpty(ext) || !ext.StartsWith(".")) return; //return 不是以扩展名句点开始，返回Windows缺省图标

                RegistryKey extsubkey = Registry.ClassesRoot.OpenSubKey(ext); //从注册表中读取扩展名相应的子键   
                if (extsubkey == null) return;

                var extdefaultvalue = extsubkey.GetValue(null) as string; //取出扩展名对应的文件类型名称   
                if (extdefaultvalue == null) return;

                if (extdefaultvalue.Equals("exefile", StringComparison.OrdinalIgnoreCase)) //扩展名类型是可执行文件   
                {
                    RegistryKey exefilesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);
                    //从注册表中读取文件类型名称的相应子键   
                    if (exefilesubkey != null)
                    {
                        var exefiledescription = exefilesubkey.GetValue(null) as string; //得到exefile描述字符串   
                        if (exefiledescription != null) description = exefiledescription;
                    }
                    var exefilePhiconLarge = new IntPtr();
                    var exefilePhiconSmall = new IntPtr();
                    NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2,
                                                 ref exefilePhiconLarge, ref exefilePhiconSmall, 1);
                    if (exefilePhiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(exefilePhiconLarge);
                    if (exefilePhiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(exefilePhiconSmall);
                    return;
                }

                RegistryKey typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue); //从注册表中读取文件类型名称的相应子键   
                if (typesubkey == null) return;

                var typedescription = typesubkey.GetValue(null) as string; //得到类型描述字符串   
                if (typedescription != null) description = typedescription;

                RegistryKey defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon"); //取默认图标子键   
                if (defaulticonsubkey == null) return;

                //得到图标来源字符串   
                var defaulticon = defaulticonsubkey.GetValue(null) as string; //取出默认图标来源字符串   
                if (defaulticon == null) return;
                string[] iconstringArray = defaulticon.Split(',');
                int nIconIndex = 0; //声明并初始化图标索引   
                if (iconstringArray.Length > 1)
                    if (!int.TryParse(iconstringArray[1], out nIconIndex))
                        nIconIndex = 0; //int.TryParse转换失败，返回0   

                //得到图标   
                var phiconLarge = new IntPtr();
                var phiconSmall = new IntPtr();
                NativeMethods.ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall,
                                             1);
                if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
                if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
            }
            catch
            {
                largeIcon = smallIcon = null;
                description = "";
            }
        }

        #region Nested type: FileInformation

        public struct FileInformation
        {
            public string Filename { get; set; }
            public long Filesize { get; set; }
            public string Filetype { get; set; }
            public DateTime LastWriteTime { get; set; }
            public DateTime LastAccessTime { get; set; }
            public DateTime CreationTime { get; set; }
            public Icon LargeIcon { get; set; }
            public Icon SmallIcon { get; set; }
        }

        #endregion
    }
}