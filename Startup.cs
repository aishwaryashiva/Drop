using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Drop
{
    class Startup
    {
        // The path to the key where Windows looks for startup applications
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public bool IsStartUpSet()
        {
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("Drop") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                return false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                return true;
            }
        }
        public void SetStartUp()
        {
            // Add the value in the registry so that the application runs at startup
            rkApp.SetValue("Drop", Application.ExecutablePath.ToString());
        }
        public void RemoveStartup()
        {
            // Remove the value from the registry so that the application doesn't start
            rkApp.DeleteValue("Drop", false);
        }
    }
}
