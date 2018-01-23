using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PBTFrontOfficeProject.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Ekran değiştirme işlemleri için kullanılır.
        /// </summary>
        int _CurrentScreenIndex;
        public int CurrentScreenIndex
        {
            get { return _CurrentScreenIndex; }
            set { _CurrentScreenIndex = value; OnPropertyChange("CurrentScreenIndex"); }
        }

        /// <summary>
        /// Alt ekran değiştirme işlemi için kullanılır.
        /// </summary>
        int _CurrentContentScreenIndex;
        public int CurrentContentScreenIndex
        {
            get { return _CurrentContentScreenIndex; }
            set { _CurrentContentScreenIndex = value; OnPropertyChange("CurrentContentScreenIndex"); }
        }

        public MainViewModel()
        {
            AppService.MenuService.ScreenChanged += MenuService_ScreenChanged;
            AppService.MenuContentService.ScreenChanged += MenuContentService_ScreenChanged;
        }


       



        private void MenuContentService_ScreenChanged(object sender, IMenuContainer e)
        {
            CurrentContentScreenIndex = e.ViewIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuContentService_ScreenChanged(object sender, EnumContentScreen e)
        {
            CurrentContentScreenIndex = (int)e;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuService_ScreenChanged(object sender, EnumScreen e)
        {
            CurrentScreenIndex = (int)e;
        }
    }
}
