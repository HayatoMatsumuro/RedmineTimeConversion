using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ReamineTimeConversion
{
    public class Setting
    {

        // ========================================================================
        // property
        // ========================================================================
        #region +

        /// <summary>URL</summary>
        public string URL { get; set; }

        /// <summary>APIKey</summary>
        public string APIKEY { get; set; }

        /// <summary>プロジェクトID</summary>
        public string PROJECTID { get; set; }

        #endregion

        #region +
        /// <summary>設定ファイルパス</summary>
        private static string settingFilePath = string.Empty;

        /// <summary>静的インスタンス</summary>
        private static Setting setting;

        #endregion

        /// <summary>静的インスタンス取得</summary>
        public static Setting GetInstance()
        {
            return setting;
        }

        /// <summary>静的コンストラクタ</summary>
        static Setting()
        {
            settingFilePath = GetExePath() + "\\Setting.xml";

            if (File.Exists(settingFilePath))
            {
                LoadSetting();
            }
            else
            {
                setting = new Setting();

                SaveSetting();
            }
        }

        /// <summary>設定を開く</summary>
        public static void LoadSetting()
        {
            //XmlSerializerオブジェクトを作成
            XmlSerializer serializer = new XmlSerializer(typeof(Setting));

            //読み込むファイルを開く
            using (StreamReader sr = new StreamReader(settingFilePath, new UTF8Encoding(false)))
            {
                //XMLファイルから読み込み、逆シリアル化する
                setting = (Setting)serializer.Deserialize(sr);
            }
        }

        /// <summary>設定を保存する</summary>
        public static void SaveSetting()
        {
            //XmlSerializerオブジェクトを作成
            XmlSerializer serializer = new XmlSerializer(typeof(Setting));

            //書き込むファイルを開く（UTF-8 BOM無し）
            using (StreamWriter sw = new StreamWriter(settingFilePath, false, new UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, setting);
            }
        }

        /// <summary>EXEパスの取得</summary>
        /// <returns>EXEパス</returns>
        public static string GetExePath()
        {
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeFullPath = System.IO.Path.GetFullPath(exePath);
            string startupPath = System.IO.Path.GetDirectoryName(exeFullPath);
            return startupPath;
        }

    }
}