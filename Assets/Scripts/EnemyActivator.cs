using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [Header("����� �� �����")]
    public List<GameObject> enemies; // ���� �������� ������ � ����������

    [Header("��������� ���������")]
    public float spawnInterval = 5f; // �������� ����� �����������

    private float timer;

    void Start()
    {
        // ��������� ���� ������ ��� ������
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
        // �������� ���� ���������� ������
        List<GameObject> inactiveEnemies = enemies.FindAll(e => !e.activeSelf);

        if (inactiveEnemies.Count > 0)
        {
            int index = Random.Range(0, inactiveEnemies.Count);
            inactiveEnemies[index].SetActive(true);
        }
    }
}
