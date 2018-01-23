using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// Menülere göre açılan alt menüleri çağırmak için kullanılır.
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// Seçili ekran için kullanılır.
        /// </summary>
        EnumScreen _CurrentScreen;
        public void SetCurrentScreen(EnumScreen screen)
        {
            _CurrentScreen = screen;
            OnScreenChanged(screen);
        }

        /// <summary>
        /// Ekranların anlık değişimini bildirmek için kullanılır.
        /// </summary>
        /// <param name="screen">Hangi ekranın geleceğini bildirir.</param>
        private void OnScreenChanged(EnumScreen screen)
        {
            if (ScreenChanged != null)
                ScreenChanged(this, screen);
        }

        public event EventHandler<EnumScreen> ScreenChanged;
    }
}
