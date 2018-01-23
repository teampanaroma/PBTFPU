using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ReportsViewsModels.EkuTekBelgeViewModels
{
    public class ZAndReceiptNumberViewModel : BaseViewModel
    {
        private int? _ReceiptNumber;
        private int? _ZNumber;

        public int? ReceiptNumber
        {
            get { return _ReceiptNumber; }
            set { _ReceiptNumber = value; OnPropertyChange("ReceiptNumber"); }
        }

        public int? ZNumber
        {
            get { return _ZNumber; }
            set { _ZNumber = value; OnPropertyChange("ZNumber"); }
        }

        /// <summary>
        /// Eküden Z numarası ve Fiş numarası girilerek belge okuma işlemi gerçekleştirilir.
        /// </summary>
        public ICommand EjSingleZAndReceiptNumberCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.ReadEJSpecificReceiptByNumber(ZNumber.GetValueOrDefault(),ReceiptNumber.GetValueOrDefault());
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();

                });
            }
        }
    }
}
