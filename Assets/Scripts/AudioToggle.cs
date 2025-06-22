using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private Button toggleSoundButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private bool isMuted = true;
    private Image buttonImage;
    private bool soundEnabledAutomatically = false;

    private const string PlayerPrefsKey = "IsMuted";

    private void Start()
    {
        buttonImage = toggleSoundButton.GetComponent<Image>();
        toggleSoundButton.onClick.AddListener(ToggleSound);

        // Загружаем состояние звука из PlayerPrefs
        isMuted = PlayerPrefs.GetInt(PlayerPrefsKey, 1) == 1; // По умолчанию звук выключен
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateButtonImage();

        // Запускаем автоматическое включение звука через 2 секунды
        Invoke(nameof(EnableSoundAutomatically), 2f);
    }

    private void Update()
    {
        if (!soundEnabledAutomatically && Input.GetMouseButtonDown(0))
        {
            EnableSoundManually();
        }
    }

    private void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateButtonImage();

        PlayerPrefs.SetInt(PlayerPrefsKey, isMuted ? 1 : 0); // Сохраняем состояние
        PlayerPrefs.Save();

        // ✅ Метрика: отправляем событие в зависимости от действия
#if YG
    if (isMuted)
        YG.YandexGame.SendEvent("sound_muted");     // игрок выключил звук
    else
        YG.YandexGame.SendEvent("sound_unmuted");   // игрок включил звук
#endif
    }

    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }

    private void EnableSoundAutomatically()
    {
        if (soundEnabledAutomatically) return;
        soundEnabledAutomatically = true;

        // Только включаем звук, если пользователь не отключил его вручную
        if (isMuted && !PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            isMuted = false;
            AudioListener.volume = 1f;
            UpdateButtonImage();
            PlayerPrefs.SetInt(PlayerPrefsKey, 0); // Сохраняем включенный звук
            PlayerPrefs.Save();
        }
    }

    private void EnableSoundManually()
    {
        CancelInvoke(nameof(EnableSoundAutomatically));
        EnableSoundAutomatically();
    }
}