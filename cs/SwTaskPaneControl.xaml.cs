using System.ComponentModel;
using System.Windows.Controls;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks.UI.Commands;
using MMDevelop.DrawingAssistant.Properties;
using Xarial.XCad.SolidWorks;
using SolidWorks.Interop.sldworks;
using System.Diagnostics;
using static MMDevelop.DrawingAssistant.Globals;
using System.IO;
using System;
using Xarial.XCad.Base.Enums;
using System.Windows.Controls.Primitives;
using System.Runtime.CompilerServices;
using Xarial.XCad;
using Xarial.XCad.Data;
using Xarial.XCad.Documents;
using SolidWorks.Interop.swconst;

namespace MMDevelop.DrawingAssistant
{
    [Icon(typeof(Resources), nameof(Properties.Resources.cd_icon))]
    [Title("DrawingAssistant")]
    [Description("Main drawing creation interface")]
    public partial class SwTaskPaneControl : UserControl, INotifyPropertyChanged
    {
        #region Constructor

        public SwTaskPaneControl()
        {

            InitializeComponent();
            DataContext = this;
        }

        #endregion

        //private void CreateDrawing_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Klavisas veikia");

        //}

        #region Imported methods/functions or whatever it is
        private readonly DrawingFunction Draw = new DrawingFunction();
        private readonly FetchModelData Fetch = new FetchModelData();

        #endregion

        #region Public variables
        //public string mainViewProjectionName = "*Front";
        public ProjectedViews ProjectedNames = new ProjectedViews();
        public string MainViewProjection { get; set; } = "*Front";
        public ModelDoc2 StaticModel { get; set; }

        //private AccessApp swApp = new AccessApp();
        #endregion


        #region Startup

        //public event Action<ModelDoc2> ActiveModelInformationChanged = delegate
        //{
        //};

        /// <summary>
        /// Fired when the control is fully loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // By default show the No Part open screen
            //NoPartContent.Visibility = System.Windows.Visibility.Visible;
            //MainContent.Visibility = System.Windows.Visibility.Hidden;
            //swApp.Application.ShowMessageBox("Loaded", MessageBoxIcon_e.Info, MessageBoxButtons_e.Ok);

            // Listen out for the active model changing

            // swApp.Application.Sw.ActiveDoc.ActiveModelInformationChanged += Application_ActiveModelInformationChanged;
        }

        #endregion
        #region Model Events

        /// <summary>
        /// Fired when the active SolidWorks model is changed
        /// </summary>
        /// <param name="obj"></param>


        #endregion
        #region Read Details



        ///// <summary>
        ///// Reads all the details from the active SolidWorks model
        ///// </summary>
        private void ReadDetails()
        {
            var swModel = (ModelDoc2)SwApp.Sw.ActiveDoc;

            if (SwApp.Sw.ActiveDoc != StaticModel)
            {
                StaticModel = swModel;
                // Last used part model
            }
            // If we have no model, or the model is not a part
            // then show the No Part screen and return
            if (swModel == null)//|| (!model.IsPart && !model.IsAssembly))
            {
                // Show No Part screen
                // NoPartContent.Visibility = System.Windows.Visibility.Visible;
                MainContent.Visibility = System.Windows.Visibility.Hidden;

                return;
            }

            // If we got here, we have a part

            // Show the main content
            //NoPartContent.Visibility = System.Windows.Visibility.Hidden;
            MainContent.Visibility = System.Windows.Visibility.Visible;

            // Get custom properties
            // Description
            // This lates will be done -->> DescriptionText.Text = model.GetCustomProperty(CustomPropertyDescription);
            var swModelDoc = StaticModel as ModelDoc2;
            ModelName.Text = "Model name: " + Path.GetFileName(swModelDoc.GetPathName());
            ActiveConfiguration.Text = "Active configuration: " + swModelDoc.ConfigurationManager.ActiveConfiguration.Name.ToString();
            ConfigurationNumber.Text = "Configurations: " + swModelDoc.GetConfigurationCount().ToString();
            var FetchBodyDataFromModel = Fetch.FetchModelDataRun(StaticModel);
            WeldmentBodies.Text = "Weldment bodies: " + FetchBodyDataFromModel[0].ToString();
            SheetMetalBodies.Text = "Sheet metal bodies: " + FetchBodyDataFromModel[1].ToString();
            OtherBodies.Text = "Other bodies: " + FetchBodyDataFromModel[2].ToString();
            //});
        }

