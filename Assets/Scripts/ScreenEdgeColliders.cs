using UnityEngine;

public class ScreenEdgeColliders : MonoBehaviour
{
    public float thickness = 1f; // толщина коллайдеров

    void Start()
    {
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;
        Vector2 center = cam.transform.position;

        CreateEdge("Top", new Vector2(center.x, center.y + screenHeight / 2f + thickness / 2f), new Vector2(screenWidth, thickness));
        CreateEdge("Bottom", new Vector2(center.x, center.y - screenHeight / 2f - thickness / 2f), new Vector2(screenWidth, thickness));
        CreateEdge("Left", new Vector2(center.x - screenWidth / 2f - thickness / 2f, center.y), new Vector2(thickness, screenHeight));
        CreateEdge("Right", new Vector2(center.x + screenWidth / 2f + thickness / 2f, center.y), new Vector2(thickness, screenHeight));
    }

    void CreateEdge(string name, Vector2 pos, Vector2 size)
    {
        GameObject edge = new GameObject(name);
        edge.transform.position = pos;
        edge.transform.parent = transform;

        BoxCollider2D col = edge.AddComponent<BoxCollider2D>();
        col.size = size;
        col.isTrigger = false;

        edge.layer = LayerMask.NameToLayer("ScreenEdge"); // Не забудь создать слой в Unity
    }
}