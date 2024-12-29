using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TimerController : MonoBehaviour
{
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private TMP_Text timerText; // Assign this in the Inspector
    [SerializeField] private float levelDurationInSeconds = 60f; // Total time for the level in seconds

    private float timeRemaining;
    private bool isTimerRunning = false;

    private void Awake() 
    {
        timeRemaining = levelDurationInSeconds;
        UpdateTimerDisplay();
    }

    private void Start()
    {
        timeRemaining = levelDurationInSeconds;
        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning && !GameManager.Instance.IsGamePaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                HandleLoss();
            }
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public float GetRemainingTime()
    {
        return timeRemaining;
    }

    private void UpdateTimerDisplay()
    {
        // Format time as minutes:seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void HandleLoss()
    {
        SoundManager.Instance.PlaySFX(SFXType.Lose);

        UpdateTimerDisplay();
        isTimerRunning = false;
        levelUI.ShowLevelEndPanel(false);
    }
}
