using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels
{
    /// <summary>
    /// Kasa giriş-çıkış view bind edilen commanddır.
    /// </summary>
    public class CashInOutViewModel : BaseViewModel
    {
        private string _inputCashAmount;

        /// <summary>
        /// Kasa giriş çıkış için girilen değeri alır.
        /// (-) girildiyse çıkış, (+) girildiyse giriş kabul edilir.
        /// </summary>
        public string InputCashAmount
        {
            get { return _inputCashAmount; }
            set { _inputCashAmount = value; OnPropertyChange("InputCashAmount"); }
        }

        public double? CashAmount
        {
            get {
                if (string.IsNullOrEmpty(InputCashAmount))
                    return default(double);
                return double.Parse(InputCashAmount); }
        }

        /// <summary>
        /// Kasa giriş çıkış Tamam butonuna bind edilen commanddır.
        /// </summary>
        public ICommand CashInOutCommand
        {
            get
            
            {
                return new DelegateCommand((obj) =>
                {

                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.CashInCashOut(CashAmount.GetValueOrDefault());
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                    AppService.FpuService.WriteToDisplay("GIRIS ISLEMI", "YAZDIRILDI....");

                });
            }
        }
    }
}
