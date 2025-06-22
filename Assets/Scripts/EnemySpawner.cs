using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPoolEntry
{
    public GameObject prefab;        // Префаб врага
    public int count = 1;            // Количество копий
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки врагов")]
    public List<EnemyPoolEntry> enemyTypes;

    [Header("Точки спавна")]
    public Transform[] spawnPoints;

    [Header("Интервал спавна")]
    public float spawnInterval = 10f;

    private List<GameObject> pooledEnemies = new List<GameObject>();
    private float timer;

    void Start()
    {
        // Создаем пул всех врагов
        foreach (var entry in enemyTypes)
        {
            for (int i = 0; i < entry.count; i++)
            {
                GameObject enemy = Instantiate(entry.prefab);
                enemy.SetActive(false);
                pooledEnemies.Add(enemy);
            }
        }

        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ActivateNextEnemy();
            timer = spawnInterval;
        }
    }

    void ActivateNextEnemy()
    {
        foreach (GameObject enemy in pooledEnemies)
        {
            if (!enemy.activeSelf)
            {
                // Выбираем случайную точку спавна
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;

                enemy.SetActive(true);
                
                break;
            }
        }
    }
}