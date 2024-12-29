using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }

    // Example of GameManager responsibilities
    public bool IsGamePaused { get; private set; }

    public void PauseGame()
    {
        IsGamePaused = true;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
