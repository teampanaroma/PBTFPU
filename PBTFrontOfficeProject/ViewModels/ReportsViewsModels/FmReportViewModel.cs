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
    /// Mali Hafıza Raporu Menüsünü açar
    /// </summary>
    public class FmReportViewModel : BaseViewModel
    {
        private int? _firstZNumber;
        private int? _lastZNumber;

        /// <summary>
        /// Z numarasına göre mali Hafıza Raporu yazdırmada kullanıcadan alınan Başlangıç Z numarasıdır.
        /// </summary>
        public int? FirstZNumber
        {
            get { return _firstZNumber; }
            set
            { _firstZNumber = value; OnPropertyChange("FirstZNumber"); }
        }

        /// <summary>
        /// Z numarasına göre mali Hafıza Raporu yazdırma da kullanıcıdan alınan son Z numarasıdır.
        /// </summary>
        public int? LastZNumber
        {
            get { return _lastZNumber; }
            set { _lastZNumber = value; OnPropertyChange("FirstZNumber"); }
        }

        /// <summary>
        /// Z numarasına göre Mali Hafıza Raporu yazdırmada kullanılır.
        /// </summary>
        public ICommand FmReportZNumberCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    //AppService.FpuService.OpenFiscalReceipt(1, -1, "1234", "", "", 0);
                    //var b = AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);

                    //var subTotal = FPUFuctions.Instance.Subtotal();
                    //AppService.FpuService.TotalCalculating(0, 1);
                    //var x = AppService.FpuService.CloseFiscalReceipt();

                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.SetModeLogin("0000");//dinamik hale getirilecektir.
                    AppService.FpuService.ReadFMDatailByNumber(FirstZNumber.GetValueOrDefault(), LastZNumber.GetValueOrDefault());
                    AppService.FpuService.ChangeECRMode(1);
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);
                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                    AppService.FpuService.WriteToDisplay("FM RAPORU", "YAZDIRILDI....");

                });
            }
        }

        //public ICommand FmReportZNumberCommand2
        //{
        //    get
        //    {
        //        return new DelegateCommand((obj) =>
        //        {
        //            //Splasher.Splash = new SplashScreen();
        //            //Splasher.ShowSplash();
        //            //AppService.FpuService.SetModeLogin("0000");//dinamik hale getirilecektir.
        //            //AppService.FpuService.ReadFMDatailByNumber(FirstZNumber.GetValueOrDefault(), LastZNumber.GetValueOrDefault());
        //            //AppService.FpuService.ChangeECRMode(1);
        //            //AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

        //            //Thread.Sleep(1000);
        //            //Splasher.CloseSplash();
        //            AppService.FpuService.WriteToDisplay("BEDİR HOLDİNG", "GURURLA SUNAR");
        //            for (int i = 0; i < 1250; i++)
        //            {
        //                AppService.FpuService.CashierLogin(1, "0000");
        //                AppService.FpuService.OpenFiscalReceipt(1, -1, "0000", "");
        //                AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 3, 0, 1);
        //                AppService.FpuService.PreAndDiscount("D", 1);
        //                
        //                AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
        //                AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
        //                var subTotal = FPUFuctions.Instance.Subtotal();
        //                AppService.FpuService.TotalCalculating(0, subTotal);
        //                AppService.FpuService.CloseFiscalReceipt();
        //            }


        //        });
        //    }
        //}
    }
}
