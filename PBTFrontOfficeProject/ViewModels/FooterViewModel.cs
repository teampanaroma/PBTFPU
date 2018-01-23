using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels
{
    public class FooterViewModel : BaseViewModel
    {
        /// <summary>
        /// Ekran Klavyesi Açılış İşlemleri
        /// </summary>
        public ICommand ScreenKeyBoardCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.ScreenKeyboardToggle();
                });
            }
        }


        public ICommand ExitCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.FpuService.ChangeECRMode(1);
                    AppService.MenuService.SetCurrentScreen(EnumScreen.Giris_Ekrani);
                });
            }
        }
    }
}
