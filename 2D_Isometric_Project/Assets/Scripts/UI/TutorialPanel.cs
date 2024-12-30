using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject optionsButton;

    private void OnEnable() 
    {
        GameManager.Instance.PauseGame();    
    }

    private void OnDisable() 
    {
        GameManager.Instance.ResumeGame();    
    }

    public void OnStartButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        optionsButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
