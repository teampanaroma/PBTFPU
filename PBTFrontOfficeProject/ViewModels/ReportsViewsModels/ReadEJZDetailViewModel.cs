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
    /// ReadEJZDetailView ına bağlı olan viewmodel classıdır
    /// </summary>
    public class ReadEJZDetailViewModel : BaseViewModel
    {

        /// <summary>
        /// Ekü Z Detay Raporu Yazdırma da TAMAM butonuna bağlı olan command
        /// </summary>
        public ICommand ReadEJZDetailCommand
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
    }
}
