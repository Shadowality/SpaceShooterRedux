using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{

    private static Slider musicSlider;
    private static Slider sfxSlider;

    // Use this for initialization
    void Awake()
    {
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();

        musicSlider.value = PlayerPrefsManager.GetMusicVolume();
        sfxSlider.value = PlayerPrefsManager.GetSFXVolume();
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        if (!AudioManager.Instance.GetSFXAudioSource("ShieldOn").isPlaying)
            AudioManager.Instance.GetSFXAudioSource("ShieldOn").Play();
    }

    public void SetDefaults()
    {
        musicSlider.value = PlayerPrefsManager.GetDefaultMusicVolume();
        sfxSlider.value = PlayerPrefsManager.GetDefaultSFXVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SaveAndExit()
    {
        PlayerPrefsManager.SetMusicVolume(musicSlider.value);
        PlayerPrefsManager.SetSFXVolume(sfxSlider.value);
    }

}
