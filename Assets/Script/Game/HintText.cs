using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintText : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public GameObject imageUI;
    float timeCnt;
    float showTime = 1.5f;
    private void Start()
    {
        timeCnt = 0;
        imageUI.SetActive(false);
    }
    private void Update()
    {
        timeCnt += Time.deltaTime;
        if (timeCnt > showTime)
        {
            ClearText();
            //imageUI.SetActive(true);
        }
    }
    public void ClearText()
    {
        imageUI.SetActive(false);
        hintText.text = "";
        
    }
    public void ShowText(string text)
    {
        imageUI.SetActive(true);
        hintText.text = text;
        timeCnt = 0;
    }
}
