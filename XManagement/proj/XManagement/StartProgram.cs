using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BobWei.CSharp.Common;
using BobWei.CSharp.Common.Utility;
using BobWei.XManagement.UI;

#if DEBUG
using System.Runtime.InteropServices;
#endif

namespace BobWei.XManagement
{
    /// <summary>
    /// 系统启动
    /// </summary>
    public static class StartProgram
    {
#if DEBUG
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
#endif
        public static Hashtable CommandLineArgs = new Hashtable();

        /// <summary>
        /// 系统启动时需要执行的方法
        /// </summary>
        public static void Start(string[] args)
        {
            Configuration.Init();
#if DEBUG
            AllocConsole();
            Configuration.Shell.WriteLine("信息：启动程序。");
#endif
#if !DEBUG
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
#endif
            ParseCommandLineArgs(args); //解释命令行参数  

            if (!CommandLineArgs.Contains("/noinifile"))
            {
                if (File.Exists(Configuration.VarIniFilePath))
                {
                    Configuration.ReadConfig(); //载入ini配置文件信息
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Configuration.MainForm = new MainForm()); //启动主窗口

#if DEBUG
            Configuration.Shell.WriteLine("信息：主程序退出，请按Enter键结束...");
            Configuration.Shell.ReadLine();
            FreeConsole();
#endif
        }

        /// <summary>
        /// 分析命令行参数
        /// </summary>
        /// <param name="args"></param>
        private static void ParseCommandLineArgs(string[] args)
        {
            if (args.Length == 0) return;

            string pattern = Configuration.RegexCommandLineArgs;
            foreach (string arg in args)
            {
                Match match = Regex.Match(arg, pattern);
                if (!match.Success)
                {
#if DEBUG
                    Configuration.Shell.WriteLine("警告：有命令行参数未被识别。");
#endif
                }
                CommandLineArgs[match.Groups["argname"].Value] = match.Groups["argvalue"].Value;
            }
        }

        #region Exception Handling

        private static readonly string StrLogFile = Configuration.VarLogFilePath; //日志文件

#if !DEBUG
        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                WriteToLog(e.ExceptionObject.ToString());
                GuiUtility.ShowMessageBoxHand(string.Format("系统发生未知异常，详细日志请查阅\n{0}\n\n程序即将关闭。", StrLogFile),
                                              Configuration.ProductName);
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            WriteToLog(e.Exception.ToString());
            bool r = GuiUtility.ShowMessageBoxYesNo(string.Format("系统线程处理异常，详细日志请查阅\n{0}\n\n是否继续运行？", StrLogFile),
                                                    Configuration.ProductName);
            if (!r) Process.GetCurrentProcess().Kill();
        }

#endif

        private static void WriteToLog(string msg)
        {
            StreamWriter writer = null;
            try
            {
                var text = string.Format("{0}[{1:yy-MM-dd HH:mm:ss}]{0}{2}{0}", Environment.NewLine,
                                               DateTimeOffset.Now, msg);
                if (!File.Exists(StrLogFile))
                {
                    text = string.Format("{0}{1}{2}", GetSystemInfo(), new string('=', 40), text);
                }
                writer = new StreamWriter(StrLogFile, true, Encoding.UTF8);
                writer.Write(text);
            }
            catch (Exception ex)
            {
#if DEBUG
                Configuration.Shell.WriteLine("错误：无法将崩溃信息写入日志文件。" + ex.Message);
#else
                GuiUtility.ShowMessageBoxHand("错误：无法将崩溃信息写入日志文件。\n" + ex.Message, Configuration.ProductName);
#endif
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        private static string GetSystemInfo()
        {
            return SystemInfo.GetSystemInfo(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// 当掉程序。显示错误消息，记录错误日志。
        /// </summary>
        /// <param name="msgboxText">显示给用户的信息</param>
        /// <param name="logText">存入在日志的信息</param>
        public static void Crash(string msgboxText, string logText)
        {
            WriteToLog(logText);
            GuiUtility.ShowMessageBoxHand(string.Format("{0}，详细日志请查阅\n{1}\n\n程序即将关闭。", msgboxText, StrLogFile),
                                          Configuration.ProductName);
            Process.GetCurrentProcess().Kill();
        }

        #endregion
    }
}