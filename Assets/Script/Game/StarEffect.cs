using UnityEngine;
using UnityEngine.UI;

public class StarEffect : MonoBehaviour
{
    public Sprite starSprite; // 星の画像
    public RectTransform parentTransform; // 親UIオブジェクト
    public int starCount = 15; // 星の数
    public float spreadRadius = 300f; // 星が広がる半径
    private float lifetime = 1.0f; // 星の寿命（秒）
    public Color[] starColors; // 星の色リスト（ランダムで選択）
    public float rotationSpeed = 360f; // 星の回転速度（度/秒）
    public Vector2 sizeRange = new Vector2(15f, 60f); // 星のサイズ範囲

    public void PlayEffect(Vector2 uiPosition)
    {
        if (starSprite == null || parentTransform == null)
        {
            Debug.LogError("Spriteまたは親オブジェクトが設定されていません！");
            return;
        }

        for (int i = 0; i < starCount; i++)
        {
            // 新しいUIオブジェクトを作成
            GameObject star = new GameObject("Star", typeof(RectTransform), typeof(Image));
            RectTransform rectTransform = star.GetComponent<RectTransform>();
            Image image = star.GetComponent<Image>();

            // 親オブジェクトを設定し、画像コンポーネントを追加
            star.transform.SetParent(parentTransform, false);
            image.sprite = starSprite; // 画像を設定
            image.color = Color.white; // 初期カラーを白に設定

            // ランダムなサイズを設定
            float randomSize = Random.Range(sizeRange.x, sizeRange.y);
            rectTransform.sizeDelta = new Vector2(randomSize, randomSize);

            // 初期位置を設定（指定されたUI位置）
            rectTransform.anchoredPosition = uiPosition;

            // ランダムな色を設定
            if (starColors != null && starColors.Length > 0)
            {
                Color randomColor = starColors[Random.Range(0, starColors.Length)];
                randomColor.a = 0.8f; // 透明度を設定
                image.color = randomColor;
            }

            // ランダムな移動方向と速度を設定
            Vector2 randomDirection = Random.insideUnitCircle.normalized; // 単位ベクトルで方向を設定
            float randomSpeed = Random.Range(0.5f, spreadRadius); // ランダムな速度を設定

            // 移動と回転スクリプトを追加
            StarMovementUI movement = star.AddComponent<StarMovementUI>();
            movement.SetParameters(randomDirection, randomSpeed, rotationSpeed);

            // 一定時間後に星を削除
            Destroy(star, lifetime);
        }
    }

}