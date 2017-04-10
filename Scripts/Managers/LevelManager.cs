using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// DONE
public class LevelManager : Singleton<LevelManager>
{
    #region PUBLIC VARIABLES
    [Tooltip("Load next level in x seconds. 0 to disable.")]
    public float autoLoadNextLevel;
    #endregion

    #region PRIVATE VARIABLES
    private int highesScore;
    private string previousScene;
    #endregion

    #region METHODS
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        previousScene = SceneManager.GetActiveScene().name;
        if (autoLoadNextLevel <= 0)
            Debug.Log("Autoload disabled.");
        else
            Invoke("LoadNextLevel", autoLoadNextLevel);
    }

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
    #endregion
}
