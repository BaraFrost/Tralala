using UnityEngine;

public class BallValue : MonoBehaviour
{
    public int value = 1;

    public void UpdateVisual()
    {
        // ����� ����� �������� ����, ������, ������� � �.�.
        // ��������:
        // GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
    }
}
