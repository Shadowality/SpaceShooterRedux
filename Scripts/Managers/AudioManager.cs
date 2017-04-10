using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// DONE
public class AudioManager : Singleton<AudioManager>
{
    #region PUBLIC VARIABLES
    //TODO
    public bool MusicMuted
    {
        get
        {
            return musicMuted;
        }

        set
        {
            musicMuted = value;
        }
    }
    public bool SFXMuted
    {
        get
        {
            return sFXMuted;
        }

        set
        {
            sFXMuted = value;
        }
    }

    public AudioClip[] musicLevel;
    #endregion

    #region PRIVATE VARIABLES
    private Dictionary<string, AudioSource> SFXAudioSources;

    public float defaultMusicVolume = 0.25f;
    public float defaultSFXVolume = 0.5f;

    private bool musicMuted;
    private bool sFXMuted;

    private AudioSource audioSrc;
    private AudioClip audioClip;
    private AudioClip lastAudioClip;
    #endregion

    #region METHODS

    protected override void Awake()
    {
        base.Awake();

        // If no playerprefs defined.
        if (!PlayerPrefsManager.HasKey())
        {
            PlayerPrefsManager.SetDefaultMusicVolume(defaultMusicVolume);
            PlayerPrefsManager.SetDefaultSFXVolume(defaultSFXVolume);
            PlayerPrefsManager.SetMusicVolume(defaultMusicVolume);
            PlayerPrefsManager.SetSFXVolume(defaultSFXVolume);
        }

        audioSrc = GetComponent<AudioSource>();
        audioSrc.ignoreListenerVolume = true;
    }

    void Start()
    {
        SetupSFX();

        SetMusicVolume(PlayerPrefsManager.GetMusicVolume());
        SetSFXVolume(PlayerPrefsManager.GetSFXVolume());
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void SetupMusic(int buildIndex)
    {
        audioClip = musicLevel[buildIndex];

        // Don't restart music if it's the same.
        if (audioClip != lastAudioClip)
        {
            if (audioClip)
            {
                lastAudioClip = audioClip;
                audioSrc.clip = audioClip;
                audioSrc.Play();
            }
        }
    }

    void SetupSFX()
    {
        SFXAudioSources = new Dictionary<string, AudioSource>();

        AudioSource[] sfxAudioSources = gameObject.transform.GetChild(0).GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            SFXAudioSources[sfxAudioSources[i].gameObject.name] = sfxAudioSources[i];
        }
    }

    public AudioSource GetSFXAudioSource(string name)
    {
        return SFXAudioSources[name];
    }

    public void SetMusicVolume(float volume)
    {
        if (!MusicMuted)
            audioSrc.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        if (!SFXMuted)
            AudioListener.volume = volume;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupMusic(scene.buildIndex);
        Debug.Log("Level Loaded: " + scene.name);

    }
    #endregion

}
