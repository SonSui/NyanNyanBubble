using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hiragana : MonoBehaviour
{
    public string hiragana; // 平仮名文字
    public Vector3 originalPosition; // 元の位置
    public Vector3 originalSize = new Vector3(1, 1, 1); // 元のサイズ
    private Vector3 worldScale;
    public bool isHint = false;
    private Color oriCol;
    private const float defOffsetX = -9.0f;
    private const float defEndX = 9.0f;
    
    private Vector3 posBeforeMove;


    public SpriteRenderer spriteRenderer; // SpriteRendererの参照
    private Collider2D collider2D; // Collider2Dの参照

    public struct MouseAct
    {
        public float magnificat;// 拡大率
        public float rotAngles;// 回転角度
        public int mouseOverType;// マウスオーバーアクションの種類
        public Vector3 maxSize; // 最大サイズ
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
        public int actType;// アクションの種類
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

    private bool isSelected = false; // 選択されたかどうか
    private GameManager gameManager; // GameManagerへの参照
    private bool onMouseAct = false; // アクション中かどうか
    private bool onDef = true;
    private bool onMove = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRendererを取得
        collider2D = GetComponent<Collider2D>(); // Collider2Dを取得
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManagerを見つける
        transform.localScale = originalSize; // 元のサイズを保存
        worldScale = transform.lossyScale;
        originalPosition = transform.localPosition; // 元の位置を保存
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

        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask; // デフォルトではマスク内で可視
    }

    void OnDestroy()
    {
       
    }

    void Update()
    {
        if (onMouseAct) // アクション中の場合
        {
            MouseOverActType(); // マウスオーバーアクションを実行
        }
        else if (onDef)
        {
            DefaultAct(); // デフォルトアクションを実行
        }

        UpdateMaskInteraction(); // マスクとコライダーの状態を更新
    }

    private void UpdateMaskInteraction()
    {
        if (isHint || isSelected || onMove)
        {
            // OnHint、OnSelected、移動中の場合、マスクの影響を受けない
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            // コライダーを有効化
            collider2D.enabled = true; // コライダーを有効にする
        }
        else
        {
            // デフォルトではマスク内で可視
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            // スプライトが表示されているか確認
            if (spriteRenderer.isVisible)
            {
                collider2D.enabled = true; // コライダーを有効にする
            }
            else
            {
                collider2D.enabled = false; // コライダーを無効にする
            }
        }
    }

    public bool isSelect()
    {
        return isSelected; // 選択されているかどうかを返す
    }

    public void OnSelected()
    {
        isSelected = true; // 選択状態にする
        onMouseAct = true; // アクションを開始
        onDef = false;
        isHint = false;
        onMove = true;

        // マスクの影響を受けない
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // コライダーを有効にする
    }

    public void OnHint()
    {
        isHint = true;

        // マスクの影響を受けない
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // コライダーを有効にする
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

        // マスク内で可視
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        // スプライトが表示されているか確認
        if (spriteRenderer.isVisible)
        {
            collider2D.enabled = true; // コライダーを有効にする
        }
        else
        {
            collider2D.enabled = false; // コライダーを無効にする
        }
    }

    public void NoSelectOption()
    {
        isSelected = true;
        onMouseAct = false;
        onDef = false;

        // マスクの影響を受けない
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // コライダーを有効にする
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
            this.transform.localScale *= 1.01f; // 拡大

        if (mouse.rotDirection)
        {
            this.transform.Rotate(0, 0, 0.02f); // 回転
            if (mouse.rotAngles <= this.transform.rotation.z)
            {
                mouse.rotDirection = false; // 回転方向を反転
            }
        }
        else
        {
            this.transform.Rotate(0, 0, -0.02f); // 逆回転
            if (-mouse.rotAngles >= this.transform.rotation.z)
            {
                mouse.rotDirection = true; // 回転方向を反転
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
            GetComponent<SpriteRenderer>().color = Color.yellow; // マウスオーバー時に色を変更
        }
        if (isSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = oriCol; // マウスが離れた時に色を元に戻す
    }

    public void MoveTo(Vector3 targetPosition)
    {
        isSelected = true; // 選択状態にする
        onMouseAct = false; // アクションを停止
        onMove = true;

        // マスクの影響を受けない
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // コライダーを有効にする

        StartCoroutine(MoveToPosition(targetPosition)); // 指定位置に移動
    }

    public void MoveToRound(Vector3 targetPosition)
    {
        isSelected = true; // 選択状態にする
        onMouseAct = false;
        onMove = true;

        // マスクの影響を受けない
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        collider2D.enabled = true; // コライダーを有効にする
        posBeforeMove = transform.position;

        StartCoroutine(MoveToRoundTrip(targetPosition)); // 指定位置に移動し、元の位置に戻る
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // 現在位置を保存
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // 線形補間で移動
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // 最終位置に設定
        transform.localScale = originalSize;
        onDef = false;
        onMove = false;
        isSelected = true;

        // MoveTo移動完了後もコライダーを有効にする
    }

    private System.Collections.IEnumerator MoveToRoundTrip(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = transform.position; // 現在位置を保存
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time * 2); // 線形補間で移動
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // 一時的な最終位置に設定
        startPosition = transform.position; // 一時的な位置を保存
        time = 0;
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(startPosition, posBeforeMove, time * 2); // 元の位置に戻る
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = posBeforeMove; // 最終的に元の位置に設定
        transform.localScale = originalSize;
        isSelected = false; // 選択状態を解除
        onMouseAct = false; // アクションを停止
        onDef = true;
        onMove = false;

        // MoveToRoundTrip移動完了後はマスク内で可視

        // スプライトが表示されているか確認
        if (spriteRenderer.isVisible)
        {
            collider2D.enabled = true; // コライダーを有効にする
        }
        else
        {
            collider2D.enabled = false; // コライダーを無効にする
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
