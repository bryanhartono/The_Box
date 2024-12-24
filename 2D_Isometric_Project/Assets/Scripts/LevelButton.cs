using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    private int levelId;

    public void Init(int id)
    {
        levelId = id;
        levelText.text = (id + 1).ToString();
    }

    public void OnLevelButtonClicked()
    {
        string levelName = LevelManager.Instance.getLevelName(levelId);
        LevelManager.Instance.LoadLevel(levelName);
    }
}
