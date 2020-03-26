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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_URL.Text;
            string id = TextBox_ID.Text;

            string date_start = ((DateTime)(Date_Start.SelectedDate)).ToString("yyyy-MM-dd");
            string date_finish = ((DateTime)(Date_Finish.SelectedDate)).ToString("yyyy-MM-dd");

        }
    }
}
