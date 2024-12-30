using System.Globalization;
using System.Windows;

namespace PassBear
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Set the culture (e.g., "fr-FR" for French)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("hu-HU");
            base.OnStartup(e);
        }
    }
}
