using System;
using System.Windows;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.SolidWorks.UI;
using Xarial.XCad.SolidWorks.UI.PropertyPage;
using Xarial.XCad.UI.Commands;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;
using Xarial.XCad.UI.PropertyPage.Attributes;

namespace MMDevelop.DrawingAssistant
{
    [ComVisible(true)]
    [Title("DrawingAssistant Task Pane")]
    [Guid("5C7ECCC7-DF8B-4E51-87E1-4EE1CC829C09")]
    public class MainAddIn : SwAddInEx
    {
        enum DrawingAssistantCommands_e
        {

            //[CommandItemInfo(WorkspaceTypes_e.AllDocuments)]
            CreateWPFPopWindow
        }
        [ComVisible(true)]
        public class WPFPopWindow : SwPropertyManagerPageHandler
        {
            [CustomControl(typeof(SettingsPage))]
            public object SettingsPageCtrl { get; set; }
        }

        private ISwTaskPane<SwTaskPaneControl> m_TaskPane;
        private SwTaskPaneControl m_WpfControl;
        private SettingsPage m_PopupWindow;

        public override void OnConnect()
        {

            m_TaskPane = this.CreateTaskPaneWpf<SwTaskPaneControl>();
            m_WpfControl = m_TaskPane.Control;
            //m_WpfControl.SwApp = Application;
            // m_WpfControl.DataContext = new TaskPaneVM();
            CommandManager.AddCommandGroup<DrawingAssistantCommands_e>().CommandClick += OnButtonClick;
            m_PopupWindow = new SettingsPage();
            //m_WinFormsPMPage = CreatePage<WinFormsPMPage>();
            //m_WpfPMPage = CreatePage<WpfPMPage>();
            Globals.SwApp = Application;
        }

        public override void OnDisconnect()
        {
            m_TaskPane.Close();
        }

        private void OnButtonClick(DrawingAssistantCommands_e cmd)
        {
            var activeDoc = Application.Documents.Active;

            switch (cmd)
            {

                case DrawingAssistantCommands_e.CreateWPFPopWindow:
                    {

                        m_PopupWindow.ShowDialog();
                        //m_PopupWindow.Visibility=Visibility.Visible;
                        break;
                    }

            }
        }
    }
    public class Globals
    {
        private static ISwApplication _swApp;
        public static ISwApplication SwApp
        {
            get
            {
                // Reads are usually simple
                return _swApp;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _swApp = value;
            }
        }
        // Perhaps extend this to have Read-Modify-Write static methods
        // for data integrity during concurrency? Situational.
    }
}
