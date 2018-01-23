using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels
{
    /// <summary>
    /// SingleReceiptEJReportsView bind edilen viewmodel Classıdır.
    /// </summary>
    public class SingleReceiptEJReportsViewModel : BaseViewModel
    {

        /// <summary>
        /// Z numarası ve Fİş Numarası girişi yapılarak ekü den belge okuma işlemini yapar.
        /// </summary>
        public ICommand OpenZAndReceiptNumberCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Belge_Okuma_Z_Fis_No);
                });
            }
        }

        /// <summary>
        /// Tarih-saat girişi yapılarak Ekü kopyası alma ekranını açar
        /// </summary>
        public ICommand OpenDateAndTimeEJViewCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Belge_Okuma_Tarih_Saat);

                });
            }
        }


        /// <summary>
        /// Tarih-saat-fiş no girişi yapılarak Ekü kopyası alma ekranını açar
        /// </summary>
        public ICommand ReadEJDateTimeAndReceiptNumberViewCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Belge_Okuma_Tarih_Saat_Fis_No);

                });
            }
        }

    }
}
