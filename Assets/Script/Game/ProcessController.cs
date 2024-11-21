using UnityEngine;
using UnityEngine.UI;

public class ProcessController : MonoBehaviour
{
    public Image progressBarFill;
    public float totalTime = 99f;
    public float currentTime = 99f;
    private float elapsedTime = 0f;
    public Image movingImage;
    public StarEffect starEffect;
    void Update()
    {
        elapsedTime = totalTime - currentTime;
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(1 - elapsedTime / totalTime);
            progressBarFill.fillAmount = fillAmount;

            RectTransform progressBarRect = progressBarFill.GetComponent<RectTransform>();
            RectTransform movingImageRect = movingImage.GetComponent<RectTransform>();
            float newX = Mathf.Lerp(progressBarRect.rect.xMax, progressBarRect.rect.xMin, fillAmount);
            movingImageRect.anchoredPosition = new Vector2(newX - 5, movingImageRect.anchoredPosition.y);
        }
    }
    public void AddTimeEffect()
    {
        elapsedTime = totalTime - currentTime;
        float fillAmount = Mathf.Clamp01(1 - elapsedTime / totalTime);
        progressBarFill.fillAmount = fillAmount;

        RectTransform progressBarRect = progressBarFill.GetComponent<RectTransform>();
        RectTransform movingImageRect = movingImage.GetComponent<RectTransform>();
        float newX = Mathf.Lerp(progressBarRect.rect.xMax, progressBarRect.rect.xMin, fillAmount);
        Vector2 newPos= new Vector2(newX - 5-180, movingImageRect.anchoredPosition.y+460);
        movingImageRect.anchoredPosition = new Vector2(newX - 5, movingImageRect.anchoredPosition.y);
        starEffect.PlayEffect(newPos);
    }

}