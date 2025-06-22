using UnityEngine;

public class ColliderAttractor : MonoBehaviour
{
    [Tooltip("������, ��������� �������� ����� ���������")]
    public GameObject targetObject;

    private Collider ownCollider;
    private Collider targetCollider;

    [Tooltip("�������� ������������")]
    public float attractionSpeed = 5f;

    void Start()
    {
        ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
        {
            Debug.LogError("�� ������� ��� Collider'�!");
            enabled = false;
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("������� ������ �� ��������!");
            enabled = false;
            return;
        }

        targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogError("�� ������� ������� ��� Collider'�!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // ������ ��������� ����� �� ���������� ���� � ������ ����������
        Vector3 closestPointOnTarget = targetCollider.ClosestPoint(ownCollider.bounds.center);

        // ������ ��������� ����� �� ���� ���������� � ���������� ����
        Vector3 closestPointOnOwn = ownCollider.ClosestPoint(closestPointOnTarget);

        // ������ �� ����� ����� � ����� ���� (����������� ����������)
        Vector3 direction = closestPointOnTarget - closestPointOnOwn;

        // ������� ������ � ���� ����������� � �������� ���������
        if (direction.magnitude > 0.001f)
        {
            transform.position += direction.normalized * attractionSpeed * Time.deltaTime;

            // ����� �� "������������" � �� �������� ������, ����� ������������, ����� ���������� ����� ���������
            if (Vector3.Distance(ownCollider.ClosestPoint(targetCollider.bounds.center), closestPointOnTarget) < 0.01f)
            {
                // ����� ���������� ���������� ��� ������ ��������� ������
                enabled = false;
            }
        }
        else
        {
            // ���� ���������� ��� �������������, ��������� ��������
            enabled = false;
        }
    }
}
