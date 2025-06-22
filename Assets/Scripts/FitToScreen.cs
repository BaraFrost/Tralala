using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitToScreen : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null)
        {
            return;
        }

        // Подгонка размера под экран
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = sr.sprite.bounds.size;

        Vector3 scale = transform.localScale;
        scale.x = screenWidth / spriteSize.x;
        scale.y = screenHeight / spriteSize.y;
        transform.localScale = scale;

        // Установка порядка слоя
        sr.sortingOrder = -10;           // Order in Layer
        sr.sortingLayerName = "Default";  // Название Sorting Layer (если ты используешь своё — замени здесь)
    }
}
