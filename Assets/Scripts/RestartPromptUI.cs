using UnityEngine;
using YG;

public class RestartPromptUI : MonoBehaviour
{
    [Header("UI ��������")]
    [SerializeField] private GameObject overlayBackground;  // ����� �������������� ���
    [SerializeField] private GameObject centerPanel;        // ������/��������� � ������
    [SerializeField] private GameObject restartText;        // ������� "������ ������?"
    [SerializeField] private GameObject restartButton;      // ������ ��� �������
    [SerializeField] private GameObject backButton;
    /// <summary>
    /// ���������� ���� ������������� �����������.
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
    /// �������� ���� ������������� �����������.
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
