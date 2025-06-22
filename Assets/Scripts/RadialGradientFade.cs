using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialFadeUI : BaseMeshEffect
{
    [Range(0f, 1f)] public float fadeStrength = 1f;
    public bool useCanvasCenter = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        Rect rect = graphic.rectTransform.rect;
        Vector2 center = useCanvasCenter ? Vector2.zero : rect.center;
        float maxDistance = Vector2.Distance(rect.min, center); // Радиус от центра до края

        UIVertex vert = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vert, i);

            Vector2 localPos = vert.position;
            float distance = Vector2.Distance(localPos, center);
            float t = Mathf.Clamp01(distance / maxDistance); // 0 в центре, 1 на краю

            Color32 originalColor = vert.color;
            byte newAlpha = (byte)(originalColor.a * (1f - t * fadeStrength));
            vert.color = new Color32(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            vh.SetUIVertex(vert, i);
        }
    }
}