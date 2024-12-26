using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab; // Assign the level button prefab in the Inspector
    [SerializeField] private Transform levelPanel; // Assign the panel that holds the buttons in the Inspector

    private const int xDistanceBetweenLevelButtons = 325;
    private const int yDistanceBetweenLevelButtons = 300;
    private const int levelButtonStartXPos = -650;
    private const int levelButtonStartYPos = 150;

    private void Start()
    {
        InitializeLevelButtons();
    }

    private void InitializeLevelButtons()
    {
        // Clear existing buttons
        foreach (Transform child in levelPanel)
        {
            Destroy(child.gameObject);
        }
        
        int currentXPos = levelButtonStartXPos;
        int currentYPos = levelButtonStartYPos;
        int levelCount = LevelManager.Instance.GetLevelCount();

        // Dynamically create level buttons
        for (int i = 0; i < levelCount; i++)
        {
            if (i != 0 && i % 5 == 0)
            {
                currentYPos -= yDistanceBetweenLevelButtons;
            }
            
            GameObject levelButtonGameObject = Instantiate(levelButtonPrefab, levelPanel);
            levelButtonGameObject.transform.localPosition = new Vector3(currentXPos + ((i % 5) * xDistanceBetweenLevelButtons), currentYPos, 0);

            // init button
            LevelButton currentLevelButton = levelButtonGameObject.GetComponent<LevelButton>();
            if (currentLevelButton != null)
            {
                bool isUnlocked = SaveManager.Instance.IsLevelUnlocked(i);
                float bestTime = SaveManager.Instance.GetLevelTime(i);

                currentLevelButton.Init(i + 1, isUnlocked, bestTime);
            }
        }
    }

    public void OnBackButtonClicked()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
