using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ending : MonoBehaviour
{
    public TextMeshProUGUI text;
    private ScoreManager scoreManager;
    private int score;
    // Start is called before the first frame update
    void Start()
    {

        scoreManager = FindAnyObjectByType<ScoreManager>();
        score = scoreManager.finalScore;
        text.text = "ìæì_ÅF"+score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
