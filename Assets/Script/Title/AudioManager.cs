using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] backgroundMusicClips;
    public AudioClip[] soundEffects;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        // 存在しているかを確認
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // 循環設定
        musicSource.loop = true;
        SetMusicVolume(0.15f);
    }

    public void PlayMusicForCurrentScene()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Title":
                PlayMusic(backgroundMusicClips[0]); 
                break;
            case "GameScene":
                PlayMusic(backgroundMusicClips[1]); 
                break;
            case "GameOver":
                PlayMusic(backgroundMusicClips[0]); 
                break;
            default:
                Debug.LogWarning("No music assigned for this scene.");
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return; 
        }

        
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySoundEffect(int index)
    {
        if (index < soundEffects.Length)
        {
            sfxSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("Sound effect index out of range.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
}