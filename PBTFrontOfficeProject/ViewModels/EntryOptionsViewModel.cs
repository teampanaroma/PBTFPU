using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels
{
    /// <summary>
    /// Giriş ekranı ViewModel
    /// </summary>
    public class EntryOptionsViewModel : BaseViewModel
    {
        /// <summary>
        /// Raporlama ekranına Geçişi Sağlar.
        /// </summary>
        public ICommand OpenReportMenuCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.MenuService.SetCurrentScreen(EnumScreen.Rapor_Menusu);
                });
            }
        }

        /// <summary>
        /// Programlama ekranına Geçişi Sağlar.
        /// </summary>
        public ICommand OpenProgramMenuCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.FpuService.SetModeLogin("0000");//Dinamik method uygulanacaktır.
                    AppService.MenuService.SetCurrentScreen(EnumScreen.Program_Menusu);
                });
            }
        }

        /// <summary>
        /// Uygulamayı Kapatır.
        /// </summary>
        public ICommand CloseAppCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    ///Farklı bir uygulamadan dll olarak çalıştırıldıgı için bu method kullanılıyor.
                    AppService.CurrentWindow.Hide();

                });
            }
        }

        //private void log(string txt, StringBuilder sb)
        //{
        //    sb.AppendLine(string.Format("{0} - {1}", txt, DateTime.Now.TimeOfDay));
        //}
    }
}
