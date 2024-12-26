using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    [SerializeField] private int levelCount;

    private int currentLevelId;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }

    public int GetLevelCount()
    {
        return levelCount;
    }

    public void LoadLevel(int levelId)
    {
        currentLevelId = levelId - 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_" + levelId.ToString());
    }

    public void RestartLevel()
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void CompleteLevel(float timeTaken)
    {
        // Unlock the next level
        SaveManager.Instance.UnlockLevel(currentLevelId + 1);

        // Save the best time for the current level
        SaveManager.Instance.SaveLevelTime(currentLevelId, timeTaken);
    }
}
