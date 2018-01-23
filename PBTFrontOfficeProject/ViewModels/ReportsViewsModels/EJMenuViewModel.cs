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
    /// Ekü Menüsüne bin edilmiş viewmodel classıdır.
    /// </summary>
    public class EJMenuViewModel : BaseViewModel
    {

        /// <summary>
        /// Ekü detay raporu yazdırma viewını açar
        /// </summary>
        public ICommand EJDetailCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.EJModuleReport();
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                });
            }
        }

        /// <summary>
        /// Z numarasına göre ekü detay raporu yazdırma viewını açar
        /// </summary>
        public ICommand EJZDetailCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();

                    AppService.FpuService.EJZInfoReport();
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                });
            }
        }

        /// <summary>
        /// Eküden Tek Belge Okuma Menüsünü Açar
        /// </summary>
        public ICommand OpenSingleReceiptEJReportsViewCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Eku_Tek_Belge_Okuma_Menusu);
                });
            }
        }


        /// <summary>
        /// Eküden Dönemsel Belge Okuma menüsünü açar.
        /// </summary>
        public ICommand OpenPeriodicReceiptEJReportsCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Donemsel_Belge_Okuma);
                });
            }
        }

    }
}
