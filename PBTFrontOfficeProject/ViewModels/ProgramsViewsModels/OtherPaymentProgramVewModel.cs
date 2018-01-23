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
    public class OtherPaymentProgramVewModel : BaseViewModel
    {
        private string _OtherPayment1;

        public string OtherPayment1
        {
            get { return _OtherPayment1; }
            set { _OtherPayment1 = value; OnPropertyChange("OtherPayment1"); }
        }
        private string _OtherPayment2;

        public string OtherPayment2
        {
            get { return _OtherPayment2; }
            set { _OtherPayment2 = value; OnPropertyChange("OtherPayment2"); }
        }
        private string _OtherPayment3;

        public string OtherPayment3
        {
            get { return _OtherPayment3; }
            set { _OtherPayment3 = value; OnPropertyChange("OtherPayment3"); }
        }
        private string _OtherPayment4;

        public string OtherPayment4
        {
            get { return _OtherPayment4; }
            set { _OtherPayment4 = value; OnPropertyChange("OtherPayment4"); }
        }
        private string _OtherPayment5;

        public string OtherPayment5
        {
            get { return _OtherPayment5; }
            set { _OtherPayment5 = value; OnPropertyChange("OtherPayment5"); }
        }
        private string _OtherPayment6;

        public string OtherPayment6
        {
            get { return _OtherPayment6; }
            set { _OtherPayment6 = value; OnPropertyChange("OtherPayment6"); }
        }
        private string _OtherPayment7;

        public string OtherPayment7
        {
            get { return _OtherPayment7; }
            set { _OtherPayment7 = value; OnPropertyChange("OtherPayment7"); }
        }
        private string _OtherPayment8;

        public string OtherPayment8
        {
            get { return _OtherPayment8; }
            set { _OtherPayment8 = value; OnPropertyChange("OtherPayment8"); }
        }

        private string _OtherPayment9;

        public string OtherPayment9
        {
            get { return _OtherPayment9; }
            set { _OtherPayment9 = value; OnPropertyChange("OtherPayment9"); }
        }

        private string _OtherPayment10;

        public string OtherPayment10
        {
            get { return _OtherPayment10; }
            set { _OtherPayment10 = value; OnPropertyChange("OtherPayment10"); }
        }

        /// <summary>
        /// Diğer ödeme tiplerini programlama da TAMAM butonuna bağlı olan command
        /// </summary>
        public ICommand OtherPaymentDescCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    AppService.FpuService.DefAddPayName(0,OtherPayment1);
                    AppService.FpuService.DefAddPayName(1, OtherPayment2);
                    AppService.FpuService.DefAddPayName(2, OtherPayment3);
                    AppService.FpuService.DefAddPayName(3, OtherPayment4);
                    AppService.FpuService.DefAddPayName(4, OtherPayment5);
                    AppService.FpuService.DefAddPayName(5, OtherPayment6);
                    AppService.FpuService.DefAddPayName(6, OtherPayment7);
                    AppService.FpuService.DefAddPayName(7, OtherPayment8);
                    AppService.FpuService.DefAddPayName(8, OtherPayment9);
                    AppService.FpuService.DefAddPayName(9, OtherPayment10);

                    AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);
                    
                  
                });
            }
        }


        internal void ResetInputs()
        {
            OtherPayment1 = null;
            OtherPayment2 = null;
            OtherPayment3= null;
            OtherPayment4 = null;
            OtherPayment5 = null;
            OtherPayment6 = null;
            OtherPayment7 = null;
            OtherPayment8 = null;
            OtherPayment9 = null;
            OtherPayment10 = null;
        }
    }
}
