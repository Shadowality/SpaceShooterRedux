using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour
{
    #region Private Fields

    private const string DEFAULT_MUSIC_VOLUME_KEY = "default_music_volume";
    private const string DEFAULT_SFX_VOLUME_KEY = "default_sfx_volume";
    private const string DIFFICULTY_KEY = "difficulty";
    private const string HIGH_SCORE_KEY = "high_score";
    private const string LEVEL_KEY = "level_unlocked_";
    private const string MUSIC_VOLUME_KEY = "master_volume";
    private const string SFX_VOLUME_KEY = "sfx_volume";

    #endregion Private Fields

    #region Public Methods

    public static float GetDefaultMusicVolume()
    {
        return PlayerPrefs.GetFloat(DEFAULT_MUSIC_VOLUME_KEY);
    }

    public static float GetDefaultSFXVolume()
    {
        return PlayerPrefs.GetFloat(DEFAULT_SFX_VOLUME_KEY);
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
    }

    public static bool HasKey()
    {
        return PlayerPrefs.HasKey(MUSIC_VOLUME_KEY);
    }

    public static bool IsLevelUnlocked(int level)
    {
        // Obtain corresponding value to the key.
        int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
        bool isLevelUnlocked = (levelValue == 1);

        if (level > SceneManager.sceneCountInBuildSettings - 1)
            return isLevelUnlocked;
        else
        {
            Debug.Log("Level not in build");
            return false;
        }
    }

    public static void SetDefaultMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        PlayerPrefs.SetFloat(DEFAULT_MUSIC_VOLUME_KEY, volume);
    }

    public static void SetDefaultSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        PlayerPrefs.SetFloat(DEFAULT_SFX_VOLUME_KEY, volume);
    }

    public static void SetDifficulty(float difficulty)
    {
        if (difficulty >= 1 && difficulty <= 3)
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
        else
            Debug.Log("Difficulty out of range");
    }

    public static void SetHighScore(int score)
    {
        if (score >= 0)
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
        else
            Debug.Log("Score value must be positive");
    }

    public static void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public static void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    // TODO
    public static void UnlockLevel(int level)
    {
        if (level > SceneManager.sceneCountInBuildSettings - 1)
            PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); // 1 is for true (unlocked)
        else
            Debug.Log("Level not in build");
    }

    #endregion Public Methods
}