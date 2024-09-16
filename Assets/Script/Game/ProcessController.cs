using UnityEngine;
using UnityEngine.UI;

public class ProcessController : MonoBehaviour
{
    public Image progressBarFill;
    public float totalTime = 99f;
    public float currentTime = 99f;
    private float elapsedTime = 0f;
    public Image movingImage;
    void Update()
    {
        elapsedTime = totalTime - currentTime;
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(1-elapsedTime / totalTime);
            progressBarFill.fillAmount = fillAmount;

            RectTransform progressBarRect = progressBarFill.GetComponent<RectTransform>();
            RectTransform movingImageRect = movingImage.GetComponent<RectTransform>();
            float newX = Mathf.Lerp(progressBarRect.rect.xMax, progressBarRect.rect.xMin, fillAmount);
            movingImageRect.anchoredPosition = new Vector2(newX-5, movingImageRect.anchoredPosition.y);
        }
    }
}