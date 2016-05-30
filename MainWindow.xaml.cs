using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.IO;

namespace Drop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsClipboardOn = false;
        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.IsLite)
            {
                this.Title = "Drop Lite";
                lblTitle.Text = "Drop Lite";
            }
            else
            {
                this.Title = "Drop";
                lblTitle.Text = "Drop";
            }
            this.DataContext = this;
            if (IsClipboardOn)
            {
                btnClipBoard.Visibility = Visibility.Hidden;
                ClipboardMonitor.Start();
                ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            }
        }
        static void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            switch (format)
            {
                case ClipboardFormat.Text:
                    Utility.AddData(new UserData() { DisplayName = Utility.GetShortenedName(Utility.GetURLTitle(data.ToString())), Data = data, DataType = "Text" });
                    break;
                case ClipboardFormat.FileDrop:
                    string[] files = (string[])data;
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileAttributes attr = File.GetAttributes(files[i]);
                        bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                        Utility.AddData(new UserData() { DisplayName = Utility.GetShortenedName(new FileInfo(files[i]).Name), Data = files[i], DataType = isFolder ? "Folder" : "File" });
                    }
                    break;
                default:break;
            }
            InitItems();
        }
        public void RunWorker()
        {
            InitItems();
            //worker.RunWorkerAsync();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private Point start;
        private void files_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.start = e.GetPosition(null);
        }
        private static object GetDataFromListBox(ListView source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }
        private static ObservableCollection<UserData> _FileItems = new ObservableCollection<UserData>();
        public static ObservableCollection<UserData> FileItems
        {
            get
            {
                return _FileItems;
            }
        }
        private static ObservableCollection<UserData> _FolderItems = new ObservableCollection<UserData>();
        public static ObservableCollection<UserData> FolderItems
        {
            get
            {
                return _FolderItems;
            }
        }
        private static ObservableCollection<UserData> _TextItems = new ObservableCollection<UserData>();
        public static ObservableCollection<UserData> TextItems
        {
            get
            {
                return _TextItems;
            }
        }
        public static int InitItems()
        {
            FileOps.UserDataCollection userData = Utility.userData;
            if (userData != null && userData.userData != null && userData.userData.Count > 0)
            {
                foreach (UserData ud in userData.userData)
                {
                    switch (ud.DataType)
                    {
                        case "file":
                            if (_FileItems.Where(c => c.Data == ud.Data).ToArray().Length == 0)
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    _FileItems.Add(ud);
                                });
                            break;
                        case "folder":
                            if (_FolderItems.Where(c => c.Data == ud.Data).ToArray().Length == 0)
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    _FolderItems.Add(ud);
                                });
                            break;
                        case "text":
                            if (_TextItems.Where(c => c.Data == ud.Data).ToArray().Length == 0)
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    _TextItems.Add(ud);
                                });
                            break;
                        default: break;
                    }
                }
                
                //worker.ReportProgress(90,
                    //Dispatcher.BeginInvoke(
                
                    //)
                    //);
            }
            return 0;
        }
        private void files_MouseMove(object sender, MouseEventArgs e)
        {

            Point mpos = e.GetPosition(null);
            Vector diff = this.start - mpos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                ListView parent = (ListView)sender;
                DataObject obj = GetDataObject(parent);
                if (obj != null)
                {
                    Hide();
                    DragDrop.DoDragDrop(parent, obj, DragDropEffects.Copy);
                }
            }
        }
        DataObject GetDataObject(ListView targetListView)
        {
            if (targetListView.SelectedItems.Count == 0)
            {
                return null;
            }
            object data = targetListView.SelectedItems;

            System.Collections.IList items = (System.Collections.IList)data;
            var collection = items.Cast<UserData>();
            if (data != null)
            {
                DataObject obj = new DataObject();
                switch (targetListView.Name.ToLower())
                {
                    case "files":
                    case "folders":
                        List<string> filePaths = new List<string>();
                        foreach (UserData ud in collection)
                        {
                            filePaths.Add(new System.IO.FileInfo(ud.Data.ToString()).FullName);
                        }
                        System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
                        sc.AddRange(filePaths.ToArray());
                        obj.SetFileDropList(sc);
                        break;
                    case "texts":
                        string textData = "";
                        foreach (UserData ud in collection)
                        {
                            textData = textData + ud.Data;
                        }
                        obj.SetText(textData);
                        break;
                    default: return null;
                }
                return obj;
            }
            return null;
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("This will turn off " + (Properties.Settings.Default.IsLite ? "Drop Lite" : "Drop") + " and you will lose all your data.\n\nFiles and Folders will not be deleted from their original locations.\n\n Are you sure?", "Exiting", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                ClipboardMonitor.Stop();
                Environment.Exit(0);
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
           
        }

        private void TabItem_DragLeave_1(object sender, DragEventArgs e)
        {
            
        }

        private void files_DragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void files_MouseLeave(object sender, MouseEventArgs e)
        {
            //Hide();
            //Point mpos = e.GetPosition(null);
            //Vector diff = this.start - mpos;

            //if (e.LeftButton == MouseButtonState.Pressed)
            ////&&
            ////    Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
            ////    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            //{
            //    if (this.files.SelectedItems.Count == 0)
            //        return;

            //    // right about here you get the file urls of the selected items.  
            //    // should be quite easy, if not, ask.  
            //    string[] Files = new String[files.SelectedItems.Count];
            //    int ix = 0;
            //    foreach (object nextSel in files.SelectedItems)
            //    {
            //        Files[ix] = "C:\\Users\\MyName\\Music\\My playlist\\" + nextSel.ToString();
            //        ++ix;
            //    }
            //    string dataFormat = DataFormats.FileDrop;
            //    DataObject dataObject = new DataObject(dataFormat, files);
            //    DragDrop.DoDragDrop(this.files, dataObject, DragDropEffects.Copy);
            //} 
        }

        private void Window_DragLeave_1(object sender, DragEventArgs e)
        {

        }
        private void btnClipBoard_Click(object sender, RoutedEventArgs e)
        {
            EnableClipboardMonitoring();
        }
        private void CtrlCCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsClipboardOn)
            {
                ListView currentList = (ListView)sender;
                DataObject obj = GetDataObject(currentList);
                if (obj != null) Clipboard.SetDataObject(obj);
            }
            else
                ShowClipboardError();
        }

        private void CtrlCCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DelCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsClipboardOn)
            {
                ListView currentList = (ListView)sender;
                if (currentList.SelectedItems.Count > 0)
                {
                    object data = currentList.SelectedItems;
                    System.Collections.IList items = (System.Collections.IList)data;
                    var collection = items.Cast<UserData>().ToArray();
                    for (int i = 0; i < collection.Length; i++)
                    {
                        Utility.userData.userData.Remove(collection[i]);
                        InitItems();
                        switch (collection[i].DataType)
                        {
                            case "file":
                                _FileItems.Remove(collection[i]);
                                break;
                            case "folder":
                                _FolderItems.Remove(collection[i]);
                                break;
                            case "text":
                                _TextItems.Remove(collection[i]);
                                break;
                            default: break;
                        }
                    }
                }
            }
            else
                ShowClipboardError();
        }

        private void DelDeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RightClickCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsClipboardOn)
            {
                ListView currentList = (ListView)(((ContextMenu)this.Resources["SharedContextMenu"]).PlacementTarget);
                DataObject obj = GetDataObject(currentList);
                if (obj != null) Clipboard.SetDataObject(obj);
            }
            else
                ShowClipboardError();
        }

        private void RightClickCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RightClickDeleteCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsClipboardOn)
            {
                ListView currentList = (ListView)(((ContextMenu)this.Resources["SharedContextMenu"]).PlacementTarget);
                if (currentList.SelectedItems.Count > 0)
                {
                    object data = currentList.SelectedItems;
                    System.Collections.IList items = (System.Collections.IList)data;
                    var collection = items.Cast<UserData>().ToArray();
                    for (int i = 0; i < collection.Length; i++)
                    {
                        Utility.userData.userData.Remove(collection[i]);
                        InitItems();
                        switch (collection[i].DataType)
                        {
                            case "file":
                                _FileItems.Remove(collection[i]);
                                break;
                            case "folder":
                                _FolderItems.Remove(collection[i]);
                                break;
                            case "text":
                                _TextItems.Remove(collection[i]);
                                break;
                            default: break;
                        }
                    }
                }
            }
            else
                ShowClipboardError();
        }
        public void ShowClipboardError()
        {
            MessageBoxResult mbr = MessageBox.Show("Clipboard Monitoring not enabled. Do you want to enable it?", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (mbr == MessageBoxResult.Yes)
                EnableClipboardMonitoring();
        }
        void EnableClipboardMonitoring()
        {
            System.Diagnostics.Process.Start("https://sellfy.com/p/9ch7/");
        }
        private void RightClickDeleteCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
