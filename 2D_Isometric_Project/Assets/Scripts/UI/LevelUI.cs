using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private TMP_Text gameEndText;

    private void Awake() 
    {
        levelEndPanel.SetActive(false);
    }

    private void Start() 
    {
        SoundManager.Instance.PlayBGM(BGMType.Level);
    }

    public void ShowLevelEndPanel(bool isVictory)
    {
        gameEndText.text = isVictory ? "Level Cleared!" : "Time's Up!";
        GameManager.Instance.PauseGame();
        levelEndPanel.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);

        // Resume the game and restart the level
        GameManager.Instance.ResumeGame();
        LevelManager.Instance.RestartLevel();
    }

    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);

        // Resume the game and load the main menu
        GameManager.Instance.ResumeGame();
        LevelManager.Instance.LoadMainMenu();
    }

    public void OnOptionsButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        optionsPanel.SetActive(true);
    }

    public void OnLevelSelectionButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);

        // Resume the game and load the level select menu
        GameManager.Instance.ResumeGame();
        LevelManager.Instance.LoadLevelSelectionMenu();
    }
}
