using System.Collections;
using UnityEngine;

public class ShakeBalls : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("Длительность тряски каждого шара, сек.")]
    [SerializeField] private float shakeDuration = 0.5f;
    [Tooltip("Максимальное смещение от оригинальной позиции.")]
    [SerializeField] private float shakeMagnitude = 0.2f;

    /// <summary>
    /// Вызывается из UI-кнопки: перебирает все шары с BallController и трясёт их.
    /// </summary>
    public void ShakeAllBalls()
    {
        // Находим все объекты с твоим скриптом BallController
        var balls = FindObjectsOfType<BallController>();
        foreach (var ball in balls)
        {
            StartCoroutine(Shake(ball.transform));
        }
    }

    private IEnumerator Shake(Transform t)
    {
        Vector3 originalPos = t.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            t.position = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Возвращаем на исходную позицию
        t.position = originalPos;
    }
}