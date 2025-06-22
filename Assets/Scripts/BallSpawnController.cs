using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BallSpawnController : MonoBehaviour
{
    [Serializable]
    public struct Spawnable
    {
        public GameObject prefab;
        public Sprite icon;
        [Range(0, 100)] public float spawnChance;
    }

    [SerializeField] private BallSaveManager saveManager;

    [Header("Настройки спавна")]
    [SerializeField] private List<Spawnable> spawnables;
    [SerializeField] private Transform spawnPoint;

    [Header("Время отключения визуала (SpriteRenderer и LineRenderer)")]
    [SerializeField] private float visualDisableDuration = 0.75f;

    [Header("Время восстановления масштаба (и разрешения нового спавна)")]
    [SerializeField] private float scaleRestoreDuration = 0.75f;

    [Header("Ограничения по X")]
    [SerializeField] private float xMin = -8f;
    [SerializeField] private float xMax = 8f;

    [Header("UI элементы")]
    [SerializeField] private TextMeshProUGUI ballCountText;
    [SerializeField] private List<Image> nextBallsUI;

    [Header("Ссылка на панель, которая обновляет иконки по тегам")]
    [SerializeField] private BallTagPanelController ballTagPanelController;

    [Header("UI Исключения для Drag")]
    [SerializeField] private RectTransform draggableExceptionUI;

    [Header("Лайн рендерер для отключения")]
    [SerializeField] private LineRenderer lineRendererToDisable;

    private SpriteRenderer currentBallSpriteRenderer;
    private bool isDragging = false;

    private int currentIndex = 0;
    private Queue<int> nextIndices = new Queue<int>();

    private Camera mainCamera;

    private Vector3 defaultScale;
    private Coroutine scaleCoroutine;

    private bool canSpawn = true; // теперь это отвечает за разрешение на спавн

    void Start()
    {
        saveManager.LoadBalls();
        currentBallSpriteRenderer = GetComponent<SpriteRenderer>();
        if (currentBallSpriteRenderer == null)
            Debug.LogWarning("SpriteRenderer не найден на объекте BallSpawnController.");

        if (spawnables == null || spawnables.Count == 0)
            Debug.LogWarning("Список объектов для спавна пуст!");

        if (spawnPoint == null)
        {
            GameObject found = GameObject.FindWithTag("SpawnPoint");
            if (found != null)
            {
                spawnPoint = found.transform;
                Debug.Log("SpawnPoint найден автоматически по тегу.");
            }
            else
            {
                Debug.LogError("SpawnPoint не назначен в инспекторе и не найден по тегу! Установи его вручную.");
            }
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Камера с тегом 'MainCamera' не найдена в сцене! Drag не будет работать.");
        }

        NormalizeChances();

        defaultScale = transform.localScale;

        currentIndex = GetWeightedRandomIndex();

        nextIndices.Clear();
        for (int i = 0; i < 3; i++)
            nextIndices.Enqueue(GetWeightedRandomIndex());

        UpdateCurrentBallVisual();
        UpdateNextBallsUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() || IsPointerOverException())
            {
                isDragging = true;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            if (mainCamera == null) return;

            Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            float targetX = Mathf.Clamp(world.x, xMin, xMax);

            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

            if (spawnPoint != null)
            {
                spawnPoint.position = new Vector3(targetX, spawnPoint.position.y, spawnPoint.position.z);
            }
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            TrySpawnBall();
            isDragging = false;
        }
    }

    private void TrySpawnBall()
    {
        if (!canSpawn) return;
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint не назначен. Невозможно создать объект.");
            return;
        }

        float randomOffsetX = UnityEngine.Random.Range(-0.1f, 0.1f);
        Vector3 spawnPosition = spawnPoint.position + new Vector3(randomOffsetX, 0f, 0f);

        GameObject spawnedBall = Instantiate(spawnables[currentIndex].prefab, spawnPosition, Quaternion.identity);

        StartCoroutine(HandleVisualsAndScale());

        if (ballTagPanelController != null && spawnedBall != null)
        {
            ballTagPanelController.OnBallSpawned(spawnedBall.tag);
        }

        if (nextIndices.Count > 0)
            currentIndex = nextIndices.Dequeue();

        nextIndices.Enqueue(GetWeightedRandomIndex());

        UpdateCurrentBallVisual();
        UpdateNextBallsUI();
    }

    private IEnumerator HandleVisualsAndScale()
    {
        canSpawn = false;

        transform.localScale = defaultScale * 0.6f;

        if (currentBallSpriteRenderer != null)
            currentBallSpriteRenderer.enabled = false;
        if (lineRendererToDisable != null)
            lineRendererToDisable.enabled = false;

        yield return new WaitForSeconds(visualDisableDuration);

        if (currentBallSpriteRenderer != null)
            currentBallSpriteRenderer.enabled = true;
        if (lineRendererToDisable != null)
            lineRendererToDisable.enabled = true;

        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleBackToDefault());
    }

    private IEnumerator ScaleBackToDefault()
    {
        float t = 0;
        Vector3 from = transform.localScale;

        while (t < scaleRestoreDuration)
        {
            t += Time.deltaTime;
            float progress = t / scaleRestoreDuration;
            transform.localScale = Vector3.Lerp(from, defaultScale, progress);
            yield return null;
        }

        transform.localScale = defaultScale;
        canSpawn = true;
    }

    private void UpdateCurrentBallVisual()
    {
        if (currentBallSpriteRenderer != null && IsValidIndex(currentIndex))
        {
            currentBallSpriteRenderer.sprite = spawnables[currentIndex].icon;
        }
    }

    private void UpdateNextBallsUI()
    {
        if (nextBallsUI == null) return;

        int i = 0;
        foreach (var idx in nextIndices)
        {
            if (i >= nextBallsUI.Count) break;
            if (IsValidIndex(idx))
            {
                nextBallsUI[i].sprite = spawnables[idx].icon;
                nextBallsUI[i].color = Color.white;
            }
            else
            {
                nextBallsUI[i].sprite = null;
                nextBallsUI[i].color = Color.clear;
            }
            i++;
        }

        for (; i < nextBallsUI.Count; i++)
        {
            nextBallsUI[i].sprite = null;
            nextBallsUI[i].color = Color.clear;
        }
    }

    private int GetWeightedRandomIndex()
    {
        float totalWeight = 0f;
        foreach (var s in spawnables)
            totalWeight += s.spawnChance;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentSum = 0f;

        for (int i = 0; i < spawnables.Count; i++)
        {
            currentSum += spawnables[i].spawnChance;
            if (randomValue <= currentSum)
                return i;
        }

        return spawnables.Count - 1;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < spawnables.Count;
    }

    private void NormalizeChances()
    {
        float total = 0;
        foreach (var item in spawnables)
            total += item.spawnChance;

        if (Mathf.Approximately(total, 100f)) return;

        Debug.LogWarning($"Сумма всех вероятностей = {total}, а не 100. Это допустимо, но проверь значения.");
    }

    private bool IsPointerOverException()
    {
        if (draggableExceptionUI == null) return false;

        Vector2 mousePos = Input.mousePosition;
        return RectTransformUtility.RectangleContainsScreenPoint(draggableExceptionUI, mousePos, mainCamera);
    }

    void OnApplicationQuit()
    {
        saveManager.SaveAllBalls();
    }
}
