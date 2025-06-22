using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallState
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
}

public class BallStateManager : MonoBehaviour
{
    public static BallStateManager Instance;

    public List<GameObject> balls;
    private List<BallState> savedStates = new List<BallState>();
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveState()
    {
        savedStates.Clear();
        foreach (var ball in balls)
        {
            BallState state = new BallState
            {
                position = ball.transform.position,
                rotation = ball.transform.rotation,
                isActive = ball.activeSelf
            };
            savedStates.Add(state);
        }
    }

    public void LoadState()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (i < savedStates.Count)
            {
                balls[i].transform.position = savedStates[i].position;
                balls[i].transform.rotation = savedStates[i].rotation;
                balls[i].SetActive(savedStates[i].isActive);
            }
        }
    }

    public void ResetState()
    {
        savedStates.Clear();
        score = 0;
        foreach (var ball in balls)
        {
            ball.SetActive(true);
            // Можно сбросить позиции, если нужно
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }
}