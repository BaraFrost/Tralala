using UnityEngine;

public class BallRemover : MonoBehaviour
{
    [Tooltip("���� ��������, ������� ����� �������.")]
    [SerializeField] private string[] tagsToRemove = { "Ball1", "Ball2", "Ball3" };

    /// <summary>
    /// ���������� �� UI-������. ������� ��� ������� � ��������� ������.
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
