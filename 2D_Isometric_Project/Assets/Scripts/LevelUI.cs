using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        // Resume the game and restart the level
        Time.timeScale = 1f;
        LevelManager.Instance.RestartLevel();
    }

    public void OnMainMenuButtonClicked()
    {
        // Resume the game and load the main menu
        Time.timeScale = 1f;
        LevelManager.Instance.LoadMainMenu();
    }
}
