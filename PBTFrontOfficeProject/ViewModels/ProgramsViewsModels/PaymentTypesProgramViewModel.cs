using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ProgramsViewsModels
{
    public class PaymentTypesProgramViewModel:BaseViewModel
    {
        /// Diğer ödeme tipleri viewını açar
        /// </summary>
        public ICommand OtherPaymentProgramCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Diger_Odeme_Programlama);                    
                });
            }
        }

        /// Döviz ödeme tipleri viewını açar
        /// </summary>
        public ICommand ForeignCurrencyProgramCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Doviz_Programlama);
                });
            }
        }
    }
}
