using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Wanderer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float minSpawnInterval = 10f;
    [SerializeField] private float maxSpawnInterval = 30f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject wandererPrefab;

    [Header("Sound")]
    [SerializeField] private AudioClip collisionSound;

    [Header("Settings")]
    [SerializeField] private bool allowDragging = true;
    [SerializeField] private int maxWanderers = 10;

    private static int currentWanderers = 0;

    private AudioSource audioSource;
    private Transform currentTarget;
    private bool isDragging = false;
    private Vector3 offset;
    private float spawnTimer;

    void Start()
    {
        currentWanderers++;
        audioSource = GetComponent<AudioSource>();
        PickRandomTarget();
        ResetSpawnTimer();
    }

    void OnDestroy()
    {
        currentWanderers--;
    }

    void Update()
    {
        if (!isDragging && currentTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.05f)
            {
                PickRandomTarget();
            }
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnCopy();
            ResetSpawnTimer();
        }
    }

    void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
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
        if (!allowDragging) return;

        isDragging = true;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseDrag()
    {
        if (!allowDragging || !isDragging) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;
    }

    void OnMouseUp()
    {
        if (!allowDragging) return;

        isDragging = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }

    void SpawnCopy()
    {
        if (wandererPrefab != null)
        {
            if (currentWanderers >= maxWanderers) return;

            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            Instantiate(wandererPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Wanderer prefab not assigned.");
        }
    }
}