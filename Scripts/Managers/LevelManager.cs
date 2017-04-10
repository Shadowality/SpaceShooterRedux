using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    #region Public Fields

    [Tooltip("Load next level in x seconds. 0 to disable.")]
    public float autoLoadNextLevel;

    #endregion Public Fields

    #region Private Fields

    private int highestScore;
    private string previousScene;

    #endregion Private Fields

    #region Public Methods

    public void LoadLevel(string name)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        autoLoadNextLevel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void LoadPreviousLevel()
    {
        LoadLevel(previousScene);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion Protected Methods

    #region Private Methods

    private void Start()
    {
        previousScene = SceneManager.GetActiveScene().name;
        if (autoLoadNextLevel <= 0)
            Debug.Log("Autoload disabled.");
        else
            Invoke("LoadNextLevel", autoLoadNextLevel);
    }

    #endregion Private Methods
}