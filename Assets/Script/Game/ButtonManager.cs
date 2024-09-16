using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Script : MonoBehaviour
{
    Scene scene;
    public GameManager gameManager;
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }
    public void buttonReturn_Down()
    {
        SceneManager.LoadScene("Title");
    }
    public void buttonReset_Down()
    {
        SceneManager.LoadScene(scene.name);
    }
    public void buttonStart_Down()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void buttonRefresh_Down()
    {
        if (gameManager.isGameOver == false)
        {
            gameManager.ResetHiraAndGroup(true);
        }
    }
    public void buttonExitGame_Down()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        
            Application.Quit();
        #endif
    }
    public void buttonRanking_Down()
    {
        SceneManager.LoadScene("Ranking");
    }
}
