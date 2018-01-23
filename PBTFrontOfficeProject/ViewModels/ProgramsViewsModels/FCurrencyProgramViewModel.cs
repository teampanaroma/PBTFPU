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
    public class FCurrencyProgramViewModel : BaseViewModel
    {

        #region Döviz Adı
        private string _FCurrencyName1;
        public string FCurrencyName1
        {
            get { return _FCurrencyName1; }
            set { _FCurrencyName1 = value; OnPropertyChange("FCurrencyName1"); }
        }

        private string _FCurrencyName2;
        public string FCurrencyName2
        {
            get { return _FCurrencyName2; }
            set { _FCurrencyName2 = value; OnPropertyChange("FCurrencyName2"); }
        }

        private string _FCurrencyName3;
        public string FCurrencyName3
        {
            get { return _FCurrencyName3; }
            set { _FCurrencyName3 = value; OnPropertyChange("FCurrencyName3"); }
        }

        private string _FCurrencyName4;
        public string FCurrencyName4
        {
            get { return _FCurrencyName4; }
            set { _FCurrencyName4 = value; OnPropertyChange("FCurrencyName4"); }
        }

        private string _FCurrencyName5;
        public string FCurrencyName5
        {
            get { return _FCurrencyName5; }
            set { _FCurrencyName5 = value; OnPropertyChange("FCurrencyName5"); }
        }

        private string _FCurrencyName6;
        public string FCurrencyName6
        {
            get { return _FCurrencyName6; }
            set { _FCurrencyName6 = value; OnPropertyChange("FCurrencyName6"); }
        }

        #endregion

        #region Tutar
        //Tutar
        private string _FCurrencyInputAmount1;
        public string FCurrencyInputAmount1
        {
            get { return _FCurrencyInputAmount1; }
            set { _FCurrencyInputAmount1 = value; OnPropertyChange("FCurrencyInputAmount1"); }
        }

        private double FCurrencyAmount1
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount1))
                    return default(double);
                return double.Parse(FCurrencyInputAmount1);
            }
        }

        private string _FCurrencyInputAmount2;
        public string FCurrencyInputAmount2
        {
            get { return _FCurrencyInputAmount2; }
            set { _FCurrencyInputAmount2 = value; OnPropertyChange("FCurrencyInputAmount2"); }
        }

        private double FCurrencyAmount2
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount2))
                    return default(double);
                return double.Parse(FCurrencyInputAmount2);
            }
        }

        private string _FCurrencyInputAmount3;
        public string FCurrencyInputAmount3
        {
            get { return _FCurrencyInputAmount3; }
            set { _FCurrencyInputAmount3 = value; OnPropertyChange("FCurrencyInputAmount3"); }
        }

        private double FCurrencyAmount3
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount3))
                    return default(double);
                return double.Parse(FCurrencyInputAmount3);
            }
        }


        private string _FCurrencyInputAmount4;
        public string FCurrencyInputAmount4
        {
            get { return _FCurrencyInputAmount4; }
            set { _FCurrencyInputAmount4 = value; OnPropertyChange("FCurrencyInputAmount4"); }
        }

        private double FCurrencyAmount4
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount4))
                    return default(double);
                return double.Parse(FCurrencyInputAmount4);
            }
        }

        private string _FCurrencyInputAmount5;
        public string FCurrencyInputAmount5
        {
            get { return _FCurrencyInputAmount5; }
            set { _FCurrencyInputAmount5 = value; OnPropertyChange("FCurrencyInputAmount5"); }
        }

        private double FCurrencyAmount5
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount5))
                    return default(double);
                return double.Parse(FCurrencyInputAmount5);
            }
        }

        private string _FCurrencyInputAmount6;
        public string FCurrencyInputAmount6
        {
            get { return _FCurrencyInputAmount6; }
            set { _FCurrencyInputAmount6 = value; OnPropertyChange("FCurrencyInputAmount6"); }
        }

        private double FCurrencyAmount6
        {
            get
            {
                if (string.IsNullOrEmpty(FCurrencyInputAmount6))
                    return default(double);
                return double.Parse(FCurrencyInputAmount6);
            }
        }
        #endregion




        /// <summary>
        /// Döviz tuşlarını programlamak için kullanılır.
        /// </summary>
        public ICommand FCurrencyDescCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (string.IsNullOrEmpty(FCurrencyName1) || string.IsNullOrEmpty(FCurrencyName2) || string.IsNullOrEmpty(FCurrencyName3) || string.IsNullOrEmpty(FCurrencyName4) || string.IsNullOrEmpty(FCurrencyName5) || string.IsNullOrEmpty(FCurrencyName6))
                    {
                        UIMessage.ShowErrorMessage("Lütfen Tüm Alanları Doldurunuz...");
                    }
                    else
                    {
                        Splasher.Splash = new SplashScreen();
                        Splasher.ShowSplash();

                        AppService.FpuService.SetForeignCurrency(1, FCurrencyName1, FCurrencyAmount1, true);
                        AppService.FpuService.SetForeignCurrency(2, FCurrencyName2, FCurrencyAmount2, true);
                        AppService.FpuService.SetForeignCurrency(3, FCurrencyName3, FCurrencyAmount3, true);
                        AppService.FpuService.SetForeignCurrency(4, FCurrencyName4, FCurrencyAmount4, true);
                        AppService.FpuService.SetForeignCurrency(5, FCurrencyName5, FCurrencyAmount5, true);
                        AppService.FpuService.SetForeignCurrency(6, FCurrencyName6, FCurrencyAmount6, true);
                        AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);

                        Thread.Sleep(2000);
                        Splasher.CloseSplash();
                    }
                });
            }
        }
    }
}
