using System.Windows.Forms;

namespace BobWei.XManagement.UI
{
    public class DialogForm : Form
    {
        public DialogForm()
        {
            ApplyDialogStyle(this);
        }

        /// <summary>
        /// 使普通Form应用具有对话框窗口的样式
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Form ApplyDialogStyle(Form form)
        {
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.CenterParent;
            return form;
        }
    }
}