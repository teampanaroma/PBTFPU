using PBTFrontOfficeProject.Service;
using PBTFrontOfficeProject.ViewModels.ProgramsViewsModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBTFrontOfficeProject.Views.ProgramsViews
{
    /// <summary>
    /// Interaction logic for ProgramsMenuView.xaml
    /// </summary>
    public partial class ProgramsMenuView : UserControl
    {
        TextBlock tb;

        public ProgramsMenuView()
        {
            InitializeComponent();
            this.DataContext = new ProgramsMenuViewModel();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            lb2.SelectedIndex = -1;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            tb = sender as TextBlock;
        }

        private void DoubleAnimation_Changed(object sender, EventArgs e)
        {
            DoubleAnimation dbn = sender as DoubleAnimation;
            dbn.To = Convert.ToDouble(tb.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.tabShell.SelectedIndex = 0;
        }
        private void img_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deneme");
        }
    }
}
