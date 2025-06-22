using UnityEngine;
using YG;

public class RestartPromptUI : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private GameObject overlayBackground;  // Серый полупрозрачный фон
    [SerializeField] private GameObject centerPanel;        // Панель/контейнер в центре
    [SerializeField] private GameObject restartText;        // Надпись "Начать заново?"
    [SerializeField] private GameObject restartButton;      // Кнопка под текстом
    [SerializeField] private GameObject backButton;
    /// <summary>
    /// Показывает окно подтверждения перезапуска.
    /// </summary>
    public void ShowRestartPrompt()
    {
        
        if (overlayBackground != null)
            overlayBackground.SetActive(false);

        if (centerPanel != null)
            centerPanel.SetActive(true);

        if (restartText != null)
            restartText.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);
        if (backButton != null)
            backButton.SetActive(true);
    }

    /// <summary>
    /// Скрывает окно подтверждения перезапуска.
    /// </summary>
    public void HideRestartPrompt()
    {
        if (overlayBackground != null)
            overlayBackground.SetActive(true);

        if (centerPanel != null)
            centerPanel.SetActive(false);

        if (restartText != null)
            restartText.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);
        if (backButton != null)
            backButton.SetActive(false);
    }
}
