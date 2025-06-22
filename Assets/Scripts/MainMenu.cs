using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using YG;

public class MainMenu : MonoBehaviour
{
    private const string lastSceneKey = "LastScene";

    [Header("UI")]
    public Button playButton;                   // Кнопка "Играть" / "Уровни"
    public TextMeshProUGUI playButtonText;      // Текст на кнопке (подключи в инспекторе)
    public Button restartButton;                // Кнопка "Перезагрузить" (подключи в инспекторе)

    void Start()
    {
        if (!PlayerPrefs.HasKey(lastSceneKey))
        {
            PlayerPrefs.SetString(lastSceneKey, "Level1");
            PlayerPrefs.Save();
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool hasSave = PlayerPrefs.HasKey(lastSceneKey);

        if (playButtonText != null)
            playButtonText.text = hasSave ? "Уровни" : "Играть";

        if (playButton != null)
            playButton.gameObject.SetActive(hasSave);

        if (restartButton != null)
            restartButton.gameObject.SetActive(hasSave);
    }

    public void FirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayGame()
    {
        string sceneToLoad = PlayerPrefs.GetString(lastSceneKey, "");
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
        {
            Debug.LogWarning("Сохранённая сцена пуста. Загружается первый уровень.");
            SceneManager.LoadScene("Level1");
        }
    }

    public void MainMenus()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ProgressScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void RestartLevel()
    {
        // Показ рекламы перед перезапуском, если нужно
        if (YG2.isTimerAdvCompleted && !YG2.nowAdsShow)
            YG2.InterstitialAdvShow();

        // Удаляем все шары с сцены
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
            Destroy(ball.gameObject);

        // Сброс очков, если требуется
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        // Локальный счётчик перезапусков
        int restartCount = PlayerPrefs.GetInt("RestartLevelCount", 0) + 1;
        PlayerPrefs.SetInt("RestartLevelCount", restartCount);
        PlayerPrefs.Save();

        // --- Метрики ---

        // 1. Простой счёт события
        YG2.MetricaSend("level_restart");

        // 2. Счёт с параметром: сколько раз нажали перезапуск
        YG2.MetricaSend("level_restart_count", "count", restartCount.ToString());

        // 3. Отправка с более детальной структурой
        var restartData = new Dictionary<string, object>
        {
            { "restartNumber", restartCount },
            { "scene", SceneManager.GetActiveScene().name },
            { "timestamp", System.DateTime.UtcNow.ToString("o") }
        };
        YG2.MetricaSend("level_restart_detailed", restartData);


        // Очистка сохранённых шаров
        YG2.saves.savedBalls.Clear();
        YG2.SaveProgress();

        // Перезагрузка сцены
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName =
                System.IO.Path.GetFileNameWithoutExtension(
                    SceneUtility.GetScenePathByBuildIndex(nextIndex));
            PlayerPrefs.SetString(lastSceneKey, nextSceneName);
            PlayerPrefs.Save();
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("Следующего уровня не существует в Build Settings.");
        }
    }

    public void LoadFunScene()
    {
        SceneManager.LoadScene("Fun");
    }
}
