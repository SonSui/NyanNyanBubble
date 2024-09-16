using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int finalScore;
    private string filePath;
    private int newRecordPos=-1;
    private TextMeshProUGUI rankText;
    private TextMeshProUGUI scoreText;
    public static ScoreManager instance;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        filePath = Application.dataPath + "/../ScoreData.txt";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /*private void OnEnable()
    {
        SceneCheck();
    }*/

    // Update is called once per frame

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New scene loaded: " + scene.name);
        SceneCheck();
    }

    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void SceneCheck()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "GameOver")
        {
            Debug.Log("GameOverScene Checked");
            return;
        }
        else if (scene.name == "GameScene")
        {
            Debug.Log("GameScene Checked");
            return;
        }
        else if (scene.name == "Title")
        {
            Debug.Log("Title Checked");
            newRecordPos = -1;
            return;
        }
        else if (scene.name == "Ranking")
        {
            
            Debug.Log("Ranking Checked");
            rankText = GameObject.Find("RankingText").GetComponent<TextMeshProUGUI>();
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); 
            DisplayRankings();

            return;
        }
        Debug.Log("NoScene Checked");
    } 
    public int InsertAndSortRecord(string newRecord)
    {
        List<string> records = new List<string>();
        int newScore = ExtractScore(newRecord);

        
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            
            for (int i = 1; i < lines.Length; i++)
            {
                records.Add(lines[i]);
            }
        }

        
        records.Add(newRecord);

        
        records.Sort((record1, record2) =>
        {
            int score1 = ExtractScore(record1);
            int score2 = ExtractScore(record2);
            return score2.CompareTo(score1); // 大きいから並べる
        });

        int newRecordIndex = records.IndexOf(newRecord) + 1;
        newRecordPos = newRecordIndex;

        string updatedData = records.Count + "$\n";

        
        foreach (string record in records)
        {
            updatedData += record + "\n";
        }

        // 保存
        try
        {
            File.WriteAllText(filePath, updatedData);
            Debug.Log("Records updated and sorted. New record position: " + newRecordIndex);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save records: " + e.Message);
        }

        
        return newRecordIndex;
    }

    // Data Extract
    private int ExtractScore(string record)
    {
        // データの形：点数$日付$時間$名前$
        string[] parts = record.Split('$');
        if (parts.Length > 0 && int.TryParse(parts[0], out int score))
        {
            return score;
        }
        return 0; 
    }
    public void DisplayRankings()
    {
        List<string> records = new List<string>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                records.Add(lines[i]);
            }
        }

        string rankDisplayText = "";
        string scoreDisplayText = "";
        int maxDisplay = 6;

        if (newRecordPos < 0)
        {
            for (int i = 0; i < Mathf.Min(records.Count, maxDisplay); i++)
            {
                rankDisplayText += $"{i + 1}. {GetPlayerName(records[i])}\n";
                scoreDisplayText += $"{GetPlayerScore(records[i])}\n";
            }
        }
        else if (newRecordPos > 0 && newRecordPos <= 6)
        {
            for (int i = 0; i < Mathf.Min(records.Count, maxDisplay); i++)
            {
                if (i + 1 == newRecordPos)
                {
                    rankDisplayText += $"<color=#FFA500>{i + 1}. {GetPlayerName(records[i])}</color>\n";
                    scoreDisplayText += $"<color=#FFA500>{GetPlayerScore(records[i])}</color>\n";
                }
                else
                {
                    rankDisplayText += $"{i + 1}. {GetPlayerName(records[i])}\n";
                    scoreDisplayText += $"{GetPlayerScore(records[i])}\n";
                }
            }
        }
        else if (newRecordPos > 6)
        {
            for (int i = 0; i < Mathf.Min(records.Count, 5); i++)
            {
                rankDisplayText += $"{i + 1}. {GetPlayerName(records[i])}\n";
                scoreDisplayText += $"{GetPlayerScore(records[i])}\n";
            }

            rankDisplayText += $"<color=#FFA500>{newRecordPos}. {GetPlayerName(records[newRecordPos - 1])}</color>\n";
            scoreDisplayText += $"<color=#FFA500>{GetPlayerScore(records[newRecordPos - 1])}</color>\n";
        }

        rankText.text = rankDisplayText;  
        scoreText.text = scoreDisplayText;
    }
    private string GetPlayerName(string record)
    {
        
        string[] parts = record.Split('$');
        return parts.Length >= 4 ? parts[3] : "Unknown";
    }
    private string GetPlayerScore(string record)
    {
        string[] parts = record.Split('$');
        return parts.Length > 0 ? parts[0] : "0";
    }
}
