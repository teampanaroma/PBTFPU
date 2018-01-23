using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.EkuTekBelgeViewModels
{
    public class ReadEJDateAndTimeViewModel : BaseViewModel
    {
        private DateTime? _InputDate;
        public DateTime? InputDate { get { return _InputDate; } set { _InputDate = value; OnPropertyChange("InputDate"); } }
        public ReadEJDateAndTimeViewModel()
        {
            InputDate = DateTime.Now;
        }

        //string dateFormat = InputDate.Value.Day.ToString("D2") +
        //                       InputDate.Value.Month.ToString("D2") +
        //                       InputDate.Value.Year.ToString().Substring(2, 2) + //DDMMYYHHMMDDMMYYHHMM
        //                       InputDate.Value.Hour.ToString("D2") +
        //                       InputDate.Value.Minute.ToString("D2");

        public ICommand SingleEjDateTimeCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();
                    //DDMMYYHHMMDDMMYYHHMM
                    AppService.FpuService.ReadEJSpecificReceiptByDatetime(InputDate.Value.Day.ToString("D2") +
                               InputDate.Value.Month.ToString("D2") +
                               InputDate.Value.Year.ToString().Substring(2, 2) + 
                               InputDate.Value.Hour.ToString("D2") +
                               InputDate.Value.Minute.ToString("D2"));
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();

                });
            }
        }
    }
}
