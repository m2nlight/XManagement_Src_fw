using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BobWei.CSharp.Common.Utility;
using WeifenLuo.WinFormsUI.Docking;

namespace BobWei.XManagement.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            InitializeLayout();
            Size = Configuration.AppearanceWindowBounds.Size;
            Location = GuiUtility.SetWindowFitLocation(Configuration.AppearanceWindowBounds, Configuration.WorkingArea);
            //最小化窗口会显示为正常
            WindowState = Configuration.AppearanceWindowState == FormWindowState.Minimized
                              ? FormWindowState.Normal
                              : Configuration.AppearanceWindowState;
        }

        private void InitializeLayout()
        {
            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            _dummyFunctionTreeWindow=new DummyFunctionTreeWindow();
            _dummyReportDoc=new DummyReportDoc();
            
            LoadLayout(Configuration.AppearanceSaveLayout);
            //ShowMdiChild(_dummyReportDoc);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(DummyFunctionTreeWindow).ToString())
                return _dummyFunctionTreeWindow;
            if (persistString == typeof(DummyReportDoc).ToString())
                return _dummyReportDoc;

            // DummyDoc overrides GetPersistString to add extra information into persistString.
            // Any DockContent may override this value to add any needed information for deserialization.

            //string[] parsedStrings = persistString.Split(new char[] { ',' });
            //if (parsedStrings.Length != 3)
            //    return null;

            //if (parsedStrings[0] != typeof(DummyDoc).ToString())
            //    return null;

            //DummyDoc dummyDoc = new DummyDoc();
            //if (parsedStrings[1] != string.Empty)
            //    dummyDoc.FileName = parsedStrings[1];
            //if (parsedStrings[2] != string.Empty)
            //    dummyDoc.Text = parsedStrings[2];

            //return dummyDoc;
            return null;
        }

        private void LoadLayout(bool useLayoutFile)
        {
            if (useLayoutFile && File.Exists(Configuration.VarLayoutFilePath))
            {
                var loaded = false;
                try
                {
                    dockPanel1.LoadFromXml(Configuration.VarLayoutFilePath, _deserializeDockContent);   //载入布局
                    loaded = true;
                }
                catch
                {
#if DEBUG
                    Configuration.Shell.WriteLine("载入外部文件布局时发生异常，主窗口LoadLayout方法。" + Configuration.VarLayoutFilePath);
#endif
                }
                if (loaded) return;
            }

            var byteArray = Encoding.Unicode.GetBytes(Properties.Resources.layout);
            using (var ms = new MemoryStream(byteArray))
            {
                dockPanel1.LoadFromXml(ms, _deserializeDockContent);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Configuration.UpdateConfig();
            Configuration.SaveConfig();
            SaveLayout();
            base.OnClosing(e);
        }

        #region DockPanel窗口

        private DeserializeDockContent _deserializeDockContent;     //序列化DockPanel窗口用
        private DummyFunctionTreeWindow _dummyFunctionTreeWindow;   //功能树工具窗口
        private DummyReportDoc _dummyReportDoc;                     //报告文档窗口

        /// <summary>
        /// 将布局写入文件
        /// </summary>
        private void SaveLayout()
        {
            if (Configuration.AppearanceSaveLayout)
            {
                try
                {
                    dockPanel1.SaveAsXml(Configuration.VarLayoutFilePath);
                }
                catch
                {
#if DEBUG
                    Configuration.Shell.WriteLine("警告：在主窗口关闭时，保存布局文件失败。" + Configuration.VarLayoutFilePath);
#endif
                }
            }
        }

        #endregion

        #region MDI主窗口功能扩展

        /// <summary>
        /// 返回主DockPanel对象
        /// </summary>
        public DockPanel DockPanel
        {
            get { return dockPanel1; }
        }

        /// <summary>
        /// 获得全部文档窗口
        /// </summary>
        public IEnumerable<DocumentWindow> DocumentWindows
        {
            get { return dockPanel1.Documents.Where(n => n is DocumentWindow).Select(n => (DocumentWindow)n); }
        }

        /// <summary>
        /// 关闭所有文档窗口，除了form窗口外
        /// </summary>
        /// <param name="form"></param>
        internal void CloseAllDocumentFormsExcept(Form form)
        {
            foreach (Form item in dockPanel1.Contents.ToArray())
            {
                if (item != form && item is DocumentWindow)
                {
                    var doc = (DocumentWindow)item;
                    if (doc.CloseButton && doc.CloseButtonVisible) //检查是否可关闭
                    {
                        try
                        {
                            doc.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (form != null) ActivateMdiChild(form);
        }

        /// <summary>
        /// 激活某个文档窗口
        /// </summary>
        /// <param name="form">文档窗口</param>
        internal void ActiviteMdiChild(Form form)
        {
            if (form != null && MdiChildren.Contains(form))
            {
                ActivateMdiChild(form);
            }
        }

        /// <summary>
        /// 显示文档窗口
        /// </summary>
        /// <param name="form">文档窗口</param>
        internal void ShowMdiChild(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }
            var docWnd = form as DocumentWindow;
            if (docWnd != null)
            {
                docWnd.Show(dockPanel1);
            }
            else
            {
                if (form.MdiParent == null)
                {
                    form.MdiParent = this;
                }
                form.Show();
            }
        }

        #endregion
    }
}