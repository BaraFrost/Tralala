using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class BallSaveManager : MonoBehaviour
{
    [System.Serializable]
    public class BallSaveData
    {
        public string tag;
        public float x, y, z;
        public void SetPosition(Vector3 p) { x = p.x; y = p.y; z = p.z; }
        public Vector3 GetPosition() => new Vector3(x, y, z);
    }

    [System.Serializable]
    public class BallTagPrefab
    {
        public string tag;
        public GameObject prefab;
    }

    [Header("Префабы по тэгу")]
    public List<BallTagPrefab> ballPrefabs;

    [Header("Интервал автосохранения (сек)")]
    [SerializeField] private float saveInterval = 5f;

    private Dictionary<string, GameObject> tagToPrefab = new Dictionary<string, GameObject>();
    private Coroutine autoSaveRoutine;

    void Awake()
    {
        foreach (var b in ballPrefabs)
            if (!tagToPrefab.ContainsKey(b.tag))
                tagToPrefab.Add(b.tag, b.prefab);

        // Ждём, когда SDK подтянет данные
        YG2.onGetSDKData += OnDataLoaded;
    }

    void OnDestroy()
    {
        YG2.onGetSDKData -= OnDataLoaded;
    }

    private void OnDataLoaded()
    {
        // Загружаем шары один раз
        LoadBalls();

        // После первого успешного Load запускаем автосейв
        autoSaveRoutine = StartCoroutine(AutoSaveRoutine());
    }

    IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            SaveAllBalls();
        }
    }

    public void SaveAllBalls()
    {
        var balls = FindObjectsOfType<Ball>();
        YG2.saves.savedBalls.Clear();

        foreach (var ball in balls)
        {
            var d = new BallSaveData { tag = ball.tag };
            d.SetPosition(ball.transform.position);
            YG2.saves.savedBalls.Add(d);
        }

        YG2.SaveProgress();
    }

    public void LoadBalls()
    {
        // 1) уничтожаем все текущие шары
        foreach (var existing in FindObjectsOfType<Ball>())
            Destroy(existing.gameObject);

        // 2) создаём по сохранению
        var saved = YG2.saves.savedBalls;
        if (saved == null) return;

        foreach (var data in saved)
        {
            if (tagToPrefab.TryGetValue(data.tag, out var prefab))
                Instantiate(prefab, data.GetPosition(), Quaternion.identity);
        }
    }

    public void ClearSavedBalls()
    {
        // Очистка и сохранение сразу
        YG2.saves.savedBalls.Clear();
        YG2.SaveProgress();
    }
}
