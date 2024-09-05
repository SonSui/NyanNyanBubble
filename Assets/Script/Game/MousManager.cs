using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    //private bool isMonjiHit = false; 
    private GameManager gameManager; // GameManager�ւ̎Q��
    private HoldingSlot holdingSlot;
    int cnt = 0; // �J�E���g�ϐ�

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager��������
        holdingSlot = FindObjectOfType<HoldingSlot>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // �}�E�X�̈ʒu���擾

        if (Input.GetMouseButtonUp(0)) // �}�E�X�̍��{�^����������Ă���ꍇ
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero); // ���ׂẴq�b�g���擾
            if (hits.Length > 0)
            {
               
                                  
                RaycastHit2D hit = hits[0];
                Hiragana hiraganaScript = hit.collider.gameObject.GetComponent<Hiragana>();
                if (hiraganaScript != null && !hiraganaScript.isSelect()) // ���ɑI������Ă��Ȃ����ǂ������m�F
                {
                    hiraganaScript.OnSelected(); // ��������I����Ԃɂ���
                    cnt++;
                    Debug.Log("Mouse is over: " + hit.collider.gameObject.name); // �f�o�b�O���b�Z�[�W
                    string hiragana = hiraganaScript.hiragana;
                    gameManager.OnHiraganaSelected(hiraganaScript); // GameManager�ɒʒm
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
                if (hiraganaScript != null && hiraganaScript.isSelect()) // ���ɑI������Ă��Ȃ����ǂ������m�F
                {
                    Debug.Log("Mouse is cancelling : " + hit.collider.gameObject.name); // �f�o�b�O���b�Z�[�W
                    string hiragana = hiraganaScript.hiragana;
                    gameManager.OnHiraganaCancel(hiraganaScript);// GameManager�ɒʒm
                    hiraganaScript.CancelSelect();
                }
            }
        }
        /*else if (Input.GetMouseButtonUp(0) && isMonjiHit) // �}�E�X�̍��{�^���������ꂽ�ꍇ
        {
            
            cnt = 0;
        }*/
        /*if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero); // ���ׂẴq�b�g���擾
            if (hits.Length > 0)
            {
                
                                   
                RaycastHit2D hit = hits[0];
                Hiragana hiraganaScript = hit.collider.gameObject.GetComponent<Hiragana>();
                if (hiraganaScript != null && !hiraganaScript.isSelect()) // ���ɑI������Ă��Ȃ����ǂ������m�F
                {
                    hiraganaScript.OnSelected();
                    //gameManager.DeleteHiraInQueue(hiraganaScript);
                    gameManager.OnSPHiraganaSelected(hiraganaScript);
                }
            }
        }*/
    }
}
