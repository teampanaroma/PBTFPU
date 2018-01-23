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
    public class ReceiptEndMessageProgramViewModel : BaseViewModel
    {
        private string _Message1;
        private string _Message2;

        public string Message1
        {
            get { return _Message1; }
            set { _Message1 = value; OnPropertyChange("Message1"); }
        }
        public string Message2
        {
            get { return _Message2; }
            set { _Message2 = value; OnPropertyChange("Message2"); }
        }

        public ReceiptEndMessageProgramViewModel()
        {
            AppService.FpuService.SetModeLogin("0000");
            Message1 = AppService.FpuService.ReadFootMessage(1);
            Message2 = AppService.FpuService.ReadFootMessage(2);
            AppService.FpuService.ChangeECRMode(1);
        }

        public ICommand ReceiptEndMessageCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (!String.IsNullOrEmpty(Message1) && !String.IsNullOrEmpty(Message2))
                    {
                        Splasher.Splash = new SplashScreen();
                        Splasher.ShowSplash();

                        AppService.FpuService.SettingFootMessage(1, Message1);
                        AppService.FpuService.SettingFootMessage(2, Message2);
                        AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);

                        Thread.Sleep(1000);
                        Splasher.CloseSplash();
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
