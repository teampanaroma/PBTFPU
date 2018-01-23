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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBTFrontOfficeProject.Views.ProgramsViews.PContentViews
{
    /// <summary>
    /// Interaction logic for FCurrencyProgramView.xaml
    /// </summary>
    public partial class FCurrencyProgramView : UserControl
    {
        public FCurrencyProgramView()
        {
            InitializeComponent();
            this.DataContext = new FCurrencyProgramViewModel();
        }
    }
}
