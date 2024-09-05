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
        gameManager.ResetHiraAndGroup(true);
    }
}