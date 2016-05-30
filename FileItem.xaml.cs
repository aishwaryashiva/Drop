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
    /// Interaction logic for FileItem.xaml
    /// </summary>
    public partial class FileItem : UserControl
    {
        public static DependencyProperty FullFolderPathProperty = DependencyProperty.Register("FullPath", typeof(string), typeof(FileItem));
        public string FullPath
        {
            get { return (string)base.GetValue(FullFolderPathProperty); }
            set { base.SetValue(FullFolderPathProperty, value); }
        }
        public static DependencyProperty FolderDisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(FileItem));
        public string DisplayName
        {
            get { return (string)base.GetValue(FolderDisplayNameProperty); }
            set
            {
                base.SetValue(FolderDisplayNameProperty, value);
            }
        }
        public FileItem()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(this.ToolTip.ToString());
        }
    }
}
