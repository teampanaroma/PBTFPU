using PBTFrontOfficeProject.Controls.SplashScreen;
using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ProgramsViewsModels
{
    public class DepartmentsProgramViewModel : BaseViewModel
    {
        private FPUReadTax taxReturn;
        public DepartmentsProgramViewModel()
        {
            taxReturn = AppService.FpuService.UploadTaxRate();
            TaxAvailable = new List<FPUTaxOperation>();

            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount1, TaxName = "A" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount2, TaxName = "B" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount3, TaxName = "C" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount4, TaxName = "D" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount5, TaxName = "E" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount6, TaxName = "F" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount7, TaxName = "G" });
            TaxAvailable.Add(new FPUTaxOperation { TaxAmount = taxReturn.TaxAmount8, TaxName = "H" });
            SelectedTax1 = TaxAvailable.FirstOrDefault();
            SelectedTax2 = TaxAvailable.FirstOrDefault();
            SelectedTax3 = TaxAvailable.FirstOrDefault();
            SelectedTax4 = TaxAvailable.FirstOrDefault();
            SelectedTax5 = TaxAvailable.FirstOrDefault();
            SelectedTax6 = TaxAvailable.FirstOrDefault();
            SelectedTax7 = TaxAvailable.FirstOrDefault();
            SelectedTax8 = TaxAvailable.FirstOrDefault();
            SelectedTax9 = TaxAvailable.FirstOrDefault();
            SelectedTax10 = TaxAvailable.FirstOrDefault();
            SelectedTax11 = TaxAvailable.FirstOrDefault();
            SelectedTax12 = TaxAvailable.FirstOrDefault();

            //var dep1=AppService.FpuService.ReadDepartments(1);
            //if (dep1.ErrorCode==0)
            //{
            //    DepartmentName1 = dep1.DepName;
            //}

            //var dep2 = AppService.FpuService.ReadDepartments(2);
            //if (dep2.ErrorCode == 0)
            //{
            //    DepartmentName2 = dep2.DepName;
            //}

            //var dep3 = AppService.FpuService.ReadDepartments(3);
            //if (dep3.ErrorCode == 0)
            //{
            //    DepartmentName3 = dep3.DepName;
            //}

            //var dep4 = AppService.FpuService.ReadDepartments(4);
            //if (dep4.ErrorCode == 0)
            //{
            //    DepartmentName4 = dep4.DepName;
            //}

            //var dep5 = AppService.FpuService.ReadDepartments(5);
            //if (dep5.ErrorCode==0)
            //{
            //    DepartmentName5 = dep5.DepName;
            //}

            //var dep6 = AppService.FpuService.ReadDepartments(6);
            //if (dep6.ErrorCode == 0)
            //{
            //    DepartmentName6 = dep6.DepName;
            //}

            //var dep7 = AppService.FpuService.ReadDepartments(7);
            //if (dep7.ErrorCode == 0)
            //{
            //    DepartmentName7 = dep7.DepName;
            //}


            //var dep8 = AppService.FpuService.ReadDepartments(8);
            //if (dep8.ErrorCode == 0)
            //{
            //    DepartmentName8 = dep8.DepName;
            //}

            //var dep9 = AppService.FpuService.ReadDepartments(9);
            //if (dep9.ErrorCode == 0)
            //{
            //    DepartmentName9 = dep9.DepName;
            //}

            //var dep10 = AppService.FpuService.ReadDepartments(10);
            //if (dep10.ErrorCode == 0)
            //{
            //    DepartmentName10 = dep10.DepName;
            //}

            //var dep11 = AppService.FpuService.ReadDepartments(11);
            //if (dep11.ErrorCode == 0)
            //{
            //    DepartmentName11 = dep11.DepName;
            //}

            //var dep12 = AppService.FpuService.ReadDepartments(12);
            //if (dep12.ErrorCode == 0)
            //{
            //    DepartmentName12 = dep12.DepName;
            //}
         

        }
        private List<FPUTaxOperation> _TaxAvailable;

        public List<FPUTaxOperation> TaxAvailable
        {
            get { return _TaxAvailable; }
            set { _TaxAvailable = value; OnPropertyChange("TaxAvailable"); }
        }

        #region SelectedTax
        private FPUTaxOperation _selectedTax1;
        public FPUTaxOperation SelectedTax1
        {
            get { return _selectedTax1; }
            set
            {
                if (_selectedTax1 != value)
                {
                    _selectedTax1 = value;
                    OnPropertyChange("SelectedTax1");
                }
            }
        }

        private FPUTaxOperation _selectedTax2;
        public FPUTaxOperation SelectedTax2
        {
            get { return _selectedTax2; }
            set
            {
                if (_selectedTax2 != value)
                {
                    _selectedTax2 = value;
                    OnPropertyChange("SelectedTax2");
                }
            }
        }

        private FPUTaxOperation _selectedTax3;
        public FPUTaxOperation SelectedTax3
        {
            get { return _selectedTax3; }
            set
            {
                if (_selectedTax3 != value)
                {
                    _selectedTax3 = value;
                    OnPropertyChange("SelectedTax3");
                }
            }
        }

        private FPUTaxOperation _selectedTax4;
        public FPUTaxOperation SelectedTax4
        {
            get { return _selectedTax4; }
            set
            {
                if (_selectedTax4 != value)
                {
                    _selectedTax4 = value;
                    OnPropertyChange("SelectedTax4");
                }
            }
        }

        private FPUTaxOperation _selectedTax5;
        public FPUTaxOperation SelectedTax5
        {
            get { return _selectedTax5; }
            set
            {
                if (_selectedTax5 != value)
                {
                    _selectedTax5 = value;
                    OnPropertyChange("SelectedTax5");
                }
            }
        }

        private FPUTaxOperation _selectedTax6;
        public FPUTaxOperation SelectedTax6
        {
            get { return _selectedTax6; }
            set
            {
                if (_selectedTax6 != value)
                {
                    _selectedTax6 = value;
                    OnPropertyChange("SelectedTax6");
                }
            }
        }

        private FPUTaxOperation _selectedTax7;
        public FPUTaxOperation SelectedTax7
        {
            get { return _selectedTax7; }
            set
            {
                if (_selectedTax7 != value)
                {
                    _selectedTax7 = value;
                    OnPropertyChange("SelectedTax7");
                }
            }
        }

        private FPUTaxOperation _selectedTax8;
        public FPUTaxOperation SelectedTax8
        {
            get { return _selectedTax8; }
            set
            {
                if (_selectedTax8 != value)
                {
                    _selectedTax8 = value;
                    OnPropertyChange("SelectedTax8");
                }
            }
        }

        private FPUTaxOperation _selectedTax9;
        public FPUTaxOperation SelectedTax9
        {
            get { return _selectedTax9; }
            set
            {
                if (_selectedTax9 != value)
                {
                    _selectedTax9 = value;
                    OnPropertyChange("SelectedTax9");
                }
            }
        }

        private FPUTaxOperation _selectedTax10;
        public FPUTaxOperation SelectedTax10
        {
            get { return _selectedTax10; }
            set
            {
                if (_selectedTax10 != value)
                {
                    _selectedTax10 = value;
                    OnPropertyChange("SelectedTax10");
                }
            }
        }

        private FPUTaxOperation _selectedTax11;
        public FPUTaxOperation SelectedTax11
        {
            get { return _selectedTax11; }
            set
            {
                if (_selectedTax11 != value)
                {
                    _selectedTax11 = value;
                    OnPropertyChange("SelectedTax11");
                }
            }
        }

        private FPUTaxOperation _selectedTax12;
        public FPUTaxOperation SelectedTax12
        {
            get { return _selectedTax12; }
            set
            {
                if (SelectedTax12 != value)
                {
                    _selectedTax12 = value;
                    OnPropertyChange("SelectedTax12");
                }
            }
        }
        #endregion

        #region DepartmentName Prop&Fields
        private string _DepartmentName1;
        public string DepartmentName1
        {
            get { return _DepartmentName1; }
            set { _DepartmentName1 = value; OnPropertyChange("DepartmentName1"); }
        }

        private string _DepartmentName2;
        public string DepartmentName2
        {
            get { return _DepartmentName2; }
            set { _DepartmentName2 = value; OnPropertyChange("DepartmentName2"); }
        }

        private string _DepartmentName3;
        public string DepartmentName3
        {
            get { return _DepartmentName3; }
            set { _DepartmentName3 = value; OnPropertyChange("DepartmentName3"); }
        }

        private string _DepartmentName4;
        public string DepartmentName4
        {
            get { return _DepartmentName4; }
            set { _DepartmentName4 = value; OnPropertyChange("DepartmentName4"); }
        }

        private string _DepartmentName5;
        public string DepartmentName5
        {
            get { return _DepartmentName5; }
            set { _DepartmentName5 = value; OnPropertyChange("DepartmentName5"); }
        }

        private string _DepartmentName6;
        public string DepartmentName6
        {
            get { return _DepartmentName6; }
            set { _DepartmentName6 = value; OnPropertyChange("DepartmentName6"); }
        }

        private string _DepartmentName7;
        public string DepartmentName7
        {
            get { return _DepartmentName7; }
            set { _DepartmentName7 = value; OnPropertyChange("DepartmentName7"); }
        }

        private string _DepartmentName8;
        public string DepartmentName8
        {
            get { return _DepartmentName8; }
            set { _DepartmentName8 = value; OnPropertyChange("DepartmentName8"); }
        }

        private string _DepartmentName9;
        public string DepartmentName9
        {
            get { return _DepartmentName9; }
            set { _DepartmentName9 = value; OnPropertyChange("DepartmentName9"); }
        }

        private string _DepartmentName10;
        public string DepartmentName10
        {
            get { return _DepartmentName10; }
            set { _DepartmentName10 = value; OnPropertyChange("DepartmentName10"); }
        }

        private string _DepartmentName11;
        public string DepartmentName11
        {
            get { return _DepartmentName11; }
            set { _DepartmentName11 = value; OnPropertyChange("DepartmentName11"); }
        }

        private string _DepartmentName12;
        public string DepartmentName12
        {
            get { return _DepartmentName12; }
            set { _DepartmentName12 = value; OnPropertyChange("DepartmentName12"); }
        }
        #endregion


        /// <summary>
        /// Departman programlama butonuna bind edilen commanddır
        /// </summary>
        public ICommand DepartmentProgramCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (string.IsNullOrEmpty(DepartmentName1) || string.IsNullOrEmpty(DepartmentName2) || string.IsNullOrEmpty(DepartmentName3) || string.IsNullOrEmpty(DepartmentName4) || string.IsNullOrEmpty(DepartmentName5) || string.IsNullOrEmpty(DepartmentName6) || string.IsNullOrEmpty(DepartmentName7) || string.IsNullOrEmpty(DepartmentName8) || string.IsNullOrEmpty(DepartmentName9) || string.IsNullOrEmpty(DepartmentName10) || string.IsNullOrEmpty(DepartmentName11) || string.IsNullOrEmpty(DepartmentName12))
                    {
                        UIMessage.ShowInfoMessage("Lütfen Departman Adını Boş Bırakmayınız.");
                    }
                    else
                    {
                        Splasher.Splash = new SplashScreen();
                        Splasher.ShowSplash();
                        AppService.FpuService.SetDepartments(1, SelectedTax1.TaxName, 99999999, 1, DepartmentName1, "");
                        AppService.FpuService.SetDepartments(2, SelectedTax2.TaxName, 99999999, 1, DepartmentName2, "");
                        AppService.FpuService.SetDepartments(3, SelectedTax3.TaxName, 99999999, 1, DepartmentName3, "");
                        AppService.FpuService.SetDepartments(4, SelectedTax4.TaxName, 99999999, 1, DepartmentName4, "");
                        AppService.FpuService.SetDepartments(5, SelectedTax5.TaxName, 99999999, 1, DepartmentName5, "");
                        AppService.FpuService.SetDepartments(6, SelectedTax6.TaxName, 99999999, 1, DepartmentName6, "");
                        AppService.FpuService.SetDepartments(7, SelectedTax7.TaxName, 99999999, 1, DepartmentName7, "");
                        AppService.FpuService.SetDepartments(8, SelectedTax8.TaxName, 99999999, 1, DepartmentName8, "");
                        AppService.FpuService.SetDepartments(9, SelectedTax9.TaxName, 99999999, 1, DepartmentName9, "");
                        AppService.FpuService.SetDepartments(10, SelectedTax10.TaxName, 99999999, 1, DepartmentName10, "");
                        AppService.FpuService.SetDepartments(11, SelectedTax11.TaxName, 99999999, 1, DepartmentName11, "");
                        AppService.FpuService.SetDepartments(12, SelectedTax12.TaxName, 99999999, 1, DepartmentName12, "");
                        for (int i = 1; i <= 12; i++)
                        {
                            AppService.FpuService.SetGroup(i, i, "Grup", "");
                        }
                        Thread.Sleep(3000);
                        Splasher.CloseSplash();
                        AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Bos_Ekran);
                        AppService.FpuService.WriteToDisplay("DEPARTMANLAR", "YAZDIRILDI....");

                    }
                });
            }
        }

    }
}
