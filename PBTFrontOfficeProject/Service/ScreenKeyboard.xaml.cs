using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// Interaction logic for ScreenKeyboard.xaml
    /// </summary>
    public partial class ScreenKeyboard : Window
    {
        public Window CurrentWindow { get; set; }
        public bool IsHide { get; set; }
        Rect _screen;

        public ScreenKeyboard()
        {
            InitializeComponent();
            _screen = System.Windows.SystemParameters.WorkArea;

            WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

            IsHide = true;
        }

        private void label1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
                CurrentWindow.Focus();
            }
            catch
            {


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsHide = true;
            this.Hide();
            AppService.IsVirtualKeyboardActive = false;
            CurrentWindow.Focus();
        }

        private void button45_Click(object sender, RoutedEventArgs e)
        {
            CurrentWindow.Focus();
            string text = ((Button)sender).Content.ToString();

            var textEvent = new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text))
            {
                RoutedEvent = TextInputEvent
            };

            InputManager.Current.ProcessInput(textEvent);
        }

        private void btn_space_Click(object sender, RoutedEventArgs e)
        {
            CurrentWindow.Focus();
            Key key = (Key)((Button)sender).Tag;
            KeyEventArgs keyEvent = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
            {
                RoutedEvent = PreviewKeyDownEvent
            };

            InputManager.Current.ProcessInput(keyEvent);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppService.IsVirtualKeyboardActive = true;
            //button45.Content = POS.Localization.Properties.Resources.Num1 + POS.Localization.Properties.Resources.Num2 + POS.Localization.Properties.Resources.Num3;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && CurrentWindow != null)
            {
                CurrentWindow.Focus();
                CurrentWindow.Activate();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
                CurrentWindow.Focus();
            }
            catch { }
        }

    }
}
