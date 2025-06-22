using System.Collections;
using UnityEngine;

public class ShakeBalls : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("������������ ������ ������� ����, ���.")]
    [SerializeField] private float shakeDuration = 0.5f;
    [Tooltip("������������ �������� �� ������������ �������.")]
    [SerializeField] private float shakeMagnitude = 0.2f;

    /// <summary>
    /// ���������� �� UI-������: ���������� ��� ���� � BallController � ����� ��.
    /// </summary>
    public void ShakeAllBalls()
    {
        // ������� ��� ������� � ����� �������� BallController
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

        // ���������� �� �������� �������
        t.position = originalPos;
    }
}