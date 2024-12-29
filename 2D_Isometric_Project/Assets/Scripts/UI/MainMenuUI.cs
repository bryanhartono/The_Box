using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject mainMenuPanel;
    
    private void OnEnable() 
    {
        SoundManager.Instance.PlayBGM(BGMType.MainMenu);
        
        mainMenuPanel.SetActive(!LevelManager.Instance.shouldOpenLevelSelect);
        levelSelectPanel.SetActive(LevelManager.Instance.shouldOpenLevelSelect);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        levelSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnOptionsButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        optionsPanel.SetActive(true);
        optionsPanel.GetComponent<OptionsPanelUI>().HideLevelOnlyButtons();
    }
}
