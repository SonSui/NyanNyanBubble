using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo_Scene : MonoBehaviour
{
    public float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Debug.Log("Logo_Scene.cs:Displayed Logo for 3 seconds change scene.");
            ChangeScene();
        }
        if (Input.anyKeyDown)
        {
            Debug.Log("Title_Scene.cs:KeyDown,Load Title");
            ChangeScene();
        }
    }
    void ChangeScene()
    {
        SceneManager.LoadScene("Title");
    }
}
