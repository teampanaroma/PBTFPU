using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace PBTFrontOfficeProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            App.Current.Exit += Current_Exit;

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
typeof(FrameworkElement),
new FrameworkPropertyMetadata(
XmlLanguage.GetLanguage(
System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)));

            var fpucheckMessages = AppService.FpuService.CheckOpenOperations();
            if (fpucheckMessages.Count > 0)
            {
                var msg = string.Join(Environment.NewLine, fpucheckMessages);
                UIMessage.ShowErrorMessage(msg);
            }
            base.OnStartup(e);
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (FPUFuctions.IsInstanceCreated)
            {
                FPUFuctions.Instance.Dispose();
            }
            AppService.ShutDown();

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show(GetExceptionMessage(ex), "Beklenmeyen Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(GetExceptionMessage(e.Exception), "Beklenmeyen Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string GetExceptionMessage(Exception ex)
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
