using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels.ProgramsViewsModels
{
    /// <summary>
    /// Programlama Menusü ViewModel
    /// </summary>
    public class ProgramsMenuViewModel : BaseViewModel
    {
        /// <summary>
        /// Programlama ekranındaki butonlardan gelen commandı yakalar.
        /// </summary>
        public ICommand PFunctionCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    switch (obj.ToString())
                    {
                        case "taxIdProgram":
                            AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.KDV_Programlama);
                            break;
                        case "depProgram":
                            AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Departman_Programlama);
                            break;
                        case "receiptHeadProgram":
                            AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Fis_Basligi_Mesaji_Programlama);
                            break;
                        case "receiptEndProgram":
                            AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Fis_Sonu_Mesaji_Programlama);
                            break;
                        case "paymentTypeProgram":
                            AppService.MenuContentService.SetProgramsScreen(VPrograms.EnumView.Odeme_Programlama_Menusu);
                            break;
                        default:
                            break;
                    }

                });
            }
        }

        /// <summary>
        /// Yan ekran değiştirme işlemi için kullanılır.
        /// </summary>
        int _CurrentContentScreenIndex;
        public int CurrentContentScreenIndex
        {
            get { return _CurrentContentScreenIndex; }
            set { _CurrentContentScreenIndex = value; OnPropertyChange("CurrentContentScreenIndex"); }
        }
        public ProgramsMenuViewModel()
        {
            AppService.MenuContentService.ScreenChanged += MenuContentService_ScreenChanged;

        }

        void MenuContentService_ScreenChanged(object sender, IMenuContainer e)
        {
            CurrentContentScreenIndex = e.ViewIndex;
        }

    }
}