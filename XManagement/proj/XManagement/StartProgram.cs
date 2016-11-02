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
    /// ϵͳ����
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
        /// ϵͳ����ʱ��Ҫִ�еķ���
        /// </summary>
        public static void Start(string[] args)
        {
            Configuration.Init();
#if DEBUG
            AllocConsole();
            Configuration.Shell.WriteLine("��Ϣ����������");
#endif
#if !DEBUG
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
#endif
            ParseCommandLineArgs(args); //���������в���  

            if (!CommandLineArgs.Contains("/noinifile"))
            {
                if (File.Exists(Configuration.VarIniFilePath))
                {
                    Configuration.ReadConfig(); //����ini�����ļ���Ϣ
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Configuration.MainForm = new MainForm()); //����������

#if DEBUG
            Configuration.Shell.WriteLine("��Ϣ���������˳����밴Enter������...");
            Configuration.Shell.ReadLine();
            FreeConsole();
#endif
        }

        /// <summary>
        /// ���������в���
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
                    Configuration.Shell.WriteLine("���棺�������в���δ��ʶ��");
#endif
                }
                CommandLineArgs[match.Groups["argname"].Value] = match.Groups["argvalue"].Value;
            }
        }

        #region Exception Handling

        private static readonly string StrLogFile = Configuration.VarLogFilePath; //��־�ļ�

#if !DEBUG
        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                WriteToLog(e.ExceptionObject.ToString());
                GuiUtility.ShowMessageBoxHand(string.Format("ϵͳ����δ֪�쳣����ϸ��־�����\n{0}\n\n���򼴽��رա�", StrLogFile),
                                              Configuration.ProductName);
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            WriteToLog(e.Exception.ToString());
            bool r = GuiUtility.ShowMessageBoxYesNo(string.Format("ϵͳ�̴߳����쳣����ϸ��־�����\n{0}\n\n�Ƿ�������У�", StrLogFile),
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
                Configuration.Shell.WriteLine("�����޷���������Ϣд����־�ļ���" + ex.Message);
#else
                GuiUtility.ShowMessageBoxHand("�����޷���������Ϣд����־�ļ���\n" + ex.Message, Configuration.ProductName);
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
        /// ����������ʾ������Ϣ����¼������־��
        /// </summary>
        /// <param name="msgboxText">��ʾ���û�����Ϣ</param>
        /// <param name="logText">��������־����Ϣ</param>
        public static void Crash(string msgboxText, string logText)
        {
            WriteToLog(logText);
            GuiUtility.ShowMessageBoxHand(string.Format("{0}����ϸ��־�����\n{1}\n\n���򼴽��رա�", msgboxText, StrLogFile),
                                          Configuration.ProductName);
            Process.GetCurrentProcess().Kill();
        }

        #endregion
    }
}