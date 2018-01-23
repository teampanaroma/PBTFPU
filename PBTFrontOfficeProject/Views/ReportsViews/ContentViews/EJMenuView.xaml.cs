using PBTFrontOfficeProject.ViewModels;
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

namespace PBTFrontOfficeProject.Views.ContentViews
{
    /// <summary>
    /// Interaction logic for EJMenuView.xaml
    /// </summary>
    public partial class EJMenuView : UserControl
    {
        public EJMenuView()
        {
            InitializeComponent();
            this.DataContext = new EJMenuViewModel();
        }
    }
}
