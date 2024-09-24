using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    // プレハブやスロット管理に関する変数
    public GameObject HiraInScene;
    private Dictionary<char, GameObject> hiraganaPrefabMap;
    public GameObject[] prefabHira;
    private Queue<string> hiraQueue = new Queue<string>();
    private List<int> wordLengths = new List<int>();

    // 平仮名に関する変数
    public int hiraCount;
    public const int hiraMax = 8;
    public int hiraMaxNow = 8;
    public string spWordNow = null;
    public int spWordNowType = 0;
    public bool[,] hiraTable = new bool[7,3];
    struct pos
    {
        public int x, y;
    };

    // 特殊単語に関する変数
    private List<string> specialWords = new List<string>();
    private List<string> specialWords_Time = new List<string>();
    private List<string> specialWords_More = new List<string>();
    public string[] spWords;
    public string[] spWords_More;
    public string[] spWords_Time;

    // スロット管理の参照
    private SlotManager slotManager;

    // 初期化処理
    void Start()
    {
        slotManager = FindObjectOfType<SlotManager>();
        spWords_Time = new string[] 
        { 
            "きかん", "きげん", "じかん" , "とけい", "じだい", "じこく", "にちじ", "きじつ"
        };
        spWords_More = new string[]
        {
            "いくた","たよう","ぞうか","まし","ふやす","ます","ふえる","おおい"
        };
        spWords = new string[]
        {
            "きかん", "きげん", "じかん" , "とけい", "じだい", "じこく", "にちじ", "きじつ",
            "いくた","たよう","ぞうか","まし","ふやす","ます","ふえる","おおい"
        };
        for(int i=0;i<7;i++)
        {
            for(int j=0;j<3;j++)
            {
                hiraTable[i, j] = false;
            }
        }
        LoadHiraganaPrefabs();
        if (VocabularyManager.Instance != null)
        {
            foreach (string word in spWords)
            {
                specialWords.Add(word);
            }
            foreach (string word in spWords_More)specialWords_More.Add(word);
            foreach (string word in spWords_Time)specialWords_Time.Add(word);
            AddSpecialWord(1);
            AddWordToEnd();
            for (int i = 0; i < hiraMaxNow; i++)
            {
                AddHiraToScene();
            }
        }
        else
        {
            Debug.Log("VocabularyManager is null");
        }
    }

    // 毎フレームの更新処理
    private void Update()
    {
        int hiraNow = HiraInScene.transform.childCount;
        if (hiraNow <= hiraMaxNow)
        {
            AddHiraToScene();
        }
    }

    // 平仮名のプレハブをロードする関数
    void LoadHiraganaPrefabs()
    {
        hiraganaPrefabMap = new Dictionary<char, GameObject>
        {
            {'あ', prefabHira[0]},{'い', prefabHira[1]},{'う', prefabHira[2]},{'え', prefabHira[3]},{'お', prefabHira[4]},
            {'か', prefabHira[5]},{'き', prefabHira[6]},{'く', prefabHira[7]},{'け', prefabHira[8]},{'こ', prefabHira[9]},
            {'が', prefabHira[10]},{'ぎ', prefabHira[11]},{'ぐ', prefabHira[12]},{'げ', prefabHira[13]},{'ご', prefabHira[14]},
            {'さ', prefabHira[15]},{'し', prefabHira[16]},{'す', prefabHira[17]},{'せ', prefabHira[18]},{'そ', prefabHira[19]},
            {'ざ', prefabHira[20]},{'じ', prefabHira[21]},{'ず', prefabHira[22]},{'ぜ', prefabHira[23]},{'ぞ', prefabHira[24]},
            {'た', prefabHira[25]},{'ち', prefabHira[26]},{'つ', prefabHira[27]},{'て', prefabHira[28]},{'と', prefabHira[29]},
            {'だ', prefabHira[30]},{'ぢ', prefabHira[31]},{'づ', prefabHira[32]},{'で', prefabHira[33]},{'ど', prefabHira[34]},
            {'な', prefabHira[35]},{'に', prefabHira[36]},{'ぬ', prefabHira[37]},{'ね', prefabHira[38]},{'の', prefabHira[39]},
            {'は', prefabHira[40]},{'ひ', prefabHira[41]},{'ふ', prefabHira[42]},{'へ', prefabHira[43]},{'ほ', prefabHira[44]},
            {'ば', prefabHira[45]},{'び', prefabHira[46]},{'ぶ', prefabHira[47]},{'べ', prefabHira[48]},{'ぼ', prefabHira[49]},
            {'ぱ', prefabHira[50]},{'ぴ', prefabHira[51]},{'ぷ', prefabHira[52]},{'ぺ', prefabHira[53]},{'ぽ', prefabHira[54]},
            {'ま', prefabHira[55]},{'み', prefabHira[56]},{'む', prefabHira[57]},{'め', prefabHira[58]},{'も', prefabHira[59]},
            {'や', prefabHira[60]},{'ゆ', prefabHira[61]},{'よ', prefabHira[62]},{'ら', prefabHira[63]},{'り', prefabHira[64]},
            {'る', prefabHira[65]},{'れ', prefabHira[66]},{'ろ', prefabHira[67]},{'ん', prefabHira[68]},{'を', prefabHira[69]},
            {'ぁ', prefabHira[70]},{'ぃ', prefabHira[71]},{'ぅ', prefabHira[72]},{'ぇ', prefabHira[73]},{'ぉ', prefabHira[74]},
            {'ゃ', prefabHira[75]},{'ゅ', prefabHira[76]},{'ょ', prefabHira[77]},{'わ', prefabHira[78]},{'ゎ', prefabHira[79]},
            {'っ', prefabHira[80]},
        };
    }

    // シーンに平仮名を追加する関数
    public void AddHiraToScene()
    {
        if (hiraQueue.Count > 0)
        {
            string hira = hiraQueue.Dequeue();
            hiraCount++;
            char hira1 = hira[0];
            GameObject newHira = SpawnHiragana(hira1);
            newHira.transform.SetParent(HiraInScene.transform, false);
            LengthManage(1);
        }
        else
        {
            Debug.Log("Word list is Null");

            AddWordToEnd();
            AddHiraToScene();
        }
    }
    private pos ReturnNullMap()
    {
        int x;
        int y;
        int t = 0;
        pos re;
        do
        {
            x = Random.Range(0, 7);
            y = Random.Range(0, 3);
            t++;
            if (t > 200) break;
        } while (hiraTable[x, y] != false);

        re.x = x-5;
        re.y = y-1;
        return re;
        
    }
    private void ShowMap()
    {
        string line;
        for(int i=0;i<3;i++)
        {
            line = "";
            for(int j=0;j<7;j++)
            {
                line += hiraTable[j, i].ToString();
            }
            Debug.Log(i+" "+line);
        }
    }
    private void ResetMap()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                hiraTable[j, i]=false;
            }
        }
    }
    // 平仮名を生成する関数
    public GameObject SpawnHiragana(char hiragana)
    {
        GameObject prefab = GetPrefabForHiragana(hiragana);
        if (prefab != null)
        {
            //float rx = Random.Range(-6.0f, 2.2f);
            //float ry = Random.Range(-1.6f, 2.0f);
            pos p= ReturnNullMap();
            Vector3 pos = new Vector3(p.x, p.y, 0);
            GameObject newHira = Instantiate(prefab, pos, Quaternion.identity);
            hiraTable[p.x + 5, p.y + 1]=true;
            //ShowMap();
            return newHira;
        }
        else
        {
            Debug.Log("prefab is null");
            return null;
        }
    }

    // 平仮名に対応するプレハブを取得する関数
    private GameObject GetPrefabForHiragana(char hiragana)
    {
        if (hiraganaPrefabMap.TryGetValue(hiragana, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogWarning("Prefab not found for hiragana: " + hiragana);
            return null;
        }
    }

    // 単語をキューの末尾に追加する関数
    private void AddWordToEnd()
    {
        int rate = Random.Range(0, 100);
        string word;
        if (rate >= 60)
        {
            int rmax = specialWords.Count;
            int index = Random.Range(0, rmax);
            word = specialWords[index];
        }
        else
        {
            word = VocabularyManager.Instance.AddWordToQueue(hiraQueue);
        }
        int length = word.Length;
        wordLengths.Add(length);
        Debug.Log("add " + length + " hiragana to end");
        slotManager.AddSlot(word);
    }

    // 単語をキューの先頭に追加する関数
    public void AddWordToFirst(string word_)
    {
        if (word_.Length <= 0)
        {
            Debug.Log("word length is 0");
            return;
        }
        Stack<string> tempStack = new Stack<string>();
        int hiraCnt = 0;
        foreach (char c in word_)
        {
            tempStack.Push(c.ToString());
            hiraCnt++;
        }
        InsertAtQueueHead(hiraQueue, tempStack);
        wordLengths.Insert(0, hiraCnt);
    }

    // キューの先頭に要素を挿入する関数
    private void InsertAtQueueHead(Queue<string> queue, Stack<string> tempStack)
    {
        Queue<string> tempQueue = new Queue<string>(queue);
        queue.Clear();
        while (tempStack.Count > 0)
        {
            string temp = tempStack.Pop();
            queue.Enqueue(temp);
        }
        foreach (var element in tempQueue)
        {
            queue.Enqueue(element);
        }
    }

    // 単語リストをリセットする関数
    public void ResetWordList()
    {
        hiraQueue.Clear();
        //wordLengths.clear();
        AddWordToEnd();
        for (int i = 0; i < hiraMaxNow; i++)
        {
            AddHiraToScene();
        }
    }

    // 単語の長さを管理する関数
    private void LengthManage(int n_)
    {
        if (wordLengths.Count == 0)
        {
            Debug.LogWarning("wordLengths is empty, cannot manage queue.");
            return;
        }
        if (wordLengths[0] <= 0)
        {
            wordLengths.RemoveAt(0);
            while (wordLengths.Count < 3)
            {
                AddWordToEnd();
            }
        }
        wordLengths[0] -= n_;
    }

    // 特殊単語を追加する関数
    public string AddSpecialWord(int type = 0, string word = null)
    {
        if (string.IsNullOrEmpty(word))
        {
            if (type == 1)
            {
                int rmax = specialWords_Time.Count;
                int index = Random.Range(0, rmax);
                word = specialWords_Time[index];
            }
            else if(type == 2)
            {
                int rmax = specialWords_More.Count;
                int index = Random.Range(0, rmax);
                word = specialWords_More[index];
            }
            else if (type == 0)
            {
                word = VocabularyManager.Instance.AddWordToQueue(hiraQueue);
            }
        }

        foreach (char c in word)
        {
            hiraQueue.Enqueue(c.ToString());
        }

        slotManager.AddSpecialSlot(word, type);
        Debug.Log("Added special word to the queue: " + word);

        int length = word.Length;
        wordLengths.Add(length);
        spWordNowType = type;
        spWordNow = word;
        return word;
    }

    // 全ての平仮名をリフレッシュする関数
    public void RefreshAllHira()
    {
        slotManager.ClearAllGroup();
        int leng = HiraInScene.transform.childCount;
        Debug.Log("GetHiraInScene's Child" + leng.ToString());
        ResetMap();
        for (int i = leng - 1; i >= 0; i--)
        {
            GameObject toDelete = HiraInScene.transform.GetChild(i).gameObject;
            Destroy(toDelete);
            Debug.Log("RefreshDelete " + i.ToString());
        }

        hiraQueue.Clear();
        wordLengths.Clear();
        hiraMaxNow = 8;
        int ra= Random.Range(0, 3);
        AddSpecialWord(ra);
        AddWordToEnd();

        for (int i = 0; i < hiraMaxNow; i++)
        {
            AddHiraToScene();
        }
    }
}
