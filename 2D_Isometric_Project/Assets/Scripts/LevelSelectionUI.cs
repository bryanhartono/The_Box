using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab; // Assign the level button prefab in the Inspector
    [SerializeField] private Transform levelPanel; // Assign the panel that holds the buttons in the Inspector

    List<string> levelList = new List<string>();

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

        // Get levels from level manager
        LevelManager.Instance.LoadLevelList(ref levelList);
        
        int currentXPos = levelButtonStartXPos;
        int currentYPos = levelButtonStartYPos;

        // Dynamically create level buttons
        for (int i = 0; i < levelList.Count; i++)
        {
            if (i != 0 && i % 5 == 0)
            {
                currentYPos -= yDistanceBetweenLevelButtons;
            }
            
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelPanel);
            buttonObj.transform.localPosition = new Vector3(currentXPos + ((i % 5) * xDistanceBetweenLevelButtons), currentYPos, 0);
            Button button = buttonObj.GetComponent<Button>();
            int levelID = i;

            // Assign the button click event
            button.onClick.AddListener(() => OnLevelButtonClicked(levelID));

            // Update button text
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = $"Level {levelID + 1}";
            }
        }
    }

    private void OnLevelButtonClicked(int levelID)
    {
        string levelName = levelList[levelID];
        LevelManager.Instance.LoadLevel(levelName);
    }

    public void OnBackButtonClicked()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
