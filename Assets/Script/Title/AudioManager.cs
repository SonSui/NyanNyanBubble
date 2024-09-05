using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static AudioManager instance;

    public AudioClip[] backgroundMusicClips;
    public AudioClip[] soundEffects;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        //存在しているかを確認
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

        //循環設定
        musicSource.loop = true;
        SetMusicVolume(0.15f);
    }

   /* private void Start()
    {
        PlayMusicForCurrentScene();
    }*/

    public void PlayMusicForCurrentScene()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Title":
                StopMusic(backgroundMusicClips[1]);
                PlayMusic(backgroundMusicClips[0]);
                break;
            case "GameScene":
                StopMusic(backgroundMusicClips[0]);
                PlayMusic(backgroundMusicClips[1]);

                break;
            case "GameOver":
                StopMusic(backgroundMusicClips[1]);
                PlayMusic(backgroundMusicClips[0]);
                break;
            default:
                Debug.LogWarning("No music assigned for this scene.");
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    public void StopMusic(AudioClip clip)
    {
        /*if (musicSource.isPlaying)
        { musicSource.Stop(); }*/
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Stop();
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