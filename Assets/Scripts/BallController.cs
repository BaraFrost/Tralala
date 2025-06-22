using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float angle;
    private Rigidbody2D rb;

    [SerializeField] private string targetTag = "Ball1";
    [SerializeField] private GameObject explosionEffect;

    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private int soundPriority = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * -transform.up;
        rb.velocity = direction.normalized * speed;

        BallSoundManager.Instance.PlaySound(spawnSound, soundPriority);

        // Уведомляем о появлении шара с определённым тегом
        if (BallTagPanelController.Instance != null)
        {
            BallTagPanelController.Instance.OnBallSpawned(gameObject.tag);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag) && collision.gameObject != gameObject)
        {
            // Убедимся, что только один из пары объектов выполняет объединение
            if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
            {
                Vector3 contactPoint = (transform.position + collision.transform.position) / 2f;

                GameObject newBall = Instantiate(ballPrefab, contactPoint, Quaternion.identity);

                // Попробуем получить значение шара
                BallValue thisBall = GetComponent<BallValue>();
                BallValue otherBall = collision.gameObject.GetComponent<BallValue>();

                int newScoreValue = 1;
                if (thisBall != null && otherBall != null)
                {
                    newScoreValue = thisBall.value + otherBall.value;

                    BallValue newBallValue = newBall.GetComponent<BallValue>();
                    if (newBallValue != null)
                    {
                        newBallValue.value = newScoreValue;
                        newBallValue.UpdateVisual(); // если реализовано
                    }

                    // Добавим очки за мерж
                    ScoreManager.Instance?.AddScore(newScoreValue);
                }

                Explode(contactPoint);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }

    void Explode(Vector3 contactPoint)
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, contactPoint, Quaternion.identity);
        }
    }
}
