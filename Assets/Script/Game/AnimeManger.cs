using UnityEngine;
using UnityEngine.UI;

public class AnimeManager : MonoBehaviour
{
    public Image scoreImage;
    public Animator scoreImageAnimator;

    public void ShowScore(Vector3 position, int score)
    {
        
        scoreImage.transform.position = Camera.main.WorldToScreenPoint(position);
        scoreImageAnimator.SetTrigger("Show");
    }
}
