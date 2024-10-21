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
    private const float defOffsetX = -9.0f;
    private const float defEndX = 9.0f;
    
    private Vector3 posBeforeMove;


    public SpriteRenderer spriteRenderer; // SpriteRenderer�̎Q��
    private Collider2D collider2D; // Collider2D�̎Q��

    public struct MouseAct
    {
        public float magnificat;// �g�嗦
        public float rotAngles;// ��]�p�x
        public int mouseOverType;// �}�E�X�I�[�o�[�A�N�V�����̎��
        public Vector3 maxSize; // �ő�T�C�Y
        public bool rotDirection;

        public MouseAct(float magnificat, float rotAngles, int mouseOverType = 0)
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
        public float offsetX;
        public float offsetSpeed;

        public float oriSpeed;
        public DefaultAction(int actType = 0, float timeCnt = 0, float radius = 1.0f, float speed = 2.0f, float offsetx = 5.0f,float offsetS=0.8f)
        {
            this.actType = actType;
            this.timeCnt = timeCnt;
            this.radius = radius;
            this.speed = speed;
            this.oriSpeed = speed;
            this.rotDirection = false;
            this.moveDirection = 0;
            this.rotAngles = 0;
            this.offsetX = offsetx;
            this.offsetSpeed=offsetS;
        }
    };
    public DefaultAction defAct;

    private bool isSelected = false; // �I�����ꂽ���ǂ���
    private GameManager gameManager; // GameManager�ւ̎Q��
    private bool onMouseAct = false; // �A�N�V���������ǂ���
    private bool onDef = true;
    private bool onMove = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer���擾
        collider2D = GetComponent<Collider2D>(); // Collider2D���擾
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager��������
        transform.localScale = originalSize; // ���̃T�C�Y��ۑ�
        worldScale = transform.lossyScale;
        originalPosition = transform.localPosition; // ���̈ʒu��ۑ�
        mouse = new MouseAct(2.2f, 0.1f);
        float rx = math.min(math.abs(-5.5f - transform.localScale.x), math.abs(2.0f - transform.localScale.x));
        float ry = math.min(math.abs(-1.5f - transform.localScale.y), math.abs(1.5f - transform.localScale.y));
        float r = math.min(rx, ry);
        float l = math.min(30, r);
        defAct = new DefaultAction(
            UnityEngine.Random.Range(1, 3),
            UnityEngine.Random.Range(0, 90),
            UnityEngine.Random.Range(l, r),
            UnityEngine.Random.Range(1.0f, 3.0f),
            UnityEngine.Random.Range(-9f, 9f),
            UnityEngine.Random.Range(0.3f,1.2f)
            );

        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask; // �f�t�H���g�ł̓}�X�N���ŉ�
    }

    void OnDestroy()
    {
       
    }

    void Update()
    {
        if (onMouseAct) // �A�N�V�������̏ꍇ
        {
            MouseOverActType(); // �}�E�X�I�[�o�[�A�N�V���������s
        }
        else if (onDef)
        {
            DefaultAct(); // �f�t�H���g�A�N�V���������s
        }

        UpdateMaskInteraction(); // �}�X�N�ƃR���C�_�[�̏�Ԃ��X�V
    }

    private void UpdateMaskInteraction()
    {
        if (isHint || isSelected || onMove)
        {
            // OnHint�AOnSelected�A�ړ����̏ꍇ�A�}�X�N�̉e�����󂯂Ȃ�
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            // �R���C�_�[��L����
            collider2D.enabled = true; // �R���C�_�[��L���ɂ���
        }
        else
        {
            // �f�t�H���g�ł̓}�X�N���ŉ�
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            // �X�v���C�g���\������Ă��邩�m�F
            if (spriteRenderer.isVisible)
            {
                collider2D.enabled = true; // �R���C�_�[��L���ɂ���
            }
            else
            {
                collider2D.enabled = false; // �R���C�_�[�𖳌��ɂ���
            }
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

        // �}�X�N�̉e�����󂯂Ȃ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // �R���C�_�[��L���ɂ���
    }

    public void OnHint()
    {
        isHint = true;

        // �}�X�N�̉e�����󂯂Ȃ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // �R���C�_�[��L���ɂ���
    }

    public bool IsHint()
    {
        return isHint;
    }

    public void CancelSelect()
    {
        isSelected = false;
        onMouseAct = false;
        onDef = true;
        onMove = false;
        transform.localScale = originalSize;

        // �}�X�N���ŉ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        // �X�v���C�g���\������Ă��邩�m�F
        if (spriteRenderer.isVisible)
        {
            collider2D.enabled = true; // �R���C�_�[��L���ɂ���
        }
        else
        {
            collider2D.enabled = false; // �R���C�_�[�𖳌��ɂ���
        }
    }

    public void NoSelectOption()
    {
        isSelected = true;
        onMouseAct = false;
        onDef = false;

        // �}�X�N�̉e�����󂯂Ȃ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // �R���C�_�[��L���ɂ���
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
        defAct.offsetX += defAct.offsetSpeed * Time.deltaTime;
        if (defAct.offsetX > defEndX)
        {
            defAct.offsetX = defOffsetX;
        }
        if (defAct.actType == 1)
        {
            Vector3 center = originalPosition;
            defAct.timeCnt += Time.deltaTime * defAct.speed;
            float x = Mathf.Cos(defAct.timeCnt) * defAct.radius + defAct.offsetX;
            float y = 0;
            transform.position = center + new Vector3(x, y, 0);
        }
        if (defAct.actType == 2)
        {
            Vector3 center = originalPosition;
            defAct.timeCnt += Time.deltaTime * defAct.speed;
            float x = defAct.offsetX;
            float y = Mathf.Sin(defAct.timeCnt) * defAct.radius;
            transform.position = center + new Vector3(x, y, 0);
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
        if (isHint) return;
        if (!isSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow; // �}�E�X�I�[�o�[���ɐF��ύX
        }
        if (isSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = oriCol; // �}�E�X�����ꂽ���ɐF�����ɖ߂�
    }

    public void MoveTo(Vector3 targetPosition)
    {
        isSelected = true; // �I����Ԃɂ���
        onMouseAct = false; // �A�N�V�������~
        onMove = true;

        // �}�X�N�̉e�����󂯂Ȃ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // �R���C�_�[��L���ɂ���

        StartCoroutine(MoveToPosition(targetPosition)); // �w��ʒu�Ɉړ�
    }

    public void MoveToRound(Vector3 targetPosition)
    {
        isSelected = true; // �I����Ԃɂ���
        onMouseAct = false;
        onMove = true;

        // �}�X�N�̉e�����󂯂Ȃ�
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // �R���C�_�[��L���ɂ���
        posBeforeMove = transform.position;

        StartCoroutine(MoveToRoundTrip(targetPosition)); // �w��ʒu�Ɉړ����A���̈ʒu�ɖ߂�
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // ���݈ʒu��ۑ�
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // ���`��Ԃňړ�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // �ŏI�ʒu�ɐݒ�
        transform.localScale = originalSize;
        onDef = false;
        onMove = false;
        isSelected = true;

        // MoveTo�ړ���������R���C�_�[��L���ɂ���
    }

    private System.Collections.IEnumerator MoveToRoundTrip(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // ���݈ʒu��ۑ�
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // ���`��Ԃňړ�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // �ꎞ�I�ȍŏI�ʒu�ɐݒ�
        startPosition = transform.position; // �ꎞ�I�Ȉʒu��ۑ�
        time = 0;
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, posBeforeMove, time * 2); // ���̈ʒu�ɖ߂�
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = posBeforeMove; // �ŏI�I�Ɍ��̈ʒu�ɐݒ�
        transform.localScale = originalSize;
        isSelected = false; // �I����Ԃ�����
        onMouseAct = false; // �A�N�V�������~
        onDef = true;
        onMove = false;

        // MoveToRoundTrip�ړ�������̓}�X�N���ŉ�

        // �X�v���C�g���\������Ă��邩�m�F
        if (spriteRenderer.isVisible)
        {
            collider2D.enabled = true; // �R���C�_�[��L���ɂ���
        }
        else
        {
            collider2D.enabled = false; // �R���C�_�[�𖳌��ɂ���
        }
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
