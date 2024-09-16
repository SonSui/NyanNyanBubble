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
    public GameObject keyBoard;
    private int score;
    private string userName;
    private float keyTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        userName = "";
        scoreManager = FindAnyObjectByType<ScoreManager>();
        score = scoreManager.finalScore;
        text.text = "ScoreF"+score;
        nameText.text = "Name:|";
        keyBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        keyTime += Time.deltaTime;
        if(keyTime>0.5)
            keyBoard.SetActive(true);
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
