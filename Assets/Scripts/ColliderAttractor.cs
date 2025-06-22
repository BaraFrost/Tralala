using UnityEngine;

public class ColliderAttractor : MonoBehaviour
{
    [Tooltip("Объект, коллайдер которого нужно притянуть")]
    public GameObject targetObject;

    private Collider ownCollider;
    private Collider targetCollider;

    [Tooltip("Скорость притягивания")]
    public float attractionSpeed = 5f;

    void Start()
    {
        ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
        {
            Debug.LogError("На объекте нет Collider'а!");
            enabled = false;
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("Целевой объект не назначен!");
            enabled = false;
            return;
        }

        targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogError("На целевом объекте нет Collider'а!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Найдем ближайшую точку на коллайдере цели к своему коллайдеру
        Vector3 closestPointOnTarget = targetCollider.ClosestPoint(ownCollider.bounds.center);

        // Найдем ближайшую точку на своём коллайдере к коллайдеру цели
        Vector3 closestPointOnOwn = ownCollider.ClosestPoint(closestPointOnTarget);

        // Вектор от своей точки к точке цели (направление притяжения)
        Vector3 direction = closestPointOnTarget - closestPointOnOwn;

        // Двигаем объект в этом направлении с заданной скоростью
        if (direction.magnitude > 0.001f)
        {
            transform.position += direction.normalized * attractionSpeed * Time.deltaTime;

            // Чтобы не "перетягивать" и не заходить внутрь, можно остановиться, когда расстояние очень маленькое
            if (Vector3.Distance(ownCollider.ClosestPoint(targetCollider.bounds.center), closestPointOnTarget) < 0.01f)
            {
                // Можно остановить притяжение или вообще выключить скрипт
                enabled = false;
            }
        }
        else
        {
            // Если коллайдеры уже соприкасаются, остановим движение
            enabled = false;
        }
    }
}
