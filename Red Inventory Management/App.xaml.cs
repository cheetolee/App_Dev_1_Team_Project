using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal("An unhandled exception occurred.", e.Exception);
            MessageBox.Show("An unhandled exception occurred.\nCheck InventoryManagementLog.txt for more information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = false;
        }
    }
}
