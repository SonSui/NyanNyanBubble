using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultText : MonoBehaviour
{
    public TextMeshProUGUI text;
    float cntTime;
    public float showTime = 1.8f;
    public GameObject imageUI;
    
    void Start()
    {
        cntTime = 0;
        imageUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        cntTime += Time.deltaTime;
        if (cntTime > showTime)
        {
            ClearText();
            
        }
    }
    public void ClearText()
    {
        text.text = null;
        imageUI.SetActive(false);
    }
    public void ShowText(string te_)
    {
        if (cntTime > showTime)
        {
            cntTime = 0;
            text.text = te_;
            imageUI.SetActive(true);
        }
        //GetComponent<AudioSource>().Play();
    }
    public void ShowTextWithAudio(string st_)
    {
        if (cntTime > showTime)
        {
            cntTime = 0;
            text.text = st_;
            imageUI.SetActive(true);
        }
        GetComponent<AudioSource>().Play();
    }
}
