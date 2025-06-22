using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private Transform[] spawnPoints;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    void Start()
    {
        // Создаём пул
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        // Запускаем спавн по таймеру
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (enemyPool.Count > 0)
        {
            SpawnEnemyFromPool();
            yield return new WaitForSeconds(10f);
        }
    }

    void SpawnEnemyFromPool()
    {
        if (enemyPool.Count == 0) return;

        GameObject enemy = enemyPool.Dequeue();
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        enemy.transform.position = spawnPoint.position;
        enemy.SetActive(true);

        // Сброс параметров врага
       // enemy.GetComponent<EnemyMover>().ResetEnemy(this);
    }

    // Метод возврата врага в пул
    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
