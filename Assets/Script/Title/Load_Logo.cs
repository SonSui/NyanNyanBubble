using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_Scene : MonoBehaviour
{
    private float timer = 0;
    public float waitTime = 5;
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKeyDown)
        {
            Debug.Log("Title_Scene.cs:KeyDown!Reset LoadScece Countdown");
            timer = 0;
        }
        if (timer > waitTime)
        {
            Debug.Log("Title_Scene.cs:No action! change scene to Logo");
            ChangeScene();
        }
    }
    void ChangeScene() => SceneManager.LoadScene("Logo");
}
