using PBTFrontOfficeProject.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBTFrontOfficeProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            this.WindowStyle = System.Windows.WindowStyle.None;
            AppService.CurrentWindow = this;
#if DEBUG
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
#endif
        }

        private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if (!(e.NewFocus is UIElement))
                    return;
                UIElement UI = e.NewFocus as UIElement;
                if (e.NewFocus is Window)
                {
                    UI = (e.NewFocus as Window);
                    //FocusManager.SetFocusedElement(UI, UI);
                    UI.Focus();
                    //UI.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
                else
                {
                    UI = (e.NewFocus as UIElement);
                    if (UI == null)
                        return;
                    if (UI is ListViewItem)
                        return;

                    //UI.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    //return;
                }
                if (UI != null)
                {
                    var result = UI.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    if (!result)
                    {
                        //Console.WriteLine(result);
                        //MessageBox.Show("asdasd");
                    }
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
                str = "Window_GotKeyboardFocus\n" + str;
                if (ex.InnerException != null)
                    str += "\n" + ex.InnerException.Message;
                //Console.WriteLine(str);

                //UserInteraction.ShowErrorMessage(str);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.IsEnabled)
            {
                e.Handled = true;
                return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.Shutdown();
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Application.Current.Shutdown();
        }


    }
}
