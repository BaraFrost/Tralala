using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private List<GameObject> ballsToSpawn; // список возможных шаров
    [SerializeField] private int minBalls = 3;
    [SerializeField] private int maxBalls = 7;
    [SerializeField] private float ySpawn = 0f;

    private bool hasSpawned = false;
    private float spawnDelay = 2f;
    private float timer = 0f;

    void Update()
    {
        if (hasSpawned) return;

        timer += Time.deltaTime;

        // Условие 1: прошло 2 секунды
        if (timer >= spawnDelay)
        {
            SpawnBalls();
            hasSpawned = true;
        }

        // Условие 2: игрок нажал на экран
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBalls();
            hasSpawned = true;
        }
    }

    void SpawnBalls()
    {
        int count = Random.Range(minBalls, maxBalls + 1);

        for (int i = 0; i < count; i++)
        {
            float x = GetRandomXWithinScreen();
            GameObject prefab = ballsToSpawn[Random.Range(0, ballsToSpawn.Count)];
            Vector3 spawnPosition = new Vector3(x, ySpawn, 0f);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    float GetRandomXWithinScreen()
    {
        Camera cam = Camera.main;
        float z = Mathf.Abs(cam.transform.position.z);

        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1, 0, z));

        return Random.Range(left.x, right.x);
    }
}