using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private class OutputTimeEntry
        {
            public int issue;

            public string title;

            public double total;

            public List<TimeEntry> timeEntryList = new List<TimeEntry>();
        }

        public MainWindow()
        {
            InitializeComponent();

            TextBox_URL.Text = Setting.GetInstance().URL;
            TextBox_APIKey.Text = Setting.GetInstance().APIKEY;
            TextBox_ID.Text = Setting.GetInstance().PROJECTID;
        }

        // 作業時間を取得
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_URL.Text;
            string apikey = TextBox_APIKey.Text;
            string id = TextBox_ID.Text;

            // 設定ファイルに保存
            Setting.GetInstance().URL = url;
            Setting.GetInstance().APIKEY = apikey;
            Setting.GetInstance().PROJECTID = id;
            Setting.SaveSetting();



            string date_start = ((DateTime)(Date_Start.SelectedDate)).ToString("yyyy-MM-dd");
            string date_finish = ((DateTime)(Date_Finish.SelectedDate)).ToString("yyyy-MM-dd");

            string acsess = url + "/time_entries.json?key=" + apikey + "&project_id=" + id + "&from=" + date_start + "&to=" + date_finish + "&limit=100";

            var manager = new RedmineManager(url, apikey);

            var parameters = new NameValueCollection { { RedmineKeys.LIMIT, "100" }, { RedmineKeys.SPENT_ON, "><" + date_start + "|" + date_finish } };

            try
            {
                List<TimeEntry> timeEntryList = manager.GetObjects<TimeEntry>(parameters);

                List<OutputTimeEntry> outputTimeEntryList = new List<OutputTimeEntry>();

                foreach ( TimeEntry timeEntry in timeEntryList)
                {
                    bool flg = false;
                    foreach( OutputTimeEntry outputTimeEntry in outputTimeEntryList)
                    {
                        if( outputTimeEntry.issue == timeEntry.Issue.Id)
                        {
                            outputTimeEntry.total += (double)timeEntry.Hours;
                            outputTimeEntry.timeEntryList.Add(timeEntry);
                            flg = true;
                            break;
                        }
                    }

                    if (!flg)
                    {
                        // 一致するチケットがない
                        var issue = manager.GetObject<Issue>(timeEntry.Issue.Id.ToString(), null);

                        OutputTimeEntry newOutputTimeEntry = new OutputTimeEntry();
                        newOutputTimeEntry.issue = timeEntry.Issue.Id;
                        newOutputTimeEntry.title = issue.Subject;
                        newOutputTimeEntry.total = (double)timeEntry.Hours;
                        newOutputTimeEntry.timeEntryList.Add(timeEntry);
                        outputTimeEntryList.Add(newOutputTimeEntry);
                    }
                }

                string str = "";

                foreach (OutputTimeEntry outputTimeEntry in outputTimeEntryList)
                {
                    str = str + "#" + outputTimeEntry.issue  + " " + outputTimeEntry.title + " " + outputTimeEntry.total + " h"+ Environment.NewLine;
                    foreach( TimeEntry timeEntry in outputTimeEntry.timeEntryList)
                    {
                        str = str + " " + timeEntry.Comments + Environment.NewLine;
                    }
                    str = str + Environment.NewLine;
                }
                TextBox_TimeEntryConversion.Text = str;
            }
            catch( Exception )
            {
                TextBox_TimeEntryConversion.Text = "取得に失敗しました。";
            }

        }
    }
}
