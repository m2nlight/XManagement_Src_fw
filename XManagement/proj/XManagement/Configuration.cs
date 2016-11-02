using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BobWei.CSharp.Common.IO;
using BobWei.XManagement.UI;

namespace BobWei.XManagement
{
    /// <summary>
    /// 系统的静态配置信息
    /// </summary>
    public static class Configuration
    {
        #region Members

        //INI节名：
        private const string SessionAppearance = "Appearance"; //外观


        //------公共变量(Var)------
        /// <summary>
        /// 程序启动时间
        /// </summary>
        public static DateTimeOffset VarStartTime { get; private set; }

        /// <summary>
        /// 程序集的所在位置
        /// </summary>
        public static string VarAssemblyLocation { get; set; }

        /// <summary>
        /// INI配置文件路径
        /// </summary>
        public static string VarIniFilePath { get; set; }

        /// <summary>
        /// 主窗口布局文件路径
        /// </summary>
        public static string VarLayoutFilePath { get; set; }

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string VarLogFilePath { get; set; }


        //------正则(Regex)------
        /// <summary>
        /// 验证路径的正则
        /// </summary>
        public static string RegexPath { get; set; }

        /// <summary>
        /// 验证命令行参数
        /// </summary>
        public static string RegexCommandLineArgs { get; set; }

        /// <summary>
        /// 调试时使用的交互程序
        /// </summary>
        public static ShellSingleton Shell { get; set; }

        /// <summary>
        /// 应用程序主窗口
        /// </summary>
        public static MainForm MainForm { get; set; }

        /// <summary>
        /// 传输文件的缓冲大小
        /// </summary>
        public static long TransferBufferSize { get; set; }

        /// <summary>
        /// 工作屏幕大小
        /// </summary>
        public static Rectangle WorkingScreenBounds
        {
            get
            {
                return MainForm != null && !MainForm.IsDisposed
                           ? Screen.FromControl(MainForm).Bounds
                           : Screen.PrimaryScreen.Bounds;
            }
        }

        /// <summary>
        /// 工作区大小
        /// </summary>
        public static Rectangle WorkingArea
        {
            get
            {
                return MainForm != null && !MainForm.IsDisposed
                           ? Screen.FromControl(MainForm).WorkingArea
                           : Screen.PrimaryScreen.WorkingArea;
            }
        }

        /// <summary>
        /// 程序集完整描述
        /// </summary>
        public static AssemblyName AssemblyName { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public static string ProductName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public static Version ProductVersion { get; set; }

        /// <summary>
        /// 支持信息
        /// </summary>
        public static string SupportInfo { get; set; }


        //------外观配置------
        /// <summary>
        /// 退出时保存布局
        /// </summary>
        public static bool AppearanceSaveLayout { get; set; }

        /// <summary>
        /// 上次退出前主窗口的状态
        /// </summary>
        public static FormWindowState AppearanceWindowState { get; set; }

        /// <summary>
        /// 上次退出前主窗口的位置和大小
        /// </summary>
        public static Rectangle AppearanceWindowBounds { get; set; }

        #endregion

        #region ctor

        static Configuration()
        {
            VarStartTime = DateTimeOffset.Now;
            Assembly assembly = Assembly.GetExecutingAssembly();
            VarAssemblyLocation = assembly.Location;

#if DEBUG
            Shell = ShellSingleton.Instance;

// ReSharper disable AssignNullToNotNullAttribute
            string shelllogDir = Path.Combine(Path.GetDirectoryName(VarAssemblyLocation), "shell.log");
// ReSharper restore AssignNullToNotNullAttribute
            string shelllogFn = Path.GetFileNameWithoutExtension(VarAssemblyLocation) +
                                string.Format(".{0:yyyy-MM-dd HH-mm-ss}.shell.log", VarStartTime);
            string shelllogPath = Path.Combine(shelllogDir, shelllogFn);
            //创建日志目录
            if (!Directory.Exists(shelllogDir))
            {
                try
                {
                    Directory.CreateDirectory(shelllogDir);
                }
                catch
                {
                    Shell.WriteLineWithoutFile("注意：创建Shell输出目录{0}失败，调试信息不会输出到文件。", shelllogDir);
                }
            }
            Shell.OutputFile = shelllogPath;
#endif


            VarIniFilePath = Path.ChangeExtension(VarAssemblyLocation, ".ini");
            VarLayoutFilePath = Path.ChangeExtension(VarAssemblyLocation, ".layout");
            // ReSharper disable AssignNullToNotNullAttribute
            string logDir = Path.Combine(Path.GetDirectoryName(VarAssemblyLocation), "log");
            // ReSharper restore AssignNullToNotNullAttribute
            string logFn = Path.GetFileNameWithoutExtension(VarAssemblyLocation) +
                           string.Format(".{0:yyyy-MM-dd HH-mm-ss}.log", VarStartTime);
            VarLogFilePath = Path.Combine(logDir, logFn);
            //创建日志目录
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Shell.WriteLine("警告：无法创建日志目录。" + ex.Message);
#endif
                }
            }

