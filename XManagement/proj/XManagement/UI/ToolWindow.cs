using WeifenLuo.WinFormsUI.Docking;

namespace BobWei.XManagement.UI
{
    public partial class ToolWindow : DockContent
    {
        public ToolWindow()
        {
            InitializeComponent();
            //DockAreas限制为非Document
            DockAreas = DockAreas.Float |
                        DockAreas.DockLeft |
                        DockAreas.DockRight |
                        DockAreas.DockTop |
                        DockAreas.DockBottom;
            HideOnClose = true;
        }
    }
}