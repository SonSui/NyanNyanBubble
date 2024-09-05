using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WordInfo
{
    public string Japanese { get; set; } // 日本語の単語
    public string English { get; set; }  // 英語の意味
}

public class JMDictParser : MonoBehaviour
{
    public static JMDictParser Instance { get; private set; } // シングルトンインスタンス
    private Dictionary<string, WordInfo> wordDictionary; // 単語の辞書

    void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // シーンが切り替わってもオブジェクトを破棄しない
            InitializeDictionary(); // 辞書を初期化
        }
        else
        {
            Destroy(gameObject);  // 複数のインスタンスが存在する場合、現在のオブジェクトを破棄
        }
    }

    private void InitializeDictionary()
    {
        wordDictionary = new Dictionary<string, WordInfo>();
        string filePath = Application.streamingAssetsPath + "/JMdict_e.xml"; // 辞書のXMLファイルのパス

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;
        settings.MaxCharactersFromEntities = long.MaxValue; // エンティティからの最大文字数の制限を設定

        using (XmlReader reader = XmlReader.Create(filePath, settings))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            XmlNodeList entryList = doc.GetElementsByTagName("entry");

            foreach (XmlNode entry in entryList)
            {
                string kanji = "";
                string kana = "";
                string english = "";
                XmlNodeList kanjiElements = entry.SelectNodes("k_ele/keb");
                XmlNodeList readingElements = entry.SelectNodes("r_ele/reb");
                XmlNodeList senseElements = entry.SelectNodes("sense/gloss");

                if (kanjiElements.Count > 0)
                {
                    kanji = kanjiElements[0].InnerText; // 漢字のエントリを取得
                }

                if (readingElements.Count > 0)
                {
                    kana = readingElements[0].InnerText; // 仮名のエントリを取得
                }

                foreach (XmlNode sense in senseElements)
                {
                    english += sense.InnerText + "; "; // 英語の意味を結合
                }

                if (!string.IsNullOrEmpty(english))
                {
                    english = english.TrimEnd(' ', ';'); // 最後のセミコロンと空白を削除
                }

                if (!string.IsNullOrEmpty(kana) && !wordDictionary.ContainsKey(kana))
                {
                    wordDictionary[kana] = new WordInfo { Japanese = kana, English = english }; // 辞書に仮名エントリを追加
                }

                if (!string.IsNullOrEmpty(kanji) && !wordDictionary.ContainsKey(kanji))
                {
                    wordDictionary[kanji] = new WordInfo { Japanese = kanji, English = english }; // 辞書に漢字エントリを追加
                }
            }
        }
    }

    public bool IsWordValid(string word, out string english)
    {
        if (wordDictionary.TryGetValue(word, out WordInfo wordInfo))
        {
            english = wordInfo.English; // 英語の意味を返す
            return true; // 単語が辞書に存在する
        }
        else
        {
            english = null; // 単語が見つからない場合はnullを返す
            return false; // 単語が辞書に存在しない
        }
    }

    void Start()
    {
        string word = "あいそう"; // 検索する単語
        string meaning;
        if (IsWordValid(word, out meaning))
        {
            Debug.Log($"{word} means {meaning}"); // 単語の意味を表示
        }
        else
        {
            Debug.Log("Word Not Found"); // 単語が見つからない場合のメッセージ
        }
    }
}
