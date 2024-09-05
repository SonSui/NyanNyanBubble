using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBGM : MonoBehaviour
{
    private AudioSource audioSource;
    string pre = "1";
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        pre = "GameScene";
    }
    // Update is called once per frame
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName =="Title"|| sceneName == "GameOver")
        {
            if(audioSource.isPlaying==false)audioSource.Play();
        }
        if(sceneName=="GameScene")
        {
            audioSource.Stop();
        }
        pre = sceneName;
    }
}
