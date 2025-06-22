using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    [Header("Sound")]
    [SerializeField] private AudioClip collisionSound;
    private AudioSource audioSource;

    private Transform currentTarget;
    private bool isDragging = false;
    private Vector3 offset;

    // —татическа€ переменна€ дл€ контрол€ звука
    private static bool isAnyEnemyPlayingSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PickRandomTarget();
    }

    void Update()
    {
        if (isDragging || currentTarget == null) return;

        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.05f)
        {
            PickRandomTarget();
        }
    }

    void PickRandomTarget()
    {
        GameObject[] dots = GameObject.FindGameObjectsWithTag("Dot");

        if (dots.Length == 0) return;

        List<GameObject> validDots = new List<GameObject>();
        foreach (GameObject dot in dots)
        {
            if (dot.transform != currentTarget)
                validDots.Add(dot);
        }

        if (validDots.Count == 0) return;

        int randomIndex = Random.Range(0, validDots.Count);
        currentTarget = validDots[randomIndex].transform;
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseDrag()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionSound != null && audioSource != null && !isAnyEnemyPlayingSound)
        {
            isAnyEnemyPlayingSound = true;
            audioSource.PlayOneShot(collisionSound);
            Invoke(nameof(ResetSoundLock), collisionSound.length);
        }
    }

    void ResetSoundLock()
    {
        isAnyEnemyPlayingSound = false;
    }
}