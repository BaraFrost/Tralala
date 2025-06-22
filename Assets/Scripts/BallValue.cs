using UnityEngine;

public class BallValue : MonoBehaviour
{
    public int value = 1;

    public void UpdateVisual()
    {
        // Здесь можно обновить цвет, размер, надпись и т.д.
        // Например:
        // GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
    }
}
