using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalPlatform : MonoBehaviour
{
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private Light2D light2D;
    [SerializeField] private Color victoryColor;
    [SerializeField] private Color defaultColor;

    [SerializeField] private TimerController timerController;

    private void Awake()
    {
        light2D.color = defaultColor;
    }

    private void TriggerVictory()
    {
        timerController.StopTimer();
        levelUI.ShowLevelEndPanel(true);
        light2D.color = victoryColor;

        LevelManager.Instance.CompleteLevel(timerController.GetRemainingTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerVictory();
        }
    }
}
