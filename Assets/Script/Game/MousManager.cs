using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    //private bool isMonjiHit = false; 
    private GameManager gameManager; // GameManagerへの参照
    private HoldingSlot holdingSlot;
    int cnt = 0; // カウント変数

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManagerを見つける
        holdingSlot = FindObjectOfType<HoldingSlot>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウスの位置を取得

        if (Input.GetMouseButtonUp(0)) // マウスの左ボタンが押されている場合
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero); // すべてのヒットを取得
            if (hits.Length > 0)
            {
               
                                  
                RaycastHit2D hit = hits[0];
                Hiragana hiraganaScript = hit.collider.gameObject.GetComponent<Hiragana>();
                if (hiraganaScript != null && !hiraganaScript.isSelect()) // 既に選択されていないかどうかを確認
                {
                    hiraganaScript.OnSelected(); // 平仮名を選択状態にする
                    cnt++;
                    Debug.Log("Mouse is over: " + hit.collider.gameObject.name); // デバッグメッセージ
                    string hiragana = hiraganaScript.hiragana;
                    gameManager.OnHiraganaSelected(hiraganaScript); // GameManagerに通知
                }
            
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            if (hits.Length > 0)
            {
                RaycastHit2D hit = hits[0];
                Hiragana hiraganaScript = hit.collider.gameObject.GetComponent<Hiragana>();
                if (hiraganaScript != null && hiraganaScript.isSelect()) // 既に選択されていないかどうかを確認
                {
                    Debug.Log("Mouse is cancelling : " + hit.collider.gameObject.name); // デバッグメッセージ
                    string hiragana = hiraganaScript.hiragana;
                    gameManager.OnHiraganaCancel(hiraganaScript);// GameManagerに通知
                    hiraganaScript.CancelSelect();
                }
            }
        }
        /*else if (Input.GetMouseButtonUp(0) && isMonjiHit) // マウスの左ボタンが離された場合
        {
            
            cnt = 0;
        }*/
        /*if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero); // すべてのヒットを取得
            if (hits.Length > 0)
            {
                
                                   
                RaycastHit2D hit = hits[0];
                Hiragana hiraganaScript = hit.collider.gameObject.GetComponent<Hiragana>();
                if (hiraganaScript != null && !hiraganaScript.isSelect()) // 既に選択されていないかどうかを確認
                {
                    hiraganaScript.OnSelected();
                    //gameManager.DeleteHiraInQueue(hiraganaScript);
                    gameManager.OnSPHiraganaSelected(hiraganaScript);
                }
            }
        }*/
    }
}
