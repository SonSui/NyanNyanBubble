using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class VocabularyManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static VocabularyManager Instance { get; private set; }

    // CSVファイル名
    public string csvFileName = "JLPT.csv";

    // 単語リストと平仮名キュー
    private List<WordEntry> wordList = new List<WordEntry>();
    public Queue<string> hiraganaQueue = new Queue<string>();

    // 単語エントリのクラス
    [System.Serializable]
    public class WordEntry
    {
        public string expression;
        public string reading;
        public string meaning;
    }

    // Awakeメソッド（インスタンスの初期化）
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCSV();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // CSVファイルをロードするメソッド
    void LoadCSV()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);

        try
        {
            string csvText = File.ReadAllText(filePath);
            ParseCSV(csvText);
            FilterHiraganaWords();
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to load CSV: " + e.Message);
        }
    }

    // CSVファイルを解析するメソッド
    void ParseCSV(string csvText)
    {
        StringReader reader = new StringReader(csvText);
        reader.ReadLine(); // ヘッダー行をスキップ
        while (true)
        {
            string line = reader.ReadLine();
            if (line == null) break;
            var values = line.Split(',');
            if (values.Length >= 3)
            {
                WordEntry entry = new WordEntry
                {
                    expression = values[0],
                    reading = values[1],
                    meaning = values[2]
                };
                wordList.Add(entry);
            }
        }
        Debug.Log("Loaded " + wordList.Count + " words.");
    }

    // 平仮名のみの単語をフィルタリングするメソッド
    void FilterHiraganaWords()
    {
        Regex hiraganaRegex = new Regex(@"^[\u3040-\u309F]+$");
        wordList = wordList.FindAll(word => hiraganaRegex.IsMatch(word.reading));
        Debug.Log("Filtered " + wordList.Count + " hiragana words.");
    }

    // ランダムに単語を選択するメソッド
    public void SelectRandomWords(int count)
    {
        hiraganaQueue.Clear();
        System.Random random = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int index = random.Next(wordList.Count);
            string selectedWord = wordList[index].reading;
            foreach (char c in selectedWord)
            {
                hiraganaQueue.Enqueue(c.ToString());
            }
        }

        Debug.Log("Selected " + hiraganaQueue.Count + " hiragana characters.");
    }

    // 単語をキューに追加するメソッド
    public string AddWordToQueue(Queue<string> targetQueue)
    {
        System.Random random = new System.Random();
        int r = UnityEngine.Random.Range(0,100);
        if (r > 30) r = 3;
        else r = 2;
        string selectedWord;
        int addedCount = 0;
        do
        {
            int index = random.Next(wordList.Count);
            selectedWord = wordList[index].reading;
        } while (selectedWord.Length > r);

        foreach (char c in selectedWord)
        {
            targetQueue.Enqueue(c.ToString());
            addedCount++;
        }
        Debug.Log("Added " + selectedWord + " to the target queue.");

        return selectedWord;
    }

    // 辞書に単語が存在するか確認するメソッド
    public bool IsWordInDictionary(string word)
    {
        foreach (var entry in wordList)
        {
            if (entry.reading == word)
            {
                return true;
            }
        }
        return false;
    }
}
