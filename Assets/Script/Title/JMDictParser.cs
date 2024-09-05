using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WordInfo
{
    public string Japanese { get; set; } // ���{��̒P��
    public string English { get; set; }  // �p��̈Ӗ�
}

public class JMDictParser : MonoBehaviour
{
    public static JMDictParser Instance { get; private set; } // �V���O���g���C���X�^���X
    private Dictionary<string, WordInfo> wordDictionary; // �P��̎���

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �V�[�����؂�ւ���Ă��I�u�W�F�N�g��j�����Ȃ�
            InitializeDictionary(); // ������������
        }
        else
        {
            Destroy(gameObject);  // �����̃C���X�^���X�����݂���ꍇ�A���݂̃I�u�W�F�N�g��j��
        }
    }

    private void InitializeDictionary()
    {
        wordDictionary = new Dictionary<string, WordInfo>();
        string filePath = Application.streamingAssetsPath + "/JMdict_e.xml"; // ������XML�t�@�C���̃p�X

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;
        settings.MaxCharactersFromEntities = long.MaxValue; // �G���e�B�e�B����̍ő啶�����̐�����ݒ�

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
                    kanji = kanjiElements[0].InnerText; // �����̃G���g�����擾
                }

                if (readingElements.Count > 0)
                {
                    kana = readingElements[0].InnerText; // �����̃G���g�����擾
                }

                foreach (XmlNode sense in senseElements)
                {
                    english += sense.InnerText + "; "; // �p��̈Ӗ�������
                }

                if (!string.IsNullOrEmpty(english))
                {
                    english = english.TrimEnd(' ', ';'); // �Ō�̃Z�~�R�����Ƌ󔒂��폜
                }

                if (!string.IsNullOrEmpty(kana) && !wordDictionary.ContainsKey(kana))
                {
                    wordDictionary[kana] = new WordInfo { Japanese = kana, English = english }; // �����ɉ����G���g����ǉ�
                }

                if (!string.IsNullOrEmpty(kanji) && !wordDictionary.ContainsKey(kanji))
                {
                    wordDictionary[kanji] = new WordInfo { Japanese = kanji, English = english }; // �����Ɋ����G���g����ǉ�
                }
            }
        }
    }

    public bool IsWordValid(string word, out string english)
    {
        if (wordDictionary.TryGetValue(word, out WordInfo wordInfo))
        {
            english = wordInfo.English; // �p��̈Ӗ���Ԃ�
            return true; // �P�ꂪ�����ɑ��݂���
        }
        else
        {
            english = null; // �P�ꂪ������Ȃ��ꍇ��null��Ԃ�
            return false; // �P�ꂪ�����ɑ��݂��Ȃ�
        }
    }

    void Start()
    {
        string word = "��������"; // ��������P��
        string meaning;
        if (IsWordValid(word, out meaning))
        {
            Debug.Log($"{word} means {meaning}"); // �P��̈Ӗ���\��
        }
        else
        {
            Debug.Log("Word Not Found"); // �P�ꂪ������Ȃ��ꍇ�̃��b�Z�[�W
        }
    }
}
