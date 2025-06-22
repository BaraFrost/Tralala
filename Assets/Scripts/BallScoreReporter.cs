using UnityEngine;

public class BallScoreReporter : MonoBehaviour
{
    void Start()
    {
        /*// Попробуем определить номер шара из его тега, например "Ball3" -> 3
        int score = GetScoreFromTag(gameObject.tag);
        if (score > 0)
        {
            ScoreManager.Instance.AddScore(score);
        }*/
    }

    private int GetScoreFromTag(string tag)
    {
        // Предполагаем, что теги названы как Ball1, Ball2, ..., Ball10
        if (tag.StartsWith("Ball"))
        {
            string numberPart = tag.Substring(4); // удаляем "Ball"
            if (int.TryParse(numberPart, out int value))
            {
                return value;
            }
        }
        return 0;
    }
}