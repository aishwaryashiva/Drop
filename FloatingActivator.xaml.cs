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
using System.IO;

namespace Drop
{
    /// <summary>
    /// Interaction logic for FloatingActivator.xaml
    /// </summary>
    public partial class FloatingActivator : Window
    {
        MainWindow mainWindow;
        public FloatingActivator()
        {
            InitializeComponent();
            mainWindow = new MainWindow();
        }

        private void Image_MouseMove_1(object sender, MouseEventArgs e)
        {
            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Window_Drop_1(object sender, DragEventArgs e)
        {
            List<System.IO.FileInfo> droppedFiles = DroppedData.GetDroppedFiles(e);
            List<string> droppedTexts = DroppedData.GetDroppedTexts(e);
            if (droppedFiles != null)
            {
                foreach (System.IO.FileInfo fi in droppedFiles)
                {
                    FileAttributes attr = File.GetAttributes(fi.FullName);
                    bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                    Utility.AddData(new UserData() { DisplayName = Utility.GetShortenedName(fi.Name), Data = fi.FullName, DataType = isFolder ? "Folder" : "File" });
                }
            }
            if (droppedTexts != null)
            {
                foreach (string text in droppedTexts)
                {
                    Utility.AddData(new UserData() { DisplayName = Utility.GetShortenedName(text), Data = text, DataType = "Text" });
                }
            }
            //mainWindow.InitItems();
            MainWindow.InitItems();
        }

        private void Window_DragEnter_1(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
        sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Image_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                mainWindow.Show();
        }
    }
}
