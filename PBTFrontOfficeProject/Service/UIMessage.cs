using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PBTFrontOfficeProject.Service
{
    public class UIMessage
    {
        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal static void ShowInfoMessage(string infoMessage)
        {
            MessageBox.Show(infoMessage, "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
