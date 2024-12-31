using System.Globalization;
using System.Threading;
using PassBear;

namespace BearLock
{
    public static class LocalizationManager
    {
        public static void ChangeCulture(string culture)
        {
            var newCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;

            // Refresh the LocalizationProxy to notify bindings
            if (App.Current.Resources["Localization"] is LocalizationProxy proxy)
            {
                proxy.Refresh();
            }
        }
    }
}
