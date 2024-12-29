using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image bestTimeBackgroundImage;

    [Header("Button Sprites")]
    [SerializeField] private Sprite enabledSprite;
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;

    private int levelId;

    public void Init(int id, bool isUnlocked, float bestTime)
    {
        levelId = id;
        levelText.text = id.ToString();

        if (!isUnlocked)
        {
            button.interactable = false;
            bestTimeText.text = "Locked";
            buttonImage.sprite = disabledSprite;
            bestTimeBackgroundImage.sprite = lockedSprite;
        }
        else if (bestTime > 0)
        {
            button.interactable = true;
            buttonImage.sprite = enabledSprite;
            bestTimeBackgroundImage.sprite = unlockedSprite;
            
            int minutes = Mathf.FloorToInt(bestTime / 60);
            int seconds = Mathf.FloorToInt(bestTime % 60);
            string bestTimeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            bestTimeText.text = "Best Time:\n" + bestTimeString;
        }
    }

    public void OnLevelButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        LevelManager.Instance.LoadLevel(levelId);
    }
}
