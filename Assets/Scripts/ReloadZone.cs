using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using YG;

public class ReloadZone : MonoBehaviour
{
    [Header("Параметры перезапуска")]
    [SerializeField] private string targetLayerName = "Ball";
    [SerializeField] private float delayBeforeReload = 2f;

    [Header("UI Элементы")]
    [SerializeField] private GameObject loseBackground;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Размер шрифта для отсчёта")]
    [SerializeField] private float minFontSize = 60f;
    [SerializeField] private float maxFontSize = 160f;

    [Header("Мигание объекта")]
    [SerializeField] private GameObject objectToBlink;
    [SerializeField] private Color blinkColor1 = Color.red;
    [SerializeField] private Color blinkColor2 = Color.white;
    [SerializeField] private float blinkInterval = 0.3f;

    [SerializeField] private GameObject objectToDisable;

    private bool gameOver = false;
    private Coroutine blinkCoroutine;
    private Coroutine countdownCoroutine;
    private SpriteRenderer blinkRenderer;
    private Color originalColor;

    private HashSet<GameObject> ballsInZone = new HashSet<GameObject>();

    private void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
            ReloadScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (ballsInZone.Add(collision.gameObject) && ballsInZone.Count == 1)
                StartBlinkAndCountdown();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (ballsInZone.Remove(collision.gameObject) && ballsInZone.Count == 0)
                StopBlinkAndCountdown();
        }
    }

    private void StartBlinkAndCountdown()
    {
        if (objectToBlink != null)
        {
            blinkRenderer = objectToBlink.GetComponent<SpriteRenderer>();
            if (blinkRenderer != null)
            {
                originalColor = blinkRenderer.color;
                blinkCoroutine = StartCoroutine(BlinkColor());
            }
        }
        countdownCoroutine = StartCoroutine(StartCountdownAnimation());
    }

    private void StopBlinkAndCountdown()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (blinkRenderer != null) blinkRenderer.color = originalColor;
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    private IEnumerator BlinkColor()
    {
        bool toggle = false;
        while (true)
        {
            blinkRenderer.color = toggle ? blinkColor1 : blinkColor2;
            toggle = !toggle;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator StartCountdownAnimation()
    {
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            countdownText.fontSize = minFontSize;
            float t = 0f, duration = 0.3f;
            while (t < duration)
            {
                t += Time.deltaTime;
                countdownText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, t / duration);
                yield return null;
            }
            yield return new WaitForSeconds(0.7f);
        }
        countdownText.gameObject.SetActive(false);
        ShowLoseUI();
        gameOver = true;
        yield return new WaitForSeconds(delayBeforeReload);
        ReloadScene();
    }

    private void ShowLoseUI()
    {
        if (loseBackground != null) loseBackground.SetActive(true);
        if (loseText != null) loseText.gameObject.SetActive(true);
        if (objectToDisable != null) objectToDisable.SetActive(false);
    }

    private void ReloadScene()
    {
        // 1. Счётчик поражений
        int loses = PlayerPrefs.GetInt("LoseCount", 0) + 1;
        PlayerPrefs.SetInt("LoseCount", loses);
        PlayerPrefs.Save();

        // 2. Отправка метрик через YG2
        var loseData = new Dictionary<string, object>
        {
            { "lose_count", loses },
            { "scene", SceneManager.GetActiveScene().name },
            { "timestamp", System.DateTime.UtcNow.ToString("o") } // ISO 8601 формат времени
        };

        YG2.MetricaSend("player_lose", loseData);

        // 3. Удаляем все шары
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
            Destroy(ball.gameObject);

        // 4. Очищаем сохранённые позиции шаров
        YG2.saves.savedBalls.Clear();
        YG2.SaveProgress();

        // 5. Перезагрузка сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
