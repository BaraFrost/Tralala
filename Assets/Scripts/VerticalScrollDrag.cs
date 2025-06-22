using UnityEngine;

public class VerticalScrollDrag : MonoBehaviour
{
    [Header("Настройки прокрутки")]
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float mouseWheelSpeed = 200f;

    [Header("Границы прокрутки (для свайпа/драг)")]
    [SerializeField] private float upperLimitY = 500f;
    [SerializeField] private float lowerLimitY = -500f;

    [Header("Границы прокрутки (для колесика мыши)")]
    [SerializeField] private float mouseWheelMinY = 0f;
    [SerializeField] private float mouseWheelMaxY = 2200f;

    [Header("Elastic Settings")]
    [Tooltip("Насколько сильно замедляется прокрутка при выходе за пределы")]
    [SerializeField] private float elasticity = 0.3f;
    [Tooltip("Скорость возврата в границы после отпускания")]
    [SerializeField] private float returnSpeed = 10f;

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private bool shouldReturn = false;
    private Vector3 targetReturnPosition;

    void Update()
    {
        // 🟩 ПРОКРУТКА КОЛЕСИКОМ МЫШИ - с жесткими границами 0..2200
        float wheelDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(wheelDelta) > 0.01f)
        {
            Vector3 pos = transform.localPosition;
            float newY = pos.y + wheelDelta * mouseWheelSpeed * Time.deltaTime;
            newY = Mathf.Clamp(newY, mouseWheelMinY, mouseWheelMaxY);
            transform.localPosition = new Vector3(pos.x, newY, pos.z);
        }

        // 🟦 ТАЧ
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touch.position;
                    isDragging = true;
                    shouldReturn = false;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 delta = touch.position - lastTouchPosition;
                        MoveGroup(delta.y);
                        lastTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    HandleElasticReturn();
                    break;
            }
        }
        // 🟨 МЫШЬ
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastTouchPosition = Input.mousePosition;
                isDragging = true;
                shouldReturn = false;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 delta = currentMousePosition - lastTouchPosition;
                MoveGroup(delta.y);
                lastTouchPosition = currentMousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                HandleElasticReturn();
            }
        }

        // 🔁 Плавный возврат в пределы (для свайпа/драг)
        if (shouldReturn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetReturnPosition, Time.deltaTime * returnSpeed);
            if (Vector3.Distance(transform.localPosition, targetReturnPosition) < 0.1f)
            {
                transform.localPosition = targetReturnPosition;
                shouldReturn = false;
            }
        }
    }

    private void MoveGroup(float deltaY)
    {
        Vector3 currentPos = transform.localPosition;
        float newY = currentPos.y + deltaY * scrollSpeed * Time.deltaTime;

        // Превышение верхнего лимита (для свайпа/драг)
        if (newY > upperLimitY)
        {
            float excess = newY - upperLimitY;
            newY = upperLimitY + excess * elasticity;
        }
        // Превышение нижнего лимита (для свайпа/драг)
        else if (newY < lowerLimitY)
        {
            float excess = lowerLimitY - newY;
            newY = lowerLimitY - excess * elasticity;
        }

        transform.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
    }

    private void HandleElasticReturn()
    {
        float y = transform.localPosition.y;
        if (y > upperLimitY)
        {
            targetReturnPosition = new Vector3(transform.localPosition.x, upperLimitY, transform.localPosition.z);
            shouldReturn = true;
        }
        else if (y < lowerLimitY)
        {
            targetReturnPosition = new Vector3(transform.localPosition.x, lowerLimitY, transform.localPosition.z);
            shouldReturn = true;
        }
    }
}
