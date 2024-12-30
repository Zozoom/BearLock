# BearLock - Local Password Manager

**BearLock** is a secure, offline-first password manager designed to store and manage your passwords locally on your device. With BearLock, your data remains private, encrypted, and accessible only to you—without reliance on cloud storage or third-party servers.

<Image Source="preview.png" />

---

## Features

### 🛡️ **Local Security**

- Fully local storage of passwords in an encrypted JSON file.
- Password encryption using Windows Data Protection API (DPAPI).
- Master password protection to secure the app (optional).

### 🔑 **Password Management**

- Generate strong passwords with customizable options:
  - Length
  - Inclusion of uppercase letters, special characters, and numbers.
- Save and organize passwords by application or website.
- View, copy, or delete stored passwords as needed.

### 🎨 **Intuitive UI**

- Modern, user-friendly interface with clear organization.
- Separate panels for password creation and stored passwords.
- Localization support for multiple languages (English and Hungarian).

### ⚙️ **Customizable Options**

- Set your preferred storage file path for complete control over data location.
- Configure application settings like "Start with Windows" or "Minimize to Tray."
- Offline-first design ensures no internet dependency.

### 📝 **Additional Features**

- Password masking to protect sensitive data from prying eyes.
- Auto-clear clipboard for added security after copying a password.
- Password history tracking for reverting to previous versions.

---

## Installation

1. **Download the Application**:
   Clone the repository or download the latest release from the [Releases](#) page.

2. **Run the Application**:

   - Double-click the `BearLock.exe` file to launch the app.
   - The application requires Windows 7 or later.

3. **Dependencies**:
   - No external dependencies—BearLock operates entirely offline.

---

## Usage

1. **Add a Password**:

   - Enter the application name and (optionally) a website URL.
   - Use the password generator to create a secure password, or enter one manually.
   - Save the password to store it locally in the encrypted database.

2. **View or Manage Passwords**:

   - Navigate to the **Stored Passwords** panel.
   - Select a password entry to reveal, copy, or delete it.

3. **Application Settings**:
   - Access the settings menu to customize behavior, including:
     - File path for password storage.
     - Startup and minimization options.

---

## Security Features

- **Encryption**: All passwords are encrypted using a robust Windows Data Protection API (DPAPI) to ensure your data remains secure.
- **No Cloud Dependency**: Your passwords are stored locally, and no data is ever transmitted to external servers.
- **Password Masking**: Masked passwords are displayed in the UI until explicitly revealed.

---

## Localization

BearLock supports the following languages:

- **English** (`en-US`)
- **Hungarian** (`hu-HU`)

You can change the language from the dropdown menu in the application header.

---

## Future Features

We aim to continually improve BearLock while keeping it fully local and secure. Planned features include:

- Master password protection for accessing the application.
- Password strength analysis.
- Custom tags and categories for better password organization.
- Secure offline backups for redundancy.

---

## Contributing

We welcome contributions! If you’d like to improve BearLock:

1. Fork this repository.
2. Create a feature branch (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-name`).
5. Open a Pull Request.

---

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## Acknowledgments

BearLock is built with love and care to provide a secure, private solution for managing passwords. Thank you for using our application and helping us improve!
