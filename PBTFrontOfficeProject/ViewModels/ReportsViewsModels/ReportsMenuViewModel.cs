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
    public class ReportsMenuViewModel : BaseViewModel
    {

        /// <summary>
        /// Raporlama ekranındaki butonlardan gelen commandı yakalar.
        /// </summary>
        public ICommand FunctionCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {

                    //AppService.FpuService.CashierLogin(1, "0000");
                    //AppService.FpuService.OpenFiscalReceipt(1, 3, "0000", "1232131312", "",0);
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 3, 0, 1);
                    //AppService.FpuService.PreAndDiscount("D", 1);

                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
                    //var subTotal = FPUFuctions.Instance.Subtotal();
                    //AppService.FpuService.TotalCalculating(0, subTotal);
                    //AppService.FpuService.CloseFiscalReceipt();

                    switch (obj.ToString())
                    {
                        case "xReport":
                            Splasher.Splash = new SplashScreen();
                            Splasher.ShowSplash();
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);
                            AppService.FpuService.XReport();
                            Thread.Sleep(1000);
                            Splasher.CloseSplash();
                            AppService.FpuService.WriteToDisplay("X RAPORU", "YAZDIRILDI....");
                            break;
                        case "zReport":
                            Splasher.Splash = new SplashScreen();
                            Splasher.ShowSplash();
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);
                            AppService.FpuService.SetModeLogin("0000");//Dinamik hale getirilecektir.
                            var result = AppService.FpuService.ZReport();
                            if (String.IsNullOrEmpty(result))
                            {
                                UIMessage.ShowInfoMessage("GÜNLÜK Z RAPORU LİMİTİNİ AŞTIĞINIZ İÇİN RAPOR YAZDIRILAMADI");
                                AppService.FpuService.WriteToDisplay("RAPOR", "YAZDIRILAMADI....");
                                AppService.FpuService.ChangeECRMode(1);
                                Splasher.CloseSplash();
                                break;
                            }
                            else
                            {
                                //test
                                AppService.FpuService.WriteToDisplay("Z RAPORU", "YAZDIRILDI....");
                                AppService.FpuService.ChangeECRMode(1);
                                Thread.Sleep(5000);
                                Splasher.CloseSplash();
                                break;
                            }
                        case "cashIn":
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Kasa_Giris_Cikis);
                            break;
                        case "cashOut":
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Kasa_Cikis);
                            break;
                        case "fmReport":
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Mali_Hafiza_Raporu);
                            break;
                        case "ejOperations":
                            AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Eku_Islemleri);
                            break;
                        default:
                            break;
                    }

                });
            }
        }


        /// <summary>
        /// Yan ekran değiştirme işlemi için kullanılır.
        /// </summary>
        int _CurrentContentScreenIndex;
        public int CurrentContentScreenIndex
        {
            get { return _CurrentContentScreenIndex; }
            set { _CurrentContentScreenIndex = value; OnPropertyChange("CurrentContentScreenIndex"); }
        }
        public ReportsMenuViewModel()
        {
            AppService.MenuContentService.ScreenChanged += MenuContentService_ScreenChanged;

        }




        private void MenuContentService_ScreenChanged(object sender, IMenuContainer e)
        {
            CurrentContentScreenIndex = e.ViewIndex;
        }

    }
}
