﻿using PBTFrontOfficeProject.ViewModels.EkuDonemselBelgeViewModels;
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

namespace PBTFrontOfficeProject.Views.ContentViews.EkuDonemselBelgeViews
{
    /// <summary>
    /// Interaction logic for ReadEJTwoZTwoReceiptView.xaml
    /// </summary>
    public partial class ReadEJTwoZTwoReceiptView : UserControl
    {
        public ReadEJTwoZTwoReceiptView()
        {
            InitializeComponent();
            this.DataContext = new ReadEJTwoZTwoReceiptViewModel();
        }
    }
}
