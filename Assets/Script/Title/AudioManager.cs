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
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip clipToPlay = null;

        switch (sceneName)
        {
            case "Title":
                clipToPlay = backgroundMusicClips[0];
                break;
            case "Logo":
                clipToPlay = backgroundMusicClips[0];
                break;
            case "GameScene":
                clipToPlay = backgroundMusicClips[1];
                break;
            case "GameOver":
                clipToPlay = backgroundMusicClips[0];
                break;
            case "Ranking":
                clipToPlay = backgroundMusicClips[0];
                break;
            default:
                Debug.LogWarning("No music assigned for this scene.");
                break;
        }

        
        if (musicSource.clip != clipToPlay)
        {
            PlayMusic(clipToPlay);
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

        if (clip != null)
        {
            Debug.Log("Now playing: " + clip.name);
        }
        else
        {
            Debug.Log("No audio clip is set.");
        }
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