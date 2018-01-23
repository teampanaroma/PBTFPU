using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.EkuDonemselBelgeViewModels
{
    /// <summary>
    /// İki Z No İki Fiş No Eküden dönemsel belge okuma
    /// </summary>
    public class ReadEJTwoZTwoReceiptViewModel : BaseViewModel
    {
        private int? _FirstReceiptNumber;

        private int? _LastReceiptNumber;

        private int? _FirstZNumber;

        private int? _LastZNumber;

        public int? FirstReceiptNumber
        {
            get { return _FirstReceiptNumber; }
            set { _FirstReceiptNumber = value; OnPropertyChange("FirstReceiptNumber"); }
        }

        public int? LastReceiptNumber
        {
            get { return _LastReceiptNumber; }
            set { _LastReceiptNumber = value; OnPropertyChange("LastReceiptNumber"); }
        }

        public int? FirstZNumber
        {
            get { return _FirstZNumber; }
            set { _FirstZNumber = value; OnPropertyChange("FirstZNumber"); }
        }

        public int? LastZNumber
        {
            get { return _LastZNumber; }
            set { _LastZNumber = value; OnPropertyChange("LastZNumber"); }
        }


        public ICommand ReadEJTwoZTwoReceiptCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.ReadEJSpecificReceiptByNumberToNumber(FirstZNumber.GetValueOrDefault(), LastZNumber.GetValueOrDefault(), FirstReceiptNumber.GetValueOrDefault(), LastReceiptNumber.GetValueOrDefault());
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();

                });
            }
        }
    }
}
