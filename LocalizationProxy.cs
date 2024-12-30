using System.ComponentModel;
using System.Globalization;
using System.Resources;
using BearLock.Properties;

namespace BearLock
{
    public class LocalizationProxy : INotifyPropertyChanged
    {

        // Triggered when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Access resource strings by key
        public string this[string key] => Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);

        // Refresh method to notify bindings
        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
