using System;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BobWei.XManagement.UI
{
    public partial class DocumentWindow : DockContent
    {
        public DocumentWindow()
        {
            InitializeComponent();
            DockAreas = DockAreas.Document;
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseButton && CloseButtonVisible)
            {
                Close();
            }
        }

        private void 除此之外全部关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Configuration.MainForm != null) Configuration.MainForm.CloseAllDocumentFormsExcept(this);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!CloseButton || !CloseButtonVisible)
            {
                ((ContextMenuStrip) sender).Items.Remove(关闭ToolStripMenuItem);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (!CloseButton || !CloseButtonVisible))
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}