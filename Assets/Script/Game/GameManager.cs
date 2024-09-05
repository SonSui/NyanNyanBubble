using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager�N���X�̒�`
public class GameManager : MonoBehaviour
{
    // UI�v�f�ƃ}�l�[�W���[�̎Q��
    public TextMeshProUGUI scoreText;
    //public Text timeText;
    public WordManager wordManager;
    public SlotManager slotManager;
    public int timeMax = 99;
    public ProcessController processController;
    public ScoreManager scoreManager;
    public AudioManager audioManager;

    // ���݂̕������̃L���[
    private Queue<Hiragana> hiraNow;

    // ����P��̔z��
    public string[] specialWords_Time;
    public string[] specialWords_Speed;
    public string[] specialWords_Stop;
    public string[] specialWords_Slow;
    public string[] specialWords_More;
    public string[] specialWords_Score;

    // ���ʕ\���p�e�L�X�g
    private ResultText resultText;
    //private List<string> currentWord;

    // ����P��Ɋ֘A����ϐ�
    private string SPWord;
    private int SPWordType;
    private int spIndex;

    // �X�R�A�Ǝ��ԂɊւ���ϐ�
    private int currentScore = 1;
    private float currentTime;
    private int score;
    private int scoreTemp;

    private bool isKanaboost = false;
    private float kanaboostTime = 0;

    // JLPT�����}�l�[�W���[
    VocabularyManager JLPTmanager;

    // �Q�[���J�n���̏���������
    void Start()
    {
        // �X�R�A�Ǝ��Ԃ̃e�L�X�g��null�łȂ����Ƃ��m�F
        if (scoreText == null) Debug.Log("ScoreText is null");
        hiraNow = new Queue<Hiragana>();
        JLPTmanager = GameObject.Find("JLPTLoader").GetComponent<VocabularyManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();    
        resultText = GameObject.Find("ResultManager").GetComponent<ResultText>();
        specialWords_Time = new string[] { "������", "������", "������", "�Ƃ���", "������", "������", "�ɂ���", "������" };
        score = 0;
        currentTime = timeMax;
        UpdateScore();
        UpdateTime();
    }

    // ���t���[���̍X�V����
    void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateScore();
        UpdateTime();

        // ���Ԃ�0�ɂȂ�����Q�[���I�[�o�[
        if (currentTime <= 0)
        {
            scoreManager.finalScore = score;
            SceneManager.LoadScene("GameOver");
        }
        if(kanaboostTime > 0)
        {
            kanaboostTime -= Time.deltaTime;
            wordManager.hiraMaxNow = 13;
        }
        else
        {
            wordManager.hiraMaxNow = 8;
        }

        SPWord = wordManager.spWordNow;
        SPWordType = wordManager.spWordNowType;
    }

    // ���������I�����ꂽ�Ƃ��̏���
    public void OnHiraganaSelected(Hiragana hiraScript)
    {
        audioManager.PlaySoundEffect(0);
        string spHiraWord = SpHiraNow();
        if (spHiraWord == hiraScript.hiragana) 
        {
            MoveToSpSlot(hiraScript);
            return;
        }
        GameObject currGroup = GetCurrentGroup();
        if (currGroup == null)
        {
            Debug.Log("no group");
            return;
        }
        int length = currGroup.transform.childCount;
        int i;
        string wordNow=null;
        for (i = 0; i < length; i++)
        {
            // �O���[�v�̃X���b�g�ɕ�������z�u
            Transform g = currGroup.transform.GetChild(i);
            if (g.GetComponent<Slot>().isFilled == false)
            {
                g.GetComponent<Slot>().isFilled = true;
                g.GetComponent<Slot>().SetHira(hiraScript);
                hiraNow.Enqueue(hiraScript);
                wordNow += hiraScript.hiragana;
                break;
            }
            else
            {
                string h = g.GetComponent<Slot>().GetHira().hiragana;
                wordNow += h;
            }
        }
        if (i == length - 1)
        {
            bool isValidWord = ValidateCurrentWord(wordNow);
            CheckQueue();
            if (isValidWord)
            {
                // �������P��̏ꍇ�A���������ړ��A�X�R�A�����Z
                for (int cc = 0; cc < length; cc++)
                {
                    Transform g = currGroup.transform.GetChild(cc);
                    Hiragana h = hiraNow.Dequeue();
                    h.MoveTo(g.position);
                    h.transform.SetParent(g.transform);
                }
                currentScore = length * (length - 1);
                if (currentScore < 1) { currentScore = 1; }
                scoreTemp = currentScore;
                score += scoreTemp;
                string ttt = "+" + scoreTemp.ToString() + "�_";
                resultText.ShowText(ttt);
                slotManager.RemoveSlot(currGroup);
            }

            else
            {
                // �Ԉ�����P��̏ꍇ�A�X���b�g�����Z�b�g
                for (int cc = 0; cc < length; cc++)
                {
                    Transform g = currGroup.transform.GetChild(cc);
                    Hiragana h = hiraNow.Dequeue();
                    h.MoveToRound(g.position);
                    g.GetComponent<Slot>().isFilled = false;
                }
            }
        }
        
    }

    public void OnHiraganaCancel(Hiragana target)
    {
        bool isTargetExists = false;
        isTargetExists=DeleteHiraInQueue(target);
        if (isTargetExists)
        {
            Debug.Log(target + " found in queue");
            audioManager.PlaySoundEffect(1);
            UpdateGroup(target);
        }
        else
        {
            Debug.Log(target + " not found in queue");
        }
    }
    private void CheckQueue()
    {
        int i = 0;
        Queue<Hiragana> tempQ = new Queue<Hiragana>();
        while (hiraNow.Count>0)
        {
            Hiragana temp = hiraNow.Dequeue();
            Debug.Log("CheckQ " + i + ":" + temp);
            tempQ.Enqueue(temp);
            i++;
        }
        hiraNow.Clear();
        while(tempQ.Count > 0)
        {
            Hiragana temp =tempQ.Dequeue();
            hiraNow.Enqueue(temp);
        }
    }
    // �L���[���畽�������폜
    public bool DeleteHiraInQueue(Hiragana target)
    {
        bool isFound = false;
        if (target == null || hiraNow.Count == 0) return false;

        Queue<Hiragana> tempQ = new Queue<Hiragana>();
        Hiragana temp = hiraNow.Dequeue();
        if (temp ==target) isFound = true;
        else tempQ.Enqueue(temp);
        while (hiraNow.Count > 0)
        {
            Debug.Log("que while");
            temp = hiraNow.Dequeue();
            if (temp == target)
            {
                isFound = true;
                continue;
            }
            tempQ.Enqueue(temp);
        }
        hiraNow.Clear();
        while (tempQ.Count > 0)
        {
            temp = tempQ.Dequeue();
            hiraNow.Enqueue(temp);
        }
        return isFound;
    }
    private void UpdateGroup(Hiragana target)
    {
        //�Ђ炪�Ȃ��폜����ƃO���[�v���X�V
        if (target == null) return;
        int cm = slotManager.wordGroups.Count;
        if (cm > 0)
        {
            foreach (GameObject group in slotManager.wordGroups)
            {
                int chilN = group.transform.childCount;
                for (int i = 0; i < chilN; i++)
                {
                    Transform groupSlot = group.transform.GetChild(i);
                    bool slotFill = groupSlot.GetComponent<Slot>().isFilled;
                    if (groupSlot.GetComponent<Slot>().GetHira() == target&&slotFill)
                    {
                        Debug.Log("Target found in slot");
                        int j = i;
                        i++;
                        for (; i < chilN; i++)
                        {
                            Transform preSlot = group.transform.GetChild(j);
                            Transform curSlot = group.transform.GetChild(i);
                            preSlot.GetComponent<Slot>().isFilled = curSlot.GetComponent<Slot>().isFilled;
                            preSlot.GetComponent<Slot>().SetHira(curSlot.GetComponent<Slot>().GetHira());
                            j++;
                        }
                        i--;
                        Transform lastSlot = group.transform.GetChild(i);
                        lastSlot.GetComponent<Slot>().isFilled = false;
                        lastSlot.GetComponent<Slot>().SetHira(null);
                    }
                }
            }
        }
    }
    

    // �X�R�A�̍X�V
    private void UpdateScore()
    {
        scoreText.text = "�_��\n" + score.ToString();
    }

    // �c�莞�Ԃ̍X�V
    private void UpdateTime()
    {
        //timeText.text = "Time:" + currentTime.ToString("F0");
        processController.currentTime = currentTime;
    }

    // ���݂̃O���[�v���擾
    private GameObject GetCurrentGroup()
    {
        Debug.Log("in get current");
        int cm = slotManager.wordGroups.Count;
        if (cm > 0)
        {
            foreach (GameObject group in slotManager.wordGroups)
            {
                int chilN = group.transform.childCount;
                for (int i = 0; i < chilN; i++)
                {
                    Transform groupSlot = group.transform.GetChild(i);
                    if (groupSlot.GetComponent<Slot>().isFilled == false)
                    {
                        return group;
                    }
                }
            }
        }
        Debug.Log("No group or no empty group");
        return null;
    }

    // ���݂̒P�������
    public bool ValidateCurrentWord(string formedWord)
    {
        //string formedWord = string.Join("", currentWord.ToArray());
        string englishMeaning;

        foreach (string word in specialWords_Time)
        {
            if (word == formedWord)
            {
                string ttt = "+5 �b";
                resultText.ShowText(ttt);
                currentTime += 5;
                return true;
            }
        }
        foreach(string word in specialWords_More) 
        {
            if (word == formedWord)
            {
                string ttt = "+5����";
                resultText.ShowText(ttt);
                return true;
            }
        }

        bool isJLPT = JLPTmanager.IsWordInDictionary(formedWord);
        if (isJLPT) { return true; }

        if (JMDictParser.Instance == null)
        {
            Debug.LogError("JMDictParser.Instance is null");
            return false;
        }

        if (JMDictParser.Instance.IsWordValid(formedWord, out englishMeaning))
        {
            Debug.Log($"The word '{formedWord}' is valid! Meaning: {englishMeaning}");
            UpdateScore();
            return true;
        }
        else
        {
            Debug.Log($"The word '{formedWord}' is not valid.");
            return false;
        }
    }

    // ���݂̓��ꕽ�������擾
    private string SpHiraNow()
    {
        if (spIndex > SPWord.Length - 1)
        {
            Debug.Log("SpIndex Overflow");
            return null;
        }
        string spHira = SPWord[spIndex].ToString();
        return spHira;
    }

    // ���ꕽ�������X���b�g�Ɉړ�
    private void MoveToSpSlot(Hiragana hira)
    {
        GameObject group = slotManager.specialGroup;
        Transform parent;

        if (group.transform.childCount > 0 && group != null)
        {
            Transform preP = group.transform.GetChild(0);
            if (preP.childCount > spIndex && preP != null)
            {
                parent = preP.GetChild(spIndex);
                Hiragana pChild0 = parent.GetChild(0).GetComponent<Hiragana>();
                if (pChild0.hiragana != hira.hiragana) Debug.Log("SpCompare Error");
                hira.MoveTo(parent.position);
                hira.transform.SetParent(parent);
                spIndex++;
                if (spIndex >= SPWord.Length)
                    SpWordComplete(preP.gameObject);
            }
            else
            {
                Debug.Log("Sp Slot Length Error");
                return;
            }
        }
        else
        {
            Debug.Log("SpSlot Null");
            return;
        }
    }

    // ����P�ꂪ���������Ƃ��̏���
    void SpWordComplete(GameObject SpGroupNow)
    {
        spIndex = 0;
        slotManager.RemoveSpecialSlot(SpGroupNow);
        string rText;
        int scoreTemp = SPWord.Length * 5;
        score += scoreTemp;
        UpdateScore();

        switch (SPWordType)
        {
            case 0:
                rText = "+" + scoreTemp.ToString();
                resultText.ShowText(rText);
                break;
            case 1:
                rText = "+" + scoreTemp.ToString() + "�_" + "  +5�b";
                currentTime += 5;
                UpdateTime();
                resultText.ShowText(rText);
                break;
            case 2:
                rText = "+" + scoreTemp.ToString() + "�_" + "  +5����";
                //currentTime += 5;
                kanaboostTime = 3;
                UpdateTime();
                resultText.ShowText(rText);
                break;
        }
    }

    // �������ƃO���[�v�����Z�b�g
    public void ResetHiraAndGroup(bool isDeduction)
    {
        if (isDeduction)
        {
            if (score > 20) score -= 20;
            else score = 0;
            string rText = "-20�_";
            resultText.ShowText(rText);
        }
        hiraNow.Clear();
        spIndex = 0;
        wordManager.RefreshAllHira();
    }
}