using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        var originalRot = transform.localRotation;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            float offsetZ = Random.Range(-20f, 20f) * magnitude;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            transform.localRotation = originalRot * Quaternion.Euler(new Vector3(0, 0, offsetZ));
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }

    public void ShakeCamera(float duration = 0.25f, float magnitude = 0.5f)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}