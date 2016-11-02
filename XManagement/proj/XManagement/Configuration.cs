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
    /// ϵͳ�ľ�̬������Ϣ
    /// </summary>
    public static class Configuration
    {
        #region Members

        //INI������
        private const string SessionAppearance = "Appearance"; //���


        //------��������(Var)------
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public static DateTimeOffset VarStartTime { get; private set; }

        /// <summary>
        /// ���򼯵�����λ��
        /// </summary>
        public static string VarAssemblyLocation { get; set; }

        /// <summary>
        /// INI�����ļ�·��
        /// </summary>
        public static string VarIniFilePath { get; set; }

        /// <summary>
        /// �����ڲ����ļ�·��
        /// </summary>
        public static string VarLayoutFilePath { get; set; }

        /// <summary>
        /// ��־�ļ�·��
        /// </summary>
        public static string VarLogFilePath { get; set; }


        //------����(Regex)------
        /// <summary>
        /// ��֤·��������
        /// </summary>
        public static string RegexPath { get; set; }

        /// <summary>
        /// ��֤�����в���
        /// </summary>
        public static string RegexCommandLineArgs { get; set; }

        /// <summary>
        /// ����ʱʹ�õĽ�������
        /// </summary>
        public static ShellSingleton Shell { get; set; }

        /// <summary>
        /// Ӧ�ó���������
        /// </summary>
        public static MainForm MainForm { get; set; }

        /// <summary>
        /// �����ļ��Ļ����С
        /// </summary>
        public static long TransferBufferSize { get; set; }

        /// <summary>
        /// ������Ļ��С
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
        /// ��������С
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
        /// ������������
        /// </summary>
        public static AssemblyName AssemblyName { get; set; }

        /// <summary>
        /// ��Ʒ��
        /// </summary>
        public static string ProductName { get; set; }

        /// <summary>
        /// ��Ʒ�汾
        /// </summary>
        public static Version ProductVersion { get; set; }

        /// <summary>
        /// ֧����Ϣ
        /// </summary>
        public static string SupportInfo { get; set; }


        //------�������------
        /// <summary>
        /// �˳�ʱ���沼��
        /// </summary>
        public static bool AppearanceSaveLayout { get; set; }

        /// <summary>
        /// �ϴ��˳�ǰ�����ڵ�״̬
        /// </summary>
        public static FormWindowState AppearanceWindowState { get; set; }

        /// <summary>
        /// �ϴ��˳�ǰ�����ڵ�λ�úʹ�С
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
            //������־Ŀ¼
            if (!Directory.Exists(shelllogDir))
            {
                try
                {
                    Directory.CreateDirectory(shelllogDir);
                }
                catch
                {
                    Shell.WriteLineWithoutFile("ע�⣺����Shell���Ŀ¼{0}ʧ�ܣ�������Ϣ����������ļ���", shelllogDir);
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
            //������־Ŀ¼
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Shell.WriteLine("���棺�޷�������־Ŀ¼��" + ex.Message);
#endif
                }
            }

            RegexPath = @"^\\$|^\\[^><|:*?\\\/]+(\\[^><|:*?\\\/]+)*[^><|:*?\\\/]*$";
            RegexCommandLineArgs = @"(?<argname>/\w+)(:(?<argvalue>\w+))?";
            TransferBufferSize = 1024*1024;
            AssemblyName = assembly.GetName();
            ProductName = AssemblyName.Name;
            ProductVersion = AssemblyName.Version;
            SupportInfo = "�뽫���󱨸������֧��";

            LoadSafeConfig();
        }

        #endregion

        #region functions

        /// <summary>
        /// ��ִ���κβ�����ʹConfiguration���ʼ��
        /// </summary>
        public static void Init()
        {
        }

        /// <summary>
        /// ���밲ȫ����
        /// </summary>
        public static void LoadSafeConfig()
        {
            AppearanceSaveLayout = true;
            AppearanceWindowState = FormWindowState.Normal;
            AppearanceWindowBounds = WorkingArea;
        }

        /// <summary>
        /// �������ļ���ȡ
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
                    Shell.WriteLine("���棺��ȡ�����ļ������޷�����{0}��ת��Ϊ{1}���͵�{2}", value, "FormWindowState",
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
                    Shell.WriteLine("���棺��ȡ�����ļ������޷�����{0}��ת��Ϊ{1}���͵�{2}", value, "Rectangle", "WindowBounds");
#endif
                }
            }
        }

        /// <summary>
        /// ������д���ļ�
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
                Shell.WriteLine("���棺д�������ļ�ʱ����");
#endif
            }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public static void UpdateConfig()
        {
            //���洰��״̬�ʹ�С
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