using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DownwardRayVisualizer : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float maxDistance = 10f;

    // Можно указать несколько слоёв в инспекторе (например, Ball и Wall)
    [SerializeField] private LayerMask hitLayers;

    [Header("Line Appearance")]
    [SerializeField] private Color rayColor = Color.white;
    [SerializeField] private float lineWidth = 0.05f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
    }

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * maxDistance;

        // Raycast, реагирующий на несколько слоёв
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, maxDistance, hitLayers);
        if (hit.collider != null)
        {
            end = hit.point;
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}