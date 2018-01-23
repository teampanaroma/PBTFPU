using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ReportsViewsModels
{
    public class CashOutViewModel:BaseViewModel
    {
        private string _cashOutAmount;

        /// <summary>
        /// Kasa giriş çıkış için girilen değeri alır.
        /// (-) girildiyse çıkış, (+) girildiyse giriş kabul edilir.
        /// </summary>
        public string CashOutAmount
        {
            get { return _cashOutAmount; }
            set { _cashOutAmount = value; OnPropertyChange("CashOutAmount"); }
        }

        public double? CashAmount
        {
            get
            {
                if (string.IsNullOrEmpty(CashOutAmount))
                    return default(double);
                return double.Parse(CashOutAmount);
            }
        }

        /// <summary>
        /// Kasa  çıkış Tamam butonuna bind edilen commanddır.
        /// </summary>
        public ICommand CashOutCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {

                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.CashInCashOut(-CashAmount.GetValueOrDefault());
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                    AppService.FpuService.WriteToDisplay("CIKIS ISLEMI", "YAZDIRILDI....");

                });
            }
        }
    }
}
