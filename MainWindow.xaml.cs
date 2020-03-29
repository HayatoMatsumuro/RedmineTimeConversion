using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace ReamineTimeConversion
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        // Redmineから取得した作業時間データの出力用クラス
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

            // 設定ファイルからデータを取得
            TextBox_URL.Text = Setting.GetInstance().URL;
            TextBox_APIKey.Text = Setting.GetInstance().APIKEY;
            TextBox_ID.Text = Setting.GetInstance().PROJECTID;
        }

        // 作業時間を取得ボタン押し
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 設定ファイルに保存
            Setting.GetInstance().URL = TextBox_URL.Text;
            Setting.GetInstance().APIKEY = TextBox_APIKey.Text;
            Setting.GetInstance().PROJECTID = TextBox_ID.Text;
            Setting.SaveSetting();

            // redmine-net-apiで開始日から終了日までの作業時間データを取得する
            string date_start = ((DateTime)(Date_Start.SelectedDate)).ToString("yyyy-MM-dd");
            string date_finish = ((DateTime)(Date_Finish.SelectedDate)).ToString("yyyy-MM-dd");

            var manager = new RedmineManager(Setting.GetInstance().URL, Setting.GetInstance().APIKEY);
            var parameters = new NameValueCollection {
                                { RedmineKeys.LIMIT, "100" },
                                { RedmineKeys.PROJECT_ID, Setting.GetInstance().PROJECTID},
                                { RedmineKeys.SPENT_ON, "><" + date_start + "|" + date_finish } };

            try
            {
                // データ取得処理
                List<TimeEntry> timeEntryList = manager.GetObjects<TimeEntry>(parameters);

                // 出力用のクラスにRedmineから取得したデータを設定
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

                // 期間内の作業時間を加工したものを出力
                string str = "";

                foreach (OutputTimeEntry outputTimeEntry in outputTimeEntryList)
                {
                    str = str + "#" + outputTimeEntry.issue  + " " + outputTimeEntry.title + " " + outputTimeEntry.total + " h"+ Environment.NewLine;
                    foreach( TimeEntry timeEntry in outputTimeEntry.timeEntryList)
                    {
                        str = str + "・" + timeEntry.Comments + Environment.NewLine;
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
