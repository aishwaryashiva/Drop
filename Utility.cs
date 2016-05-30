using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Drop
{
    public class Utility
    {
        public static FileOps.UserDataCollection userData;
        public static void AddData(UserData userdata)
        {
            try
            {
                if (userData == null)
                    userData = new FileOps.UserDataCollection();
                UserData[] ud=userData.userData.Where(c => c.Data.ToString().Trim().Equals(userdata.Data.ToString().Trim()) && c.DataType.Trim().Equals(userdata.DataType.Trim())).ToArray();
                if (ud.Length <= 0 && userdata.Data.ToString().Trim().Length > 0)
                    userData.userData.Add(userdata);
            }
            catch (Exception) { }
        }
        public static string GetShortenedName(string name)
        {
            int minLength = 50;
            string newName = name;
            if (newName.Length > minLength)
                return newName.Substring(0, minLength) + "...";
            else
                return newName;
        }
        public static string GetURLTitle(string url)
        {
            string title;
            try
            {
                System.Net.WebClient x = new System.Net.WebClient();
                string source = x.DownloadString(url);
                title = System.Text.RegularExpressions.Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups["Title"].Value;
            }
            catch (Exception) { return url; }
            return title == null ? url : title;
        }
    }
    public class FileOps
    {
        //    string enKey="@_124A.3485*TYC";
        //    string filename = System.AppDomain.CurrentDomain.BaseDirectory + "\\cpdta.dat";
        public class UserDataCollection
        {
            public List<UserData> userData = new List<UserData>();
        }
        //    public static UserDataCollection AllData
        //    {
        //        get { return new FileOps().GetData(); }
        //        set { new FileOps().SaveData(value); }
        //    }
        //    public UserDataCollection GetData()
        //    {
        //        if (System.IO.File.Exists(filename))
        //            return (UserDataCollection)(SaveXML.FileOperations.Load(filename, enKey)).GetObject();
        //        return new UserDataCollection();
        //    }
        //    public void SaveData(UserDataCollection data)
        //    {
        //        SaveXML.FileOperations.XMLFile file=new SaveXML.FileOperations.XMLFile(filename);
        //        file.SetObject(data);
        //        SaveXML.FileOperations.RESPONSE_CODES i = SaveXML.FileOperations.Save(file, true, enKey);
        //        if (i == SaveXML.FileOperations.RESPONSE_CODES.SAVE_FAILED)
        //            MessageBox.Show("Some error occured. Please restart application. Reinstall if problem persists.", "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
    }
    class DroppedData
    {
        public static List<string> GetDroppedTexts(DragEventArgs e)
        {
            List<string> droppedTexts = new List<string>();
            if (e.Data.GetDataPresent(DataFormats.Text, true))
            {
                droppedTexts.Add(e.Data.GetData(DataFormats.Text).ToString());
            }
            else
                return null;
            return droppedTexts;
        }
        public static List<FileInfo> GetDroppedFiles(DragEventArgs e)
        {
            List<FileInfo> droppedFiles = new List<FileInfo>();
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    FileInfo fi = new FileInfo(path);
                    droppedFiles.Add(fi);
                }
            }
            else
                return null;
            return droppedFiles;
        }
    }
}
