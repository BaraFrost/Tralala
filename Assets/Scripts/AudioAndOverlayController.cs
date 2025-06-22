using UnityEngine;
using YG;

public class AudioAndOverlayController : MonoBehaviour
{
    [Header("Аудио")]
    [SerializeField] private AudioSource audioSourceToDisable;

    [Header("UI")]
    [SerializeField] private GameObject overlayBackground; // Фон (например, Image, на весь экран)
    [SerializeField] private GameObject overlayContent;    // Контент поверх фона (текст, кнопки и т.п.)

    [Header("Объекты для отключения")]
    [SerializeField] private GameObject objectToToggle;       // Основной объект
    [SerializeField] private GameObject additionalObject1;    // Дополнительный объект 1
    [SerializeField] private GameObject additionalObject2;    // Дополнительный объект 2

    private bool originalAudioState;
    private bool originalObjectState;
    private bool additionalObject1State;
    private bool additionalObject2State;

    /// <summary>
    /// Активирует UI-оверлей, отключает звук и объекты.
    /// </summary>
    public void ActivateOverlay()
    {
        // 1. Отключить звук
        if (audioSourceToDisable != null)
        {
            originalAudioState = audioSourceToDisable.enabled;
            audioSourceToDisable.enabled = false;
        }

        // 2. Показать фон
        if (overlayBackground != null)
        {
            overlayBackground.SetActive(true);
            StretchToFullScreen(overlayBackground);
        }

        // 3. Показать контент поверх фона
        if (overlayContent != null)
            overlayContent.SetActive(true);

        // 4. Отключить основной объект
        if (objectToToggle != null)
        {
            originalObjectState = objectToToggle.activeSelf;
            objectToToggle.SetActive(false);
        }

        // 5. Отключить дополнительные объекты
        if (additionalObject1 != null)
        {
            additionalObject1State = additionalObject1.activeSelf;
            additionalObject1.SetActive(false);
        }

        if (additionalObject2 != null)
        {
            additionalObject2State = additionalObject2.activeSelf;
            additionalObject2.SetActive(false);
        }

        // ✅ Метрика: клик по кнопке активации оверлея
        YG2.MetricaSend("overlay_activated");
    }
    public void DeactivateOverlay()
    {
        // 1. Включить звук
        if (audioSourceToDisable != null)
            audioSourceToDisable.enabled = originalAudioState;

        // 2. Скрыть фон
        if (overlayBackground != null)
            overlayBackground.SetActive(false);

        // 3. Скрыть контент
        if (overlayContent != null)
            overlayContent.SetActive(false);

        // 4. Включить основной объект обратно
        if (objectToToggle != null)
            objectToToggle.SetActive(originalObjectState);

        // 5. Включить дополнительные объекты обратно
        if (additionalObject1 != null)
            additionalObject1.SetActive(additionalObject1State);

        if (additionalObject2 != null)
            additionalObject2.SetActive(additionalObject2State);
    }

    /// <summary>
    /// Растягивает RectTransform на весь экран (если фон — UI элемент).
    /// </summary>
    private void StretchToFullScreen(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        else
        {
            Debug.LogWarning("Overlay background does not have a RectTransform!");
        }
    }
}
