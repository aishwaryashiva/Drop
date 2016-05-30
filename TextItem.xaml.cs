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
    /// Interaction logic for TextItem.xaml
    /// </summary>
    public partial class TextItem : UserControl
    {
        public TextItem()
        {
            InitializeComponent();
            
        }
        async public Task<string> SetLinkTitle(string text)
        {
            Task<string> title = Task.Run(() => Utility.GetURLTitle(text));
            return await title;
        }

        private async void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            string s = await SetLinkTitle(TextFull.Text);
            TextTitle.Text = Utility.GetShortenedName(s.Trim().Length > 0 ? s : TextFull.Text);
        }
    }
}
