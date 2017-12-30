using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;

namespace Duracellko.ClipboardShareUI
{
    /// <summary>
    /// Installer class for Clipboard Share installation
    /// </summary>
    [RunInstaller(true)]
    public partial class ClipboardShareInstaller : Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardShareInstaller"/> class.
        /// </summary>
        public ClipboardShareInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When overridden in a derived class, removes an installation.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Collections.IDictionary"/> that contains the state of the computer after the installation was complete.</param>
        /// <exception cref="T:System.ArgumentException">
        /// The saved-state <see cref="T:System.Collections.IDictionary"/> might have been corrupted.
        /// </exception>
        /// <exception cref="T:System.Configuration.Install.InstallException">
        /// An exception occurred while uninstalling. This exception is ignored and the uninstall continues. However, the application might not be fully uninstalled after the uninstallation completes.
        /// </exception>
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            try
            {
                // remove "Run on startup" setting
                RegistryConfiguration.RunOnStartup = false;

                // remove configuration folder
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(path, "ClipboardShare");
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                throw new InstallException(Resources.Error_Uninstall, ex);
            }
        }

    }
}
