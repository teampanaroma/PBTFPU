using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.EkuDonemselBelgeViewModels
{
    public class ReadEJTwoDateTwoTimeViewModel : BaseViewModel
    {
        public ReadEJTwoDateTwoTimeViewModel()
        {
            FirstSelectedDate = DateTime.Now;
            LastSelectedDate = DateTime.Now;
        }
        private DateTime? _FirstSelectedDate;

        public DateTime? FirstSelectedDate
        {
            get { return _FirstSelectedDate; }
            set { _FirstSelectedDate = value; OnPropertyChange("FirstSelectedDate"); }
        }
        private DateTime? _LastSelectedDate;

        public DateTime? LastSelectedDate
        {
            get { return _LastSelectedDate; }
            set { _LastSelectedDate = value; OnPropertyChange("LastSelectedDate"); }
        }

        /// <summary>
        /// İki tarih arası eküden belge yazdırma işlemini yapar
        /// </summary>
        public ICommand ReadEJDateTwoTimeCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.FpuService.ReadEJSummaryByDate(FirstSelectedDate.Value.Day.ToString("D2") +
                               FirstSelectedDate.Value.Month.ToString("D2") +
                               FirstSelectedDate.Value.Year.ToString().Substring(2, 2) +
                               FirstSelectedDate.Value.Hour.ToString("D2") +
                               FirstSelectedDate.Value.Minute.ToString("D2"), LastSelectedDate.Value.Day.ToString("D2") +
                               LastSelectedDate.Value.Month.ToString("D2") +
                               LastSelectedDate.Value.Year.ToString().Substring(2, 2) +
                               LastSelectedDate.Value.Hour.ToString("D2") +
                               LastSelectedDate.Value.Minute.ToString("D2"));
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Bos_Ekran);
                });
            }
        }


    }
}
