using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    #region Public Fields

    public AudioClip[] musicLevel;
    public float defaultMusicVolume = 0.25f;
    public float defaultSFXVolume = 0.5f;

    #endregion Public Fields

    #region Private Fields

    private AudioClip audioClip;
    private AudioClip lastAudioClip;
    private AudioSource audioSrc;
    private bool musicMuted;
    private bool sFXMuted;
    private Dictionary<string, AudioSource> SFXAudioSources;

    #endregion Private Fields

    #region Public Properties

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

    #endregion Public Properties

    #region Public Methods

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

    #endregion Public Methods

    #region Protected Methods

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

    #endregion Protected Methods

    #region Private Methods

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupMusic(scene.buildIndex);
        Debug.Log("Level Loaded: " + scene.name);
    }

    private void SetupMusic(int buildIndex)
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

    private void SetupSFX()
    {
        SFXAudioSources = new Dictionary<string, AudioSource>();

        AudioSource[] sfxAudioSources = gameObject.transform.GetChild(0).GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            SFXAudioSources[sfxAudioSources[i].gameObject.name] = sfxAudioSources[i];
        }
    }

    private void Start()
    {
        SetupSFX();

        SetMusicVolume(PlayerPrefsManager.GetMusicVolume());
        SetSFXVolume(PlayerPrefsManager.GetSFXVolume());
    }

    #endregion Private Methods
}