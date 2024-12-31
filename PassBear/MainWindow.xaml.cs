using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using BearLock;

namespace PassBear
{
    public partial class MainWindow : Window
    {
        private const string DataFilePath = "passbear_passwords.json";
        public ObservableCollection<PasswordEntry> Passwords { get; set; } = new ObservableCollection<PasswordEntry>();

        public MainWindow()
        {
            InitializeComponent();
            LoadPasswordsFromFile();
            dgPasswordList.ItemsSource = Passwords;
        }

        private void ChangeLanguage(string culture)
        {
            LocalizationManager.ChangeCulture(culture);
        }

        private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string culture = selectedItem.Tag.ToString();
                LocalizationManager.ChangeCulture(culture);
            }
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            int length = int.TryParse(txtPasswordLength.Text, out var parsedLength) ? parsedLength : 12;
            bool useCapitals = chkCapitals.IsChecked ?? false;
            bool useSpecialChars = chkSpecialChars.IsChecked ?? false;
            bool useNumbers = chkNumbers.IsChecked ?? false;

            txtPassword.Text = GeneratePassword(length, useCapitals, useSpecialChars, useNumbers);
        }

        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAppName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Application Name and Password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var entry = new PasswordEntry
            {
                AppName = txtAppName.Text,
                SiteURL = txtSiteURL.Text,
                EncryptedPassword = EncryptPassword(txtPassword.Text),
                DisplayPassword = new string('*', txtPassword.Text.Length),
                CreationTime = DateTime.Now.ToString("g")
            };

            Passwords.Add(entry);
            SavePasswordsToFile(); // Save to JSON file
            ClearInputFields();
        }

        private string GeneratePassword(int length, bool useCapitals, bool useSpecialChars, bool useNumbers)
        {
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*";

            string charSet = lowerChars;
            if (useCapitals) charSet += upperChars;
            if (useSpecialChars) charSet += specialChars;
            if (useNumbers) charSet += numbers;

            var result = new StringBuilder();
            var rng = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(charSet[rng.Next(charSet.Length)]);
            }

            return result.ToString();
        }

        private string EncryptPassword(string plainText)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting password: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private string DecryptPassword(string encryptedText)
        {
            try
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedText);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error decrypting password: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private void RevealPassword_Click(object sender, RoutedEventArgs e)
        {
            if (dgPasswordList.SelectedItem is PasswordEntry selectedEntry)
            {
                selectedEntry.DisplayPassword = DecryptPassword(selectedEntry.EncryptedPassword);
                dgPasswordList.Items.Refresh();
            }
        }

        private void CopyPasswordToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (dgPasswordList.SelectedItem is PasswordEntry selectedEntry)
            {
                string decryptedPassword = DecryptPassword(selectedEntry.EncryptedPassword);
                Clipboard.SetText(decryptedPassword);
                MessageBox.Show("Password copied to clipboard!", "PassBear", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeletePassword_Click(object sender, RoutedEventArgs e)
        {
            if (dgPasswordList.SelectedItem is PasswordEntry selectedEntry)
            {
                Passwords.Remove(selectedEntry);
                SavePasswordsToFile(); // Save changes after deletion
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == textBox.Tag?.ToString())
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString();
            }
        }

        private void ClearInputFields()
        {
            txtAppName.Text = "Application Name";
            txtSiteURL.Text = "Site URL (Optional)";
            txtPassword.Text = "Password";
            txtPasswordLength.Text = "12";
            chkCapitals.IsChecked = true;
            chkSpecialChars.IsChecked = true;
            chkNumbers.IsChecked = true;
        }

        private void SavePasswordsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(Passwords);
                File.WriteAllText(DataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPasswordsFromFile()
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    var json = File.ReadAllText(DataFilePath);
                    var loadedPasswords = JsonSerializer.Deserialize<ObservableCollection<PasswordEntry>>(json);

                    if (loadedPasswords != null)
                    {
                        Passwords = loadedPasswords;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ConfigureFilePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "passbear_passwords.json",
                DefaultExt = ".json",
                Filter = "JSON files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                string newFilePath = dialog.FileName;
                // Save this path to a settings file or variable
                MessageBox.Show($"Password file path updated to:\n{newFilePath}", "Settings Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                // Update logic to use this path
            }
        }

        private void ToggleStartWithWindows_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                if (menuItem.IsChecked)
                {
                    // Logic to enable "Start with Windows"
                    MessageBox.Show("PassBear will now start with Windows.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Logic to disable "Start with Windows"
                    MessageBox.Show("PassBear will no longer start with Windows.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ToggleMinimizedToTray_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                if (menuItem.IsChecked)
                {
                    // Logic to minimize to tray
                    MessageBox.Show("PassBear will now minimize to tray.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Logic to disable minimize to tray
                    MessageBox.Show("PassBear will no longer minimize to tray.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Owner = this; // Set owner for modal behavior
            aboutWindow.ShowDialog();
        }
    }

    public class PasswordEntry
    {
        public string AppName { get; set; } = string.Empty; // Non-nullable, defaults to an empty string
        public string? SiteURL { get; set; } // Nullable to allow optional input
        public string EncryptedPassword { get; set; } = string.Empty; // Non-nullable, defaults to an empty string
        public string DisplayPassword { get; set; } = string.Empty; // Non-nullable, defaults to an empty string
        public string CreationTime { get; set; } = string.Empty; // Non-nullable, defaults to an empty string
    }

}