        #endregion
        #region Save the last used drawing template
        /// <summary>
        /// Save the last used drawing template
        /// </summary>
        private string _drawingTemplatePath = Settings.Default["LastUsedTemplate"].ToString();
        public string DrawingTemplatePath
        {
            get => _drawingTemplatePath;
            set
            {
                if (_drawingTemplatePath != value)
                {
                    _drawingTemplatePath = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        /// <summary>
        /// When the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDrawing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //PartDoc swPart;
            //var swApp = SwApplicationFactory.Create();
            //ReadDetails();
            ReadProjectedViews();
            if (StaticModel != null) //
            {
                //swPart = Application.ActiveModel.AsPart();
                if (DrawingTemplatePath == string.Empty)
                {
                    SwApp.ShowMessageBox("Please select drawing template \".drwdot\"", MessageBoxIcon_e.Info, MessageBoxButtons_e.Ok);
                    // Application.ShowMessageBox("Please select drawing template \".drwdot\"", AngelSix.SolidDna.SolidWorksMessageBoxIcon.Information, AngelSix.SolidDna.SolidWorksMessageBoxButtons.Ok);
                }
                else
                {
                    //swPart = StaticModel;
                    Draw.CreateDrawing(DrawingTemplatePath, MainViewProjection, StaticModel, ProjectedNames);
                }
            }
            else
            {
                MessageBoxResult_e messageBoxResult_e = SwApp.ShowMessageBox("There is no active model to make a drawing from. Please open a part or assembly model", MessageBoxIcon_e.Info, MessageBoxButtons_e.Ok);
                // Application.ShowMessageBox("There is no active model to make a drawing from. Please open a part or assembly model", AngelSix.SolidDna.SolidWorksMessageBoxIcon.Information, AngelSix.SolidDna.SolidWorksMessageBoxButtons.Ok);
            }
            // Get the number of selected objects
            // var count = 0;
            //Application.ActiveModel?.SelectedObjects(objects => count = objects?.Count ?? 0);
            // Let the user know
            //Application.ShowMessageBox($"Looks like you have {count} objects selected");

        }

        private void ReadProjectedViews()
        {
            //ThreadHelpers.RunOnUIThread(() =>
            //{
            ProjectedNames.back1 = (bool)Back1Projection.IsChecked;
            ProjectedNames.back2 = (bool)Back2Projection.IsChecked;
            ProjectedNames.back3 = (bool)Back3Projection.IsChecked;
            ProjectedNames.back4 = (bool)Back4Projection.IsChecked;
            ProjectedNames.left = (bool)LeftProjection.IsChecked;
            ProjectedNames.right = (bool)RightProjection.IsChecked;
            ProjectedNames.top = (bool)TopProjection.IsChecked;
            ProjectedNames.bottom = (bool)BottomProjection.IsChecked;
            ProjectedNames._3D1 = (bool)_3D1Projection.IsChecked;
            ProjectedNames._3D2 = (bool)_3D2Projection.IsChecked;
            ProjectedNames._3D3 = (bool)_3D3Projection.IsChecked;
            ProjectedNames._3D4 = (bool)_3D4Projection.IsChecked;
            //});
        }

        private void SelectDrawingTemplate_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            // ThreadHelpers.RunOnUIThread(() =>
            //{
            DrawingTemplatePath = Draw.SetDrawingTemplate(DrawingTemplatePath);
            Settings.Default["LastUsedTemplate"] = DrawingTemplatePath;
            Settings.Default.Save();
            // DescriptionText.Text = drawingTemplatePath;
            //});
        }

        private void ViewCheck_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            //ThreadHelpers.RunOnUIThread(() =>
            // {
            var myToggleButton = (ToggleButton)sender;
            TopViewCheck.IsChecked = LeftViewCheck.IsChecked = FrontViewCheck.IsChecked = BackViewCheck.IsChecked =
                RightViewCheck.IsChecked = BottomViewCheck.IsChecked = false;
            myToggleButton.IsChecked = true;
            MainViewProjection = myToggleButton.Name;
            MainViewProjection = "*" + MainViewProjection.Replace("ViewCheck", "");
            //});
            //Application.ShowMessageBox(myToggleButton.Name);
            // TopViewCheck.IsChecked =!TopViewCheck.IsChecked;
        }
        private void ProjectionCheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //MainProjection.IsChecked = true; // This is always checked
            var myCheckBox = (CheckBox)sender;

            switch (myCheckBox.Name)
            {
                case "Back1Projection":
                    ToggleCheckBoxBefore(Back1Projection, TopProjection);
                    break;
                case "Back2Projection":
                    ToggleCheckBoxBefore(Back2Projection, LeftProjection);
                    break;
                case "Back3Projection":
                    ToggleCheckBoxBefore(Back3Projection, RightProjection);
                    break;
                case "Back4Projection":
                    ToggleCheckBoxBefore(Back4Projection, BottomProjection);
                    break;
                case "TopProjection":
                    ToggleCheckBoxAfter(TopProjection, Back1Projection);
                    break;
                case "LeftProjection":
                    ToggleCheckBoxAfter(LeftProjection, Back2Projection);
                    break;
                case "RightProjection":
                    ToggleCheckBoxAfter(RightProjection, Back3Projection);
                    break;
                case "BottomProjection":
                    ToggleCheckBoxAfter(BottomProjection, Back4Projection);
                    break;
                default:
                    break;
            }
            void ToggleCheckBoxBefore(CheckBox firstCheckBox, CheckBox secondCheckBox)
            {
                if (secondCheckBox.IsChecked == false)
                {
                    secondCheckBox.IsChecked = true;
                }
            }
            void ToggleCheckBoxAfter(CheckBox firstCheckBox, CheckBox secondCheckBox)
            {
                if (firstCheckBox.IsChecked == false && secondCheckBox.IsChecked == true)
                {
                    secondCheckBox.IsChecked = false;
                }
            }
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (Application.ActiveModel.IsDrawing)
            //{
            //    Draw.DeleteAllViews(Application.ActiveModel.AsDrawing());
            //}
            if (StaticModel != null)
            {
                Fetch.FetchModelDataRun(StaticModel);
            }
        }

        private void Option_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReadDetails();
        }

        //private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    Draw.GetBoundingBox();
        ////}
    }
    public class ProjectedViews
    {
        public bool back1, back2, back3, back4, left, right, top, bottom, _3D1, _3D2, _3D3, _3D4;

        public bool mainView = true;
    }
}