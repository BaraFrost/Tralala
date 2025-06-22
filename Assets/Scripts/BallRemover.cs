using UnityEngine;

public class BallRemover : MonoBehaviour
{
    [Tooltip("Теги объектов, которые нужно удалить.")]
    [SerializeField] private string[] tagsToRemove = { "Ball1", "Ball2", "Ball3" };

    /// <summary>
    /// Вызывается из UI-кнопки. Удаляет все объекты с заданными тегами.
    /// </summary>
    public void RemoveAllBalls()
    {
        foreach (string tag in tagsToRemove)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject ball in balls)
            {
                Destroy(ball);
            }
        }
    }
}
