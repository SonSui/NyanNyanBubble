using UnityEngine;

public class StarMovementUI : MonoBehaviour
{
    private Vector2 direction; // 移動方向
    private float speed; // 移動速度
    private float rotationSpeed; // 回転速度

    public void SetParameters(Vector2 moveDirection, float moveSpeed, float rotateSpeed)
    {
        direction = moveDirection; // 移動方向を設定
        speed = moveSpeed; // 移動速度を設定
        rotationSpeed = rotateSpeed; // 回転速度を設定
    }

    void Update()
    {
        // UIオブジェクトのRectTransformを取得
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 星を移動させる（移動速度を考慮）
            rectTransform.anchoredPosition += direction * speed * Time.deltaTime;

            // 星を回転させる
            rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}