using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Drop
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        Startup startup = new Startup();
        public About()
        {
            InitializeComponent();
            chkStartup.IsChecked = startup.IsStartUpSet();
            if (Properties.Settings.Default.IsLite)
                lblTitle.Content = "Drop Lite";
            else
                lblTitle.Content = "Drop";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://drop.codeplex.com/");
        }

        private void chkStartup_Checked(object sender, RoutedEventArgs e)
        {
            startup.SetStartUp();
        }

        private void chkStartup_Unchecked(object sender, RoutedEventArgs e)
        {
            startup.RemoveStartup();
        }

        private void btnHelp_Copy_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\LICENSE.txt");
        }
    }
}
