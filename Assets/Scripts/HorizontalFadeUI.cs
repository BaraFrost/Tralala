using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HorizontalFadeUI : BaseMeshEffect
{
    public enum FadeDirection
    {
        LeftToRight,
        RightToLeft
    }

    [Header("�������� ��������")]
    [Range(0f, 1f)] public float fadeStrength = 1f;
    public FadeDirection direction = FadeDirection.LeftToRight;

    [Header("������ �������� (������ ���� � ����)")]
    public bool enableSecondFade = false;
    [Range(0f, 1f)] public float secondFadeStrength = 1f; // 1 = �����, 0 = ���������
    public FadeDirection secondFadeDirection = FadeDirection.RightToLeft;
    [Range(0f, 1f)] public float secondFadeWidth = 0.15f; // ������ ���� � ����� ������

    private RectTransform rectTransform;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        rectTransform = rectTransform ?? GetComponent<RectTransform>();
        Rect rect = graphic.rectTransform.rect;

        float minX = rect.xMin;
        float maxX = rect.xMax;
        float width = maxX - minX;

        bool fadeFromLeft = direction == FadeDirection.LeftToRight;
        bool secondFadeFromLeft = secondFadeDirection == FadeDirection.LeftToRight;

        UIVertex vert = new UIVertex();

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vert, i);

            float x = vert.position.x;
            float t = Mathf.InverseLerp(minX, maxX, x);

            // --- �������� 1 (������� �� ���� ������)
            float fade1 = fadeFromLeft ? t : 1f - t;
            float alpha1 = Mathf.Lerp(0f, 1f, fade1 * (1f - fadeStrength) + fadeStrength);

            // --- �������� 2 (��������� � ����)
            float alpha2 = 1f; // �� ��������� ��� �������

            if (enableSecondFade && secondFadeWidth > 0f)
            {
                float fade2 = secondFadeFromLeft ? t : 1f - t;

                if (fade2 <= secondFadeWidth)
                {
                    float normalized = fade2 / secondFadeWidth;
                    alpha2 = Mathf.Lerp(1f, secondFadeStrength, normalized);
                }
                else
                {
                    alpha2 = secondFadeStrength;
                }
            }

            // �������� ����� = ��������� ���� ����������
            float finalAlpha = alpha1 * alpha2;

            Color32 c = vert.color;
            vert.color = new Color32(c.r, c.g, c.b, (byte)(c.a * finalAlpha));
            vh.SetUIVertex(vert, i);
        }
    }
}