            RegexPath = @"^\\$|^\\[^><|:*?\\\/]+(\\[^><|:*?\\\/]+)*[^><|:*?\\\/]*$";
            RegexCommandLineArgs = @"(?<argname>/\w+)(:(?<argvalue>\w+))?";
            TransferBufferSize = 1024*1024;
            AssemblyName = assembly.GetName();
            ProductName = AssemblyName.Name;
            ProductVersion = AssemblyName.Version;
            SupportInfo = "请将错误报告给技术支持";

            LoadSafeConfig();
        }

        #endregion

        #region functions

        /// <summary>
        /// 不执行任何操作，使Configuration类初始化
        /// </summary>
        public static void Init()
        {
        }

        /// <summary>
        /// 载入安全配置
        /// </summary>
        public static void LoadSafeConfig()
        {
            AppearanceSaveLayout = true;
            AppearanceWindowState = FormWindowState.Normal;
            AppearanceWindowBounds = WorkingArea;
        }

        /// <summary>
        /// 从配置文件读取
        /// </summary>
        public static void ReadConfig()
        {
            string key;
            string value;
            long valueLong;
            var f = new IniFile(VarIniFilePath);
            value = f.Read(SessionAppearance, "SaveLayout");
            if (long.TryParse(value, out valueLong)) AppearanceSaveLayout = valueLong == 1;
            value = f.Read(SessionAppearance, "WindowState");
            if (value.Length > 0)
            {
                try
                {
                    AppearanceWindowState = (FormWindowState) Enum.Parse(typeof (FormWindowState), value, true);
                }
                catch (ArgumentException)
                {
#if DEBUG
                    Shell.WriteLine("警告：读取配置文件出错。无法将‘{0}’转换为{1}类型的{2}", value, "FormWindowState",
                                    "WindowState");
#endif
                }
            }
            Rectangle screenBounds = WorkingScreenBounds;
            key = string.Format("{0}x{1} {2}", screenBounds.Width, screenBounds.Height, "WindowBounds");
            value = f.Read(SessionAppearance, key);
            if (value.Length > 0)
            {
                var typeConverter = new RectangleConverter();
                try
                {
                    // ReSharper disable PossibleNullReferenceException
                    AppearanceWindowBounds = (Rectangle) typeConverter.ConvertFromInvariantString(value);
                    // ReSharper restore PossibleNullReferenceException
                }
                catch (NotSupportedException)
                {
#if DEBUG
                    Shell.WriteLine("警告：读取配置文件出错。无法将‘{0}’转换为{1}类型的{2}", value, "Rectangle", "WindowBounds");
#endif
                }
            }
        }

        /// <summary>
        /// 将配置写入文件
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                var f = new IniFile(VarIniFilePath);
                f.Write(SessionAppearance, "SaveLayout", AppearanceSaveLayout ? "1" : "0");
                f.Write(SessionAppearance, "WindowState", AppearanceWindowState.ToString());
                Rectangle screenBounds = WorkingScreenBounds;
                f.Write(SessionAppearance,
                        string.Format("{0}x{1} {2}", screenBounds.Width, screenBounds.Height, "WindowBounds"),
                        new RectangleConverter().ConvertToInvariantString(AppearanceWindowBounds));
            }
            catch (Exception)
            {
#if DEBUG
                Shell.WriteLine("警告：写入配置文件时出错。");
#endif
            }
        }

        /// <summary>
        /// 更新配置信息
        /// </summary>
        public static void UpdateConfig()
        {
            //保存窗口状态和大小
            if (MainForm != null && !MainForm.IsDisposed)
            {
                AppearanceWindowState = MainForm.WindowState;
                if (AppearanceWindowState == FormWindowState.Normal)
                {
                    AppearanceWindowBounds = MainForm.Bounds;
                }
            }
        }

        #endregion
    }
}