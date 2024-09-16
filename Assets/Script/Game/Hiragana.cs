using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hiragana : MonoBehaviour
{
    public string hiragana; // ����������
    public Vector3 originalPosition; // ���̈ʒu
    public Vector3 originalSize = new Vector3(1, 1, 1); // ���̃T�C�Y
    private Vector3 worldScale;
    public bool isHint = false;
    private Color oriCol;
    public struct MouseAct
    {
        public float magnificat;// �g�嗦
        public float rotAngles;// ��]�p�x
        public int mouseOverType;// �}�E�X�I�[�o�[�A�N�V�����̎��
        public Vector3 maxSize; // �ő�T�C�Y
        public bool rotDirection;

        public MouseAct(float magnificat, float rotAngles, int mouseOverType=0)
        {
            this.magnificat = magnificat;
            this.rotAngles = rotAngles;
            this.mouseOverType = mouseOverType;
            this.rotDirection = false;
            this.maxSize = new Vector3(1.2f, 1.2f, 1.2f);
        }
    };
    public MouseAct mouse;
    public struct DefaultAction
    {
        public int actType;// �A�N�V�����̎��
        public float rotAngles;
        public float timeCnt;
        public float radius;
        public float speed;

        public bool rotDirection;
        public int moveDirection;

        public float oriSpeed;
        public DefaultAction(int actType = 0, float timeCnt = 0, float radius = 1.0f, float speed = 2.0f)
        {
            this.actType = actType;
            this.timeCnt = timeCnt;
            this.radius = radius;
            this.speed = speed;
            this.oriSpeed = speed;
            this.rotDirection = false;
            this.moveDirection = 0;
            this.rotAngles = 0;
        }
    };
    public DefaultAction defAct;

    private bool isSelected = false; // �I�����ꂽ���ǂ���
    private GameManager gameManager; // GameManager�ւ̎Q��
    private bool onMouseAct = false; // �A�N�V���������ǂ���
    private bool onDef = true;
    private bool onMove = false;
    

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager��������
        transform.localScale = originalSize; // ���̃T�C�Y��ۑ�
        worldScale = transform.lossyScale;
        originalPosition = transform.localPosition; // ���̈ʒu��ۑ�
        mouse = new MouseAct(2.2f,0.1f);
        float rx = math.min(math.abs(-5.5f - transform.localScale.x), math.abs(2.0f - transform.localScale.x));
        float ry =math.min(math.abs(-1.5f-transform.localScale.y), math.abs(1.5f-transform.localScale.y));
        float r = math.min(rx,ry);
        float l = math.min(30,r);
        defAct = new DefaultAction(UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(0, 90), UnityEngine.Random.Range(l, r), UnityEngine.Random.Range(1.0f, 3.0f));
        
    }
    void OnDestroy()
    {
        int x = (int)originalPosition.x;
        int y = (int)originalPosition.y;
        if (x + 5 > 7 || x + 5 < 0) x = 0;
        if (y + 1 > 3 || y + 1 < 0) y = 0;
        gameManager.wordManager.hiraTable[x+5, y+1] = false;
    }

    void Update()
    {
        if (onMouseAct) // �A�N�V�������̏ꍇ
        {
            MouseOverActType(); // �}�E�X�I�[�o�[�A�N�V�����^�C�v0�����s
        }
        else if (onDef)
        {
            DefaultAct();
        }
    }
    public bool isSelect()
    {
        return isSelected; // �I������Ă��邩�ǂ�����Ԃ�
    }

    public void OnSelected()
    {
        isSelected = true; // �I����Ԃɂ���
        onMouseAct = true; // �A�N�V�������J�n
        onDef = false;
        isHint = false;
        onMove = true;
    }
    public void CancelSelect()
    {
        isSelected = false; 
        onMouseAct = false; 
        onDef = true;
        onMove = false;
        transform.localScale = originalSize;
    }
    public void NoSelectOption()
    {
        isSelected = true;
        onMouseAct = false;
        onDef = false;
    }
    public void NoMovingOption()
    {
        onDef = false;
    }
    public bool isMoving()
    {
        return onMove;
    }
    private void DefaultAct()
    {
        if (defAct.actType == 1)
        {
            Vector3 center = originalPosition;
            defAct.timeCnt += Time.deltaTime * defAct.speed;
            float x = Mathf.Cos(defAct.timeCnt) * defAct.radius;
            float y = 0;
            transform.position = center + new Vector3(x, y, 0);
            // Debug.Log($"{x},{y},{defAct.timeCnt},{defAct.radius},{defAct.speed}");
        }
        if (defAct.actType == 2)
        {
            Vector3 center = originalPosition ;
            defAct.timeCnt += Time.deltaTime * defAct.speed;
            float x = 0;
            float y = Mathf.Sin(defAct.timeCnt) * defAct.radius;
            transform.position = center + new Vector3(x, y, 0);
            // Debug.Log($"{x},{y},{defAct.timeCnt},{defAct.radius},{defAct.speed}");
        }
    }
    private void MouseOverActType()
    {
        if (this.transform.localScale.x < mouse.maxSize.x)
            this.transform.localScale *= 1.01f; // �g��
        if (mouse.rotDirection)
        {
            this.transform.Rotate(0, 0, 0.02f); // ��]
            if (mouse.rotAngles <= this.transform.rotation.z)
            {
                mouse.rotDirection = false; // ��]�����𔽓]
            }
        }
        else
        {
            this.transform.Rotate(0, 0, -0.02f); // �t��]
            if (-mouse.rotAngles >= this.transform.rotation.z)
            {
                mouse.rotDirection = true; // ��]�����𔽓]
            }
        }
    }
    public void WarningMode()
    {
        StartCoroutine(Warning());

    }
    private System.Collections.IEnumerator Warning()
    {
        Color c = Color.white;
        c.a = GetComponent<SpriteRenderer>().color.a;
        float time = 0;
        while (time < 0.5)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = c;
    }

    void OnMouseEnter()
    {
        oriCol = GetComponent<SpriteRenderer>().color;
        if (!isSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow; // �}�E�X�I�[�o�[���ɐF��ύX
        }
        if(isSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }
    void OnMouseExit()
    {
        /*Color c = Color.white;
        c.a = GetComponent<SpriteRenderer>().color.a;*/
        GetComponent<SpriteRenderer>().color = oriCol; // �}�E�X�����ꂽ���ɐF�����ɖ߂�
    }


    public void MoveTo(Vector3 targetPosition)
    {
        isSelected = true; // �I����Ԃɂ���
        onMouseAct = false; // �A�N�V�������~
        onMove = true;
        StartCoroutine(MoveToPosition(targetPosition)); // �w��ʒu�Ɉړ�
    }

    public void MoveToRound(Vector3 targetPosition)
    {
        isSelected = true; // �I����Ԃɂ���
        onMouseAct = false;
        onMove = true;
        StartCoroutine(MoveToRoundTrip(targetPosition)); // �w��ʒu�Ɉړ����A���̈ʒu�ɖ߂�
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // ���݈ʒu��ۑ�
        while (time < 0.5)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // ���`��Ԃňړ�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // �ŏI�ʒu�ɐݒ�
        transform.localScale = originalSize;
        onDef = false;
        onMove = false;
        isSelected = false;
    }

    private System.Collections.IEnumerator MoveToRoundTrip(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // ���݈ʒu��ۑ�
        while (time < 0.5)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // ���`��Ԃňړ�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // �ꎞ�I�ȍŏI�ʒu�ɐݒ�
        startPosition = transform.position; // �ꎞ�I�Ȉʒu��ۑ�
        time = 0;
        while (time < 0.5)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, time * 2); // ���̈ʒu�ɖ߂�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // �ŏI�I�Ɍ��̈ʒu�ɐݒ�
        transform.localScale = originalSize;
        isSelected = false; // �I����Ԃ�����
        onMouseAct = false; // �A�N�V�������~
        onDef = true;
        onMove = false;
    }
    public void SetOriginalScale()
    {
        Vector3 newWorldScale = transform.lossyScale;
        transform.localScale = new Vector3(
            originalSize.x * (worldScale.x / newWorldScale.x),
            originalSize.y * (worldScale.y / newWorldScale.y),
            originalSize.z * (worldScale.z / newWorldScale.z)
        );
    }
}
