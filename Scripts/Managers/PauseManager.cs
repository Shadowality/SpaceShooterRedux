using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    #region Public Fields

    public Sprite pauseSprite;
    public Sprite playSprite;
    public static PauseManager Instance;

    #endregion Public Fields

    #region Private Fields

    private static bool paused;
    private static GameObject pauseButton;
    private static GameObject pausePanel;
    private static GameObject pausePrefab;
    private static Image pauseButtonImage;
    private static RectTransform[] pausePrefabAndChildren;

    #endregion Private Fields

    #region Public Properties

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

    #endregion Public Properties

    #region Public Methods

    public void ChangeGameState()
    {
        paused = !paused ? !paused : !paused;
        Time.timeScale = paused ? 0 : 1;
        AudioListener.pause = paused;
        SetPausePanel();
    }

    #endregion Public Methods

    #region Private Methods

    private void Awake()
    {
        Instance = this;
        pausePrefab = GameObject.Find("Pause");
        pausePrefabAndChildren = pausePrefab.GetComponentsInChildren<RectTransform>(true);
        pausePanel = pausePrefabAndChildren[1].gameObject;
        PauseButton = pausePrefabAndChildren[2].gameObject;
        pauseButtonImage = PauseButton.GetComponent<Image>();
    }

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
        paused = false;
        Time.timeScale = 1;
        AudioListener.pause = paused;
    }

    private void SetPausePanel()
    {
        pauseButtonImage.sprite = paused ? playSprite : pauseSprite;
        pausePanel.SetActive(paused);
    }

    #endregion Private Methods
}