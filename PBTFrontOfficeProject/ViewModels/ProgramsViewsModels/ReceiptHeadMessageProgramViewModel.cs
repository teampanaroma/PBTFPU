using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ProgramsViewsModels
{
    public class ReceiptHeadMessageProgramViewModel : BaseViewModel
    {

        /// <summary>
        /// 1.
        /// </summary>
        public string Message1 { get; set; }

        /// <summary>
        /// 2.
        /// </summary>
        public string Message2 { get; set; }

        /// <summary>
        /// 3.
        /// </summary>
        public string Message3 { get; set; }

        /// <summary>
        /// 4.
        /// </summary>
        public string Message4 { get; set; }

        /// <summary>
        /// 5.
        /// </summary>
        public string Message5 { get; set; }

        /// <summary>
        /// 6.
        /// </summary>
        public string Message6 { get; set; }

        /// <summary>
        /// 7.
        /// </summary>
        public string Message7 { get; set; }

        /// <summary>
        /// 8.
        /// </summary>
        public string Message8 { get; set; }


        /// <summary>
        /// Fiş Başlığı Programla butonuna bind edilen Command
        /// </summary>
        public ICommand ReceiptHeadProgramCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (!String.IsNullOrEmpty(Message1) && !String.IsNullOrEmpty(Message2) && !String.IsNullOrEmpty(Message3) && !String.IsNullOrEmpty(Message4) && !String.IsNullOrEmpty(Message5) && !String.IsNullOrEmpty(Message6) && !String.IsNullOrEmpty(Message7) && !String.IsNullOrEmpty(Message8)  )
                    {
                        Splasher.Splash = new SplashScreen();
                        Splasher.ShowSplash();
                        FPUService.SettingHeaderMessage(1, Message1);
                        FPUService.SettingHeaderMessage(2, Message2);
                        FPUService.SettingHeaderMessage(3, Message3);
                        FPUService.SettingHeaderMessage(4, Message4);
                        FPUService.SettingHeaderMessage(5, Message5);
                        FPUService.SettingHeaderMessage(6, Message6);
                        FPUService.SettingHeaderMessage(7, Message7);
                        FPUService.SettingHeaderMessage(8, Message8);
                        AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);

                        Thread.Sleep(1000);
                        Splasher.CloseSplash();
                        AppService.FpuService.WriteToDisplay("FIS BASLIGI", "YAZDIRILDI....");
                    }
                    else 
                    {
                        UIMessage.ShowInfoMessage("Lütfen Tüm Alanları Doldurunuz....");
                    }

                   

                


                });
            }
        }
    }
}
