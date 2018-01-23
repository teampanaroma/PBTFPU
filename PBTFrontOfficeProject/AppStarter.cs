using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace PBTFrontOfficeProject
{
    public class AppStarter
    {
        MainWindow m = null;

        static AppStarter()
        {
            appStartup();
        }

        public void StartMainWindow()
        {
            if (!FPUFuctions.IsInstanceCreated)
            {
                initFpu();
                m = new MainWindow();
            }

            if (m == null)
            {
                m = new MainWindow();
            }
            m.Show();
        }

        public void CloseMainWindow()
        {
            AppService.ShutDown();
        }

        private static void appStartup()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            try
            {
                FrameworkElement.LanguageProperty.OverrideMetadata(
    typeof(FrameworkElement),
    new FrameworkPropertyMetadata(
    XmlLanguage.GetLanguage(
    System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch (Exception ex)
            {

            }

            initFpu();
        }

        private static void initFpu()
        {
            var fpucheckMessages = AppService.FpuService.CheckOpenOperations();
            if (fpucheckMessages.Count > 0)
            {
                var msg = string.Join(Environment.NewLine, fpucheckMessages);
                UIMessage.ShowErrorMessage(msg);
            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (FPUFuctions.IsInstanceCreated)
            {
                FPUFuctions.Instance.Dispose();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show(GetExceptionMessage(ex), "Beklenmeyen Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static string GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            if (ex.InnerException != null)
                sb.AppendLine(GetExceptionMessage(ex.InnerException));
            sb.AppendLine("StackTrace: " + ex.StackTrace);

            return sb.ToString();
        }
    }
}