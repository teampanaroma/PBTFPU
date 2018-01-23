using PBTFrontOfficeProject.ViewModels.EkuTekBelgeViewModels;
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
    /// Interaction logic for ReadEJDateAndTimeView.xaml
    /// </summary>
    public partial class ReadEJDateAndTimeView : UserControl
    {
        public ReadEJDateAndTimeView()
        {
            InitializeComponent();
            this.DataContext = new ReadEJDateAndTimeViewModel();
        }
    }
}
