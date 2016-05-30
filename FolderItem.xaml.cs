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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Drop
{
    /// <summary>
    /// Interaction logic for FolderItem.xaml
    /// </summary>
    public partial class FolderItem : UserControl
    {
        public FolderItem()
        {
            InitializeComponent();
        }
        public static DependencyProperty FullPathProperty = DependencyProperty.Register("FullFolderPath", typeof(string), typeof(FileItem));
        public string FullPath
        {
            get { return (string)base.GetValue(FullPathProperty); }
            set { base.SetValue(FullPathProperty, value); }
        }
        public static DependencyProperty DisplayNameProperty = DependencyProperty.Register("FolderDisplayName", typeof(string), typeof(FileItem));
        public string DisplayName
        {
            get { return (string)base.GetValue(DisplayNameProperty); }
            set
            {
                base.SetValue(DisplayNameProperty, value);
            }
        }

        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(this.ToolTip.ToString());
        }
    }
}
