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
    public class TaxIdProgramViewModel : BaseViewModel
    {
        private double? _TaxId1;
        private double? _TaxId2;
        private double? _TaxId3;
        private double? _TaxId4;
        private double? _TaxId5;
        private double? _TaxId6;
        private double? _TaxId7;
        private double? _TaxId8;
        private FPUReadTax taxReturn;

        public TaxIdProgramViewModel()
        {
            taxReturn = AppService.FpuService.UploadTaxRate();
            TaxId1 = taxReturn.TaxAmount1;
            TaxId2 = taxReturn.TaxAmount2;
            TaxId3 = taxReturn.TaxAmount3;
            TaxId4 = taxReturn.TaxAmount4;
            TaxId5 = taxReturn.TaxAmount5;
            TaxId6 = taxReturn.TaxAmount6;
            TaxId7 = taxReturn.TaxAmount7;
            TaxId8 = taxReturn.TaxAmount8;
        }
        public double? TaxId1
        {
            get { return _TaxId1; }
            set { _TaxId1 = value; OnPropertyChange("TaxId1"); }
        }
        public double? TaxId2
        {
            get { return _TaxId2; }
            set { _TaxId2 = value; OnPropertyChange("TaxId2"); }
        }
        public double? TaxId3
        {
            get { return _TaxId3; }
            set { _TaxId3 = value; OnPropertyChange("TaxId3"); }
        }
        public double? TaxId4
        {
            get { return _TaxId4; }
            set { _TaxId4 = value; OnPropertyChange("TaxId4"); }
        }
        public double? TaxId5
        {
            get { return _TaxId5; }
            set { _TaxId5 = value; OnPropertyChange("TaxId5"); }
        }
        public double? TaxId6
        {
            get { return _TaxId6; }
            set { _TaxId6 = value; OnPropertyChange("TaxId6"); }
        }
        public double? TaxId7
        {
            get { return _TaxId7; }
            set { _TaxId7 = value; OnPropertyChange("TaxId7"); }
        }
        public double? TaxId8
        {
            get { return _TaxId8; }
            set { _TaxId8 = value; OnPropertyChange("TaxId8"); }
        }

        /// <summary>
        /// Vergi oranlarını programlamak için kullanılır.
        /// </summary>
        public ICommand TaxIdProgramCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    //AppService.FpuService.ExpenseslipInfo(1, 0, 23, 0, 23, 0, 0, 0, 0, 0);
                    Splasher.Splash = new SplashScreen();
                    Splasher.ShowSplash();
                    AppService.FpuService.SettingTaxRate(TaxId1.GetValueOrDefault(), TaxId2.GetValueOrDefault(), TaxId3.GetValueOrDefault(), TaxId4.GetValueOrDefault(), TaxId5.GetValueOrDefault(), TaxId6.GetValueOrDefault(), TaxId7.GetValueOrDefault(), TaxId8.GetValueOrDefault());
                    Thread.Sleep(1000);
                    Splasher.CloseSplash();
                    //AppService.FpuService.CashierLogin(1, "0000");
                    //AppService.FpuService.OpenFiscalReceipt(1, 0, "0000");
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);
                    //AppService.FpuService.Registeringsale("asd", "", "123", "", 1, 1, 1, 1, 0, 1);

                    ////AppService.FpuService.CancelFiscalReceipt();
                    //var x = AppService.FpuService.Subtotal();
                    //AppService.FpuService.TotalCalculating(0, x);
                    //AppService.FpuService.CloseFiscalReceipt();

                    //AppService.FpuService.to

                    AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);
                    AppService.FpuService.WriteToDisplay("KDV", "YAZDIRILDI....");

                });
            }
        }
    }
}
