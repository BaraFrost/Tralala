using UnityEngine;

public class ScreenEdgePlacerWorld : MonoBehaviour
{
    public Transform leftObject;
    public Transform rightObject;

    [Tooltip("Фиксированные позиции по X")]
    public float leftX = -10f;
    public float rightX = 10f;

    void Start()
    {
        PlaceObjects();
    }

    void PlaceObjects()
    {
        if (leftObject != null)
        {
            Vector3 newPos = leftObject.position;
            newPos.x = leftX;
            leftObject.position = newPos;
        }

        if (rightObject != null)
        {
            Vector3 newPos = rightObject.position;
            newPos.x = rightX;
            rightObject.position = newPos;
        }
    }
}