using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ReamineTimeConversion
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 作業時間を取得
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_URL.Text;
            string apikey = TextBox_APIKey.Text;
            string id = TextBox_ID.Text;

            string date_start = ((DateTime)(Date_Start.SelectedDate)).ToString("yyyy-MM-dd");
            string date_finish = ((DateTime)(Date_Finish.SelectedDate)).ToString("yyyy-MM-dd");

            string acsess = url + "/time_entries.json?key=" + apikey + "&project_id=" + id + "&from=" + date_start + "&to=" + date_finish + "&limit=100";

            HttpClient client = new HttpClient();

            try
            {
                var stream = await client.GetStreamAsync(acsess);
                var reader = new System.IO.StreamReader(stream);
                String str = reader.ReadToEnd();
            }
            catch (Exception )
            {

            }
        }
    }
}
