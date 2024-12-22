using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalPlatform : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Light2D light2D;
    [SerializeField] private Color victoryColor;
    [SerializeField] private Color defaultColor;

    private void Awake()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        light2D.color = defaultColor;
    }

    private void TriggerVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        light2D.color = victoryColor;
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerVictory();
        }
    }
}
