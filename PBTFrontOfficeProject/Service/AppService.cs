using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// Business logic alanını soyutlamak amacıyla hazırlanmıştır.
    /// </summary>
    public class AppService
    {
        /// <summary>
        /// Menüler için oluşturulmuştur.
        /// </summary>
        static MenuService _MenuService;
        public static MenuService MenuService
        {
            get { return (_MenuService ?? (_MenuService = new MenuService())); }
        }

        /// <summary>
        /// Alt menüler için oluşturulmuştur.
        /// </summary>
        static MenuContentService _MenuContentService;
        public static MenuContentService MenuContentService
        {
            get { return (_MenuContentService ?? (_MenuContentService = new MenuContentService())); }
        }

        /// <summary>
        /// FPU işlemleri için oluşturulmuştur.
        /// </summary>
        static FpuService _FpuService;
        public static FpuService FpuService
        {
            get { return (_FpuService ?? (_FpuService = new FpuService())); }
        }


        public static void ShutDown()
        {
            if (FPUFuctions.IsInstanceCreated)
            {
                FPUFuctions.Instance.Dispose();
            }

            if (CurrentWindow != null)
            {
                CurrentWindow.Close();
                CurrentWindow = null;
            }

            if (_ScreenKeyboard != null && IsVirtualKeyboardActive)
            {
                if (_ScreenKeyboard.IsActive)
                    _ScreenKeyboard.Close();
                _ScreenKeyboard = null;
            }

        }

        public static Window CurrentWindow { get; set; }
        static ScreenKeyboard _ScreenKeyboard;

        public static bool IsVirtualKeyboardActive { get; set; }

        public static void ScreenKeyboardToggle(Window owner = null)
        {
            owner = owner ?? CurrentWindow;
            if (_ScreenKeyboard == null)
            {
                _ScreenKeyboard = new ScreenKeyboard();
            }

            if (_ScreenKeyboard.IsHide)
            {
                _ScreenKeyboard.CurrentWindow = owner;
                _ScreenKeyboard.IsHide = false;
                IsVirtualKeyboardActive = true;
                _ScreenKeyboard.Show();
            }
            else
            {
                _ScreenKeyboard.CurrentWindow = owner;
                _ScreenKeyboard.Hide();
                IsVirtualKeyboardActive = false;
                _ScreenKeyboard.IsHide = true;
                //_VirtualKeyboard = new VirtualKeyboard();

            }
        }
    }
}
