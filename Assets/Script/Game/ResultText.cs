using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultText : MonoBehaviour
{
    public TextMeshProUGUI text;
    float cntTime;
    public float showTime = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        cntTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cntTime += Time.deltaTime;
        if (cntTime > showTime) ClearText();
    }
    public void ClearText()
    {
        text.text = null;
    }
    public void ShowText(string te_)
    {
        if (cntTime > showTime)
        {
            cntTime = 0;
            text.text = te_;
        }
        GetComponent<AudioSource>().Play();
    }
}
