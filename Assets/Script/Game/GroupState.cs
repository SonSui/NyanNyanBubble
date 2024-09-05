using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupState : MonoBehaviour
{
    public bool isHint;
    public bool isDistroying = false;
    public string thisWord;
    public bool isSpecial = false;
    public GameObject catPaw;
    private SlotManager slotManager;
    private bool isDistoryable = false;
    private bool isPlayingAnimation = false;
    

    void Start()
    {
        slotManager = GameObject.Find("SlotManager").GetComponent<SlotManager>();
    }

    void Update()
    {
        if (isDistroying && !isPlayingAnimation)
        {
            int n = this.transform.childCount;
            for (int i = 0; i < n; i++)
            {
                isDistoryable = true;
                Transform c = transform.GetChild(i);
                int cchild = c.transform.childCount;
                for (int j = 0; j < cchild; j++)
                {
                    Hiragana h = c.transform.GetChild(j).GetComponent<Hiragana>();
                    if (h != null)
                    {
                        if (h.isMoving())
                        {
                            isDistoryable = false;
                            break;
                        }
                    }
                }
                if (!isDistoryable) break;
            }

            if (isDistoryable && slotManager != null)
            {
                StartCoroutine(PlayAnimation());
            }
        }
    }
    
    IEnumerator PlayAnimation()
    {
        isPlayingAnimation = true; 

        float waitTimeMax = 0.5f;
        float waitTime = 0;
        Vector3 pos = transform.position;
        GameObject newPaw = Instantiate(catPaw, pos, Quaternion.identity);
        newPaw.transform.SetParent(transform);
        Destroy(newPaw, waitTimeMax);

        while (waitTime < waitTimeMax)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        int type;
        if (isSpecial) type = 1;
        else type = 0;
        slotManager.MoveSlot(type);
        Destroy(this.gameObject);

        isPlayingAnimation = false; 
    }
}