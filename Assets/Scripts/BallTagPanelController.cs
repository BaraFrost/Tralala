using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BallTagPanelController : MonoBehaviour
{
    public static BallTagPanelController Instance { get; private set; }

    [System.Serializable]
    public struct TagIcon
    {
        public string ballTag;
        public Image uiImage;
        public Sprite icon;
    }

    [Header("Иконка по умолчанию")]
    [SerializeField] private Sprite defaultIcon;

    [SerializeField] private List<TagIcon> tagIcons;

    [Header("Настройки сдвига")]
    [SerializeField] private RectTransform iconContainer;
    [SerializeField] private float shiftStep = 100f;
    [SerializeField] private int shiftAfterReveals = 6;
    [SerializeField] private float shiftSpeed = 300f;

    [Header("Параметры частиц")]
    [Tooltip("Объект, из которого будут вылетать частицы")]
    [SerializeField] private Transform particleSpawnPoint;

    [Tooltip("Префаб частиц, который будет воспроизводиться при новом тэге")]
    [SerializeField] private GameObject particlePrefab;

    private Dictionary<string, TagIcon> tagIconDict;
    private HashSet<string> revealedTags = new HashSet<string>();
    private int shiftCount = 0;
    private bool isShifting = false;

    private Coroutine pulseCoroutine;
    private Vector3 originalScale;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        tagIconDict = new Dictionary<string, TagIcon>();
        foreach (var ti in tagIcons)
        {
            if (ti.uiImage != null)
            {
                ti.uiImage.sprite = defaultIcon;
                ti.uiImage.color = Color.white;
                ti.uiImage.enabled = true;
            }

            if (!tagIconDict.ContainsKey(ti.ballTag))
                tagIconDict.Add(ti.ballTag, ti);
            else
                Debug.LogWarning($"Duplicate tag in panel: {ti.ballTag}");
        }

        if (particleSpawnPoint != null)
            originalScale = particleSpawnPoint.localScale;
    }

    public void OnBallSpawned(string ballTag)
    {
        if (tagIconDict.TryGetValue(ballTag, out var tagIcon))
        {
            if (tagIcon.uiImage != null && tagIcon.icon != null)
            {
                tagIcon.uiImage.sprite = tagIcon.icon;
            }

            if (!revealedTags.Contains(ballTag))
            {
                revealedTags.Add(ballTag);

                PlayerPrefs.SetInt($"BallUnlocked_{ballTag}", 1);
                PlayerPrefs.Save();

                // ✅ Метрика открытия нового тега
                YG2.MetricaSend("new_ball_tag");

                // ✅ Метрика последнего открытого тега
                YG2.MetricaSend("last_unlocked_tag");

                Debug.Log($"[Yandex] New ball tag opened: {ballTag}");

                SpawnParticles();

                int neededShifts = revealedTags.Count - shiftAfterReveals;
                if (neededShifts > shiftCount && !isShifting)
                {
                    shiftCount++;
                    StartCoroutine(SmoothShiftIconsLeft());
                }
            }
        }
        else
        {
            Debug.LogWarning($"Ball tag not registered in panel: {ballTag}");
        }
    }

    private void SpawnParticles()
    {
        if (particlePrefab != null && particleSpawnPoint != null)
        {
            GameObject particles = Instantiate(particlePrefab, particleSpawnPoint.position, particleSpawnPoint.rotation);
            Destroy(particles, 3f);

            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(PulseScale(particleSpawnPoint, 1f, 1.3f, 0.5f, 3f));
        }
        else
        {
            Debug.LogWarning("Particle prefab or spawn point not assigned!");
        }
    }

    private IEnumerator PulseScale(Transform target, float minScale, float maxScale, float pulseDuration, float totalDuration)
    {
        float elapsedTotal = 0f;

        while (elapsedTotal < totalDuration)
        {
            float elapsedPulse = 0f;
            while (elapsedPulse < pulseDuration / 2f)
            {
                float scale = Mathf.Lerp(minScale, maxScale, elapsedPulse / (pulseDuration / 2f));
                target.localScale = originalScale * scale;
                elapsedPulse += Time.deltaTime;
                elapsedTotal += Time.deltaTime;
                yield return null;
            }

            elapsedPulse = 0f;
            while (elapsedPulse < pulseDuration / 2f)
            {
                float scale = Mathf.Lerp(maxScale, minScale, elapsedPulse / (pulseDuration / 2f));
                target.localScale = originalScale * scale;
                elapsedPulse += Time.deltaTime;
                elapsedTotal += Time.deltaTime;
                yield return null;
            }
        }

        target.localScale = originalScale;
        pulseCoroutine = null;
    }

    private IEnumerator SmoothShiftIconsLeft()
    {
        if (iconContainer == null)
        {
            Debug.LogWarning("Icon container is not assigned!");
            yield break;
        }

        isShifting = true;

        Vector3 startPos = iconContainer.localPosition;
        Vector3 targetPos = startPos + new Vector3(-shiftStep, 0f, 0f);

        while (Vector3.Distance(iconContainer.localPosition, targetPos) > 0.01f)
        {
            iconContainer.localPosition = Vector3.MoveTowards(
                iconContainer.localPosition,
                targetPos,
                shiftSpeed * Time.deltaTime
            );
            yield return null;
        }

        iconContainer.localPosition = targetPos;
        isShifting = false;
    }
}
