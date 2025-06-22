using UnityEngine;

public class BallSoundManager : MonoBehaviour
{
    public static BallSoundManager Instance;

    [SerializeField] private AudioSource audioSource;

    private int currentPriority = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip, int priority)
    {
        if (clip == null || audioSource == null) return;

        if (!audioSource.isPlaying || priority >= currentPriority)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            currentPriority = priority;
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentPriority = 0;
        }
    }
}