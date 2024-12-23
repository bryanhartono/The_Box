using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private TMP_Text gameEndText;

    private void Awake() 
    {
        levelEndPanel.SetActive(false);
    }

    public void ShowLevelEndPanel(bool isVictory)
    {
        gameEndText.text = isVictory ? "Level Cleared!" : "Time's Up!";
        Time.timeScale = 0f;
        levelEndPanel.SetActive(true);
    }

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
