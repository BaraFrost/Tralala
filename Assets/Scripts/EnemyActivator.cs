using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [Header("Враги на сцене")]
    public List<GameObject> enemies; // Сюда добавляй врагов в инспекторе

    [Header("Параметры активации")]
    public float spawnInterval = 5f; // Интервал между активациями

    private float timer;

    void Start()
    {
        // Отключаем всех врагов при старте
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }

        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ActivateRandomEnemy();
            timer = spawnInterval;
        }
    }

    void ActivateRandomEnemy()
    {
        // Собираем всех неактивных врагов
        List<GameObject> inactiveEnemies = enemies.FindAll(e => !e.activeSelf);

        if (inactiveEnemies.Count > 0)
        {
            int index = Random.Range(0, inactiveEnemies.Count);
            inactiveEnemies[index].SetActive(true);
        }
    }
}
