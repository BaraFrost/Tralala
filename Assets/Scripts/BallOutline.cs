using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class BallOutline : MonoBehaviour
{
    [Header("��������� �������")]
    [SerializeField] private float outlineThickness = 0.1f; // ������� �������

    [Header("�������� ��� ������� (��������, �����)")]
    [SerializeField] private Material outlineMaterial;

    private GameObject outlineObject;

    void Start()
    {
        CreateOutline();
    }

    private void CreateOutline()
    {
        if (outlineMaterial == null)
        {
            Debug.LogWarning("�� ����� �������� �������!");
            return;
        }

        // ������� �������� ������ ��� �������
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;

        SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer outlineRenderer = outlineObject.AddComponent<SpriteRenderer>();

        // �������� ������ � ����������
        outlineRenderer.sprite = originalRenderer.sprite;
        outlineRenderer.material = outlineMaterial;
        outlineRenderer.sortingLayerID = originalRenderer.sortingLayerID;
        outlineRenderer.sortingOrder = originalRenderer.sortingOrder - 1; // ����� ��� ��� �����

        // ������������ �������
        float scaleX = 1f + outlineThickness * 2f;
        float scaleY = 1f + outlineThickness * 2f;
        outlineObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    public void SetOutlineThickness(float thickness)
    {
        outlineThickness = thickness;
        if (outlineObject != null)
        {
            outlineObject.transform.localScale = new Vector3(1f + outlineThickness * 2f, 1f + outlineThickness * 2f, 1f);
        }
    }
}