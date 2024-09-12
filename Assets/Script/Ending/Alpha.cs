using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    public string sAlpha;
    public Ending ending;

    void Start()
    {
        ending =GameObject.Find("Ending").GetComponent<Ending>();
    }
    private void OnMouseUp()
    {
        ending.EnterAlpha(sAlpha);
    }
    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow; // �}�E�X�I�[�o�[���ɐF��ύX
    }
    void OnMouseExit()
    {
        Color c = Color.white;
        c.a = GetComponent<SpriteRenderer>().color.a;
        GetComponent<SpriteRenderer>().color = c; // �}�E�X�����ꂽ���ɐF�����ɖ߂�
    }
}
