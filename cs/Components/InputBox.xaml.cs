using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMDevelop.DrawingAssistant.Components
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : UserControl
    {
        public InputBox()
        {
            InitializeComponent();
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(InputBox), new PropertyMetadata(string.Empty));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }



        public int XDimension
        {
            get { return (int)GetValue(XDimensionProperty); }
            set { SetValue(XDimensionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XDimension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XDimensionProperty =
            DependencyProperty.Register("XDimension", typeof(int), typeof(InputBox), new PropertyMetadata(0));



        public int YDimension
        {
            get { return (int)GetValue(YDimensionProperty); }
            set { SetValue(YDimensionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YDimensin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YDimensionProperty =
            DependencyProperty.Register("YDimension", typeof(int), typeof(InputBox), new PropertyMetadata(0));


        //---
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(InputBox));

        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }
        //--




        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(TextChangedEvent));
        }

        void MyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            //bool tooLong;
            //if(text.Length >9) tooLong = true;
            return !_regex.IsMatch(text); //text length + regex that matches disallowed text
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {

                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
