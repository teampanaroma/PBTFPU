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
    public class ReadEJDateTimeAndReceiptNumberViewModel : BaseViewModel
    {
        public ReadEJDateTimeAndReceiptNumberViewModel()
        {
            InputDate = DateTime.Now;
        }
        private DateTime? _InputDate;
        private int? _ReceiptNo;
        public DateTime? InputDate
        {
            get { return _InputDate; }
            set { _InputDate = value; OnPropertyChange("InputDate"); }
        }

        public int? ReceiptNo
        {
            get { return _ReceiptNo; }
            set { _ReceiptNo = value; OnPropertyChange("ReceiptNo"); }
        }


        public ICommand ReadEJDateTimeAndReceiptNumberCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();
                    AppService.FpuService.ReadEJSpecificReceiptByDatetimeNo(InputDate.Value.Day.ToString("D2") +
                               InputDate.Value.Month.ToString("D2") +
                               InputDate.Value.Year.ToString().Substring(2, 2) +
                               InputDate.Value.Hour.ToString("D2") +
                               InputDate.Value.Minute.ToString("D2"), ReceiptNo.GetValueOrDefault());
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);

                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                });
            }
        }
    }
}
