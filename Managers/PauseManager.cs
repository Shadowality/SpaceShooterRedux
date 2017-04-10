using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    #region PUBLIC VARIABLES
    public static PauseManager Instance;
    public Sprite pauseSprite;
    public Sprite playSprite;
    #endregion

    #region PRIVATE VARIABLES
    private static bool paused;
    private static GameObject pausePrefab;
    private static RectTransform[] pausePrefabAndChildren;
    private static GameObject pausePanel;
    private static GameObject pauseButton;
    private static Image pauseButtonImage;

    public static GameObject PauseButton
    {
        get
        {
            return pauseButton;
        }

        set
        {
            pauseButton = value;
        }
    }
    #endregion

    #region METHODS
    void Awake()
    {
        Instance = this;
        pausePrefab = GameObject.Find("Pause");
        pausePrefabAndChildren = pausePrefab.GetComponentsInChildren<RectTransform>(true);
        pausePanel = pausePrefabAndChildren[1].gameObject;
        PauseButton = pausePrefabAndChildren[2].gameObject;
        pauseButtonImage = PauseButton.GetComponent<Image>();
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

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        paused = false;
        Time.timeScale = 1;
        AudioListener.pause = paused;
    }

    void SetPausePanel()
    {
        pauseButtonImage.sprite = paused ? playSprite : pauseSprite;
        pausePanel.SetActive(paused);
    }

    public void ChangeGameState()
    {
        paused = !paused ? !paused : !paused;
        Time.timeScale = paused ? 0 : 1;
        AudioListener.pause = paused;
        SetPausePanel();
    }

    #endregion
}