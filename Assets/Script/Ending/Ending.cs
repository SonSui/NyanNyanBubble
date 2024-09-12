using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI nameText;
    private ScoreManager scoreManager;
    private int score;
    private string userName;
    // Start is called before the first frame update
    void Start()
    {
        userName = "";
        scoreManager = FindAnyObjectByType<ScoreManager>();
        score = scoreManager.finalScore;
        text.text = "ScoreÅF"+score;
        nameText.text = "Name:|";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterAlpha(string alpha)
    {
        int i = userName.Length;
        Debug.Log("NameLength:" + i.ToString());
        if (i < 20)
        {
            userName += alpha;
            nameText.text = "Name:" + userName + "|";
        }
    
    }
    public void ButtonContinueDown()
    {
        string data=score.ToString();
        DateTime currentTime = DateTime.Now;
        data +="$" + currentTime.ToString("MM/dd") + "$" + currentTime.ToString("HH:mm") + "$" + userName + "$";
        scoreManager.InsertAndSortRecord(data);
        SceneManager.LoadScene("Ranking");
    }
}
