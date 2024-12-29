using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject levelSelectButton;
    
    private void OnEnable() 
    {
        GameManager.Instance.PauseGame();
        
        InitUI();

        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        muteToggle.onValueChanged.AddListener(ToggleMute);
    }

    private void OnDisable() 
    {
        sfxSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.RemoveAllListeners();
        muteToggle.onValueChanged.RemoveAllListeners();

        GameManager.Instance.ResumeGame();
    }

    private void InitUI()
    {
        // Initialize sliders with saved volume values
        if (SoundManager.Instance != null)
        {
            sfxSlider.value = SoundManager.Instance.SfxVolume; // Load saved SFX volume
            bgmSlider.value = SoundManager.Instance.BgmVolume; // Load saved BGM volume
            muteToggle.isOn = SoundManager.Instance.IsMuted;   // Load saved mute state
        }
    }

    public void HideLevelOnlyButtons()
    {
        mainMenuButton.SetActive(false);
        levelSelectButton.SetActive(false);
    }

    private void SetSFXVolume(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
    }

    private void SetBGMVolume(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(value);
        }
    }

    private void ToggleMute(bool isMuted)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
            SoundManager.Instance.ToggleMute(isMuted);
        }
    }

    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        gameObject.SetActive(false);
    }

    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        LevelManager.Instance.LoadMainMenu();
    }

    public void OnLevelSelectionButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        LevelManager.Instance.LoadLevelSelectionMenu();
    }
}
