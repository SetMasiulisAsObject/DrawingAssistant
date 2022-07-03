using System;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.SolidWorks.UI;

namespace MMDevelop.DrawingAssistant
{
    [ComVisible(true)]
    [Title("SOLIDWORKS Task Pane Add-In Example")]
    [Guid("5C7ECCC7-DF8B-4E51-87E1-4EE1CC829C09")]
    public class MainAddIn : SwAddInEx
    {
        private ISwTaskPane<SwTaskPaneControl> m_TaskPane;
        private SwTaskPaneControl m_WpfControl;

        public override void OnConnect()
        {
            m_TaskPane = this.CreateTaskPaneWpf<SwTaskPaneControl>();
            m_WpfControl = m_TaskPane.Control;
            m_WpfControl.DataContext = new TaskPaneVM();
        }

        public override void OnDisconnect()
        {
            m_TaskPane.Close();
        }
    }
}
