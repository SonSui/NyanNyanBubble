using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingSlot : MonoBehaviour
{
    public GameObject holdingSlot;
    private int slotNum;
    public List<Slot> slots = new List<Slot>();
    // Start is called before the first frame update
    void Start()
    {
        slotNum = holdingSlot.transform.childCount;
    }
    void Update()
    {
        for(int i = 0;i < slots.Count;i++)
        {
            if (slots[i].isFilled==true) 
            {
                if(slots[i].transform.childCount==0)
                {
                    slots[i].isFilled = false;
                }
                else
                {
                    slots[i].transform.GetChild(0).GetComponent<Hiragana>().NoMovingOption();
                }
            }
        }
    }

    // Update is called once per frame
    public void AddHira(Hiragana hira)
    {
        for (int i = 0;i < slots.Count;i++)
        {
            if (slots[i].isFilled==false)
            {
                slots[i].isFilled = true;
                hira.MoveTo(slots[i].transform.position);
                hira.transform.SetParent(slots[i].transform);
                hira.GetComponent<SpriteRenderer>().sortingOrder = 5;
                hira.originalPosition = slots[i].transform.position;
                return;
            }
        }
        hira.WarningMode();
    }
    
}
