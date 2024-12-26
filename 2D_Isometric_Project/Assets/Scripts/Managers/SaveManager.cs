using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<bool> levelUnlocked; // Tracks whether each level is unlocked
    public List<float> levelTimeRecords; // Stores the best time for each level (0 if not completed)

    public SaveData(int totalLevels)
    {
        levelUnlocked = new List<bool>(new bool[totalLevels]);
        levelUnlocked[0] = true; // Unlock the first level by default

        levelTimeRecords = new List<float>(new float[totalLevels]); // Initialize time records to 0
    }
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;

    public static SaveManager Instance { get { return _instance; } }

    private string saveFilePath;
    private SaveData saveData;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
            saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
    }

    private void Start()
    {
        LoadGame();
    }

    // Save the game data to file
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        //Debug.Log("Game saved to: " + saveFilePath);
    }

    // Load the game data from file
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            //Debug.Log("Game loaded from: " + saveFilePath);
        }
        else
        {
            //Debug.Log("No save file found. Creating a new save file.");
            saveData = new SaveData(LevelManager.Instance.GetLevelCount());
            SaveGame();
        }
    }

    // Unlock a level
    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < saveData.levelUnlocked.Count)
        {
            saveData.levelUnlocked[levelIndex] = true;
            SaveGame();
        }
    }

    // Save the best time for a level
    public void SaveLevelTime(int levelIndex, float time)
    {
        if (levelIndex >= 0 && levelIndex < saveData.levelTimeRecords.Count)
        {
            if (saveData.levelTimeRecords[levelIndex] == 0 || time < saveData.levelTimeRecords[levelIndex])
            {
                saveData.levelTimeRecords[levelIndex] = time;
                SaveGame();
            }
        }
    }

    // Get whether a level is unlocked
    public bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < saveData.levelUnlocked.Count)
        {
            return saveData.levelUnlocked[levelIndex];
        }
        return false;
    }

    // Get the best time for a level
    public float GetLevelTime(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < saveData.levelTimeRecords.Count)
        {
            return saveData.levelTimeRecords[levelIndex];
        }
        return 0;
    }
}
