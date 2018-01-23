using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.EkuDonemselBelgeViewModels
{
    /// <summary>
    /// Eküden Dönemsel Belge Okuma Menüsünün view modeli
    /// </summary>
    public class PeriodicReceiptEJReportsViewModel : BaseViewModel
    {
        /// <summary>
        /// İki Z No İki Fiş No Menusunu Açar.
        /// </summary>
        public ICommand OpenReadTwoZTwoReceiptMenuCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Donemsel_Belge_Okuma_Iki_Z_No_Iki_Fis_No);
                });
            }
        }

        /// <summary>
        /// İki Tarih İki Saat Menusunu Açar.
        /// </summary>
        public ICommand OpenReadTwoDateTwoReceiptMenuCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetReportsScreen(VReports.EnumView.Ekuden_Donemsel_Belge_Okuma_Iki_Tarih_Iki_Saat);
                });
            }
        }
    }
}
