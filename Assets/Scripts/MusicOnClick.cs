using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class MusicOnClick : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("јудиоклип, который должен проигрыватьс€ при нажатии")]
    [SerializeField] private AudioClip musicClip;

    private AudioSource audioSource;

    private static AudioSource currentlyPlayingSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = musicClip;
        audioSource.loop = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (musicClip == null)
        {
            Debug.LogWarning("Music clip is not assigned on " + gameObject.name);
            return;
        }

        // ≈сли другой источник играет Ч остановим его
        if (currentlyPlayingSource != null && currentlyPlayingSource != audioSource)
        {
            currentlyPlayingSource.Stop();
        }

        // ¬сегда перезапускаем звук, даже если это тот же самый
        audioSource.Stop();
        audioSource.Play();
        currentlyPlayingSource = audioSource;
    }
}