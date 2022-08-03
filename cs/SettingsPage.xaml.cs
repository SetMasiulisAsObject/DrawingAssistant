using MMDevelop.DrawingAssistant.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Xarial.XCad.Base.Attributes;

namespace MMDevelop.DrawingAssistant
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    [Icon(typeof(Resources), nameof(Properties.Resources.cd_icon))]
    [Title("DrawingAssistantSettings")]
    [Description("Main drawing creation settings page")]
    public partial class SettingsPage : Window
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnClosing(CancelEventArgs e) // Hides the pop up page
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private int textToInt(string inputText)
        {
            if (int.TryParse(inputText, out int value))
            {
                if (value >= 0)
                {
                    return value; //Do something
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int[] Margins =
            {
               textToInt( MarginLeft.Text),
               textToInt( MarginRight.Text),
               textToInt( MarginTop.Text),
               textToInt( MarginBottom.Text)
            };
            int[] Coordinates = { Back1Value.XDimension, Back1Value.YDimension, //0,1
                _3D1Value.XDimension,_3D1Value.YDimension,
                TopValue.XDimension, TopValue.YDimension,
                _3D2Value.XDimension, _3D2Value.YDimension,
                Back2Value.XDimension, Back2Value.YDimension,
                LeftValue.XDimension, LeftValue.YDimension,
                MainValue.XDimension, MainValue.YDimension,
                RightValue.XDimension, RightValue.YDimension,
                Back3Value.XDimension, Back3Value.YDimension,
                _3D3Value.XDimension, _3D3Value.YDimension,
                BottomValue.XDimension, BottomValue.YDimension,
                _3D3Value.XDimension, _3D3Value.YDimension,
                Back4Value.XDimension, Back4Value.YDimension //24,25
                };//26
            Settings.Default["ViewMargins"] = Margins;
            Settings.Default["DoManualPlacement"] = DoManualPlacementBox.IsChecked;
            Settings.Default["ViewCoordinates"] = Coordinates;
            Settings.Default.Save();
        }

        #region Save coordinates of projected views on drawing sheet
        private int[] _viewMargins = (int[])Settings.Default["ViewMargins"];
        public int[] ViewMargins
        {
            get => _viewMargins;
            set
            {
                if (_viewMargins != value)
                {
                    _viewMargins = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Save coordinates of projected views on drawing sheet
        /// </summary>
        private int[] _viewCoordinates = (int[])Settings.Default["ViewCoordinates"];
        public int[] ViewCoordinates
        {
            get => _viewCoordinates;
            set
            {
                if (_viewCoordinates != value)
                {
                    _viewCoordinates = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _doManualPlacement = (bool)Settings.Default["DoManualPlacement"];
        public bool DoManualPlacement
        {
            get => _doManualPlacement;
            set
            {
                if (_doManualPlacement != value)
                {
                    _doManualPlacement = value;
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

        private void EqualizeValues(object sender, RoutedEventArgs e)
        {
            //var value = (TextBox)sender;
            //try
            //{
            //    var success = Int32.TryParse(value.Text, out int result);

            //    if (success)
            //    {
            //        // Output: Unable to parse ''
            //        Back1Value.XDimension = Back1Value.YDimension = //0,1
            //        _3D1Value.XDimension = _3D1Value.YDimension =
            //        TopValue.XDimension = TopValue.YDimension =
            //        _3D2Value.XDimension = _3D2Value.YDimension =
            //        Back2Value.XDimension = Back2Value.YDimension =
            //        LeftValue.XDimension = LeftValue.YDimension =
            //        MainValue.XDimension = MainValue.YDimension =
            //        RightValue.XDimension = RightValue.YDimension =
            //        Back3Value.XDimension = Back3Value.YDimension =
            //        _3D3Value.XDimension = _3D3Value.YDimension =
            //        BottomValue.XDimension = BottomValue.YDimension =
            //        _3D3Value.XDimension = _3D3Value.YDimension =
            //        Back4Value.XDimension = Back4Value.YDimension = result;
            //    }
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine($"Unable to parse '{value.Text}'");
            //}
        }



    }
}
