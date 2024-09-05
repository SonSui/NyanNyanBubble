using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isFilled = false;
    private Hiragana holdingHira;
    private void Start()
    {
        
    }
    public void SetHira(Hiragana newHira)
    {
        holdingHira = newHira;
    }
    public Hiragana GetHira()
    {
        return holdingHira;
    }
    public void RemoveHira()
    {
        holdingHira = null;
    }
}
