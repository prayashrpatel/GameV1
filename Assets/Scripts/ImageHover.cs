using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIHoverDarkenAndScale : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Targets (auto-filled if left empty)")]
    [SerializeField] private Image image;
    [SerializeField] private RectTransform targetScale;

    [Header("Look")]
    [Range(0f, 1f)] public float darkenMultiplier = 0.75f; // 1 = no change, 0.7 ≈ 30% darker
    public float hoverScale = 1.06f;
    public float easeInTime = 0.15f;
    public float easeOutTime = 0.20f;

    [Header("Easing")]
    public AnimationCurve easeIn = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve easeOut = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool useUnscaledTime = true;

    [Header("Press Feedback")]
    public float pressedScaleMultiplier = 0.98f;

    Color baseColor;
    Vector3 baseScale;
    Coroutine anim;
    bool hovered;

    void Awake()
    {
        if (!image) image = GetComponent<Image>();
        if (!targetScale) targetScale = (RectTransform)transform;

        baseColor = image ? image.color : Color.white;
        baseScale = targetScale.localScale;
    }

    void OnEnable()  { ResetVisual(); }
    void OnDisable() { ResetVisual(); }

    void ResetVisual()
    {
        if (anim != null) StopCoroutine(anim);
        if (image) image.color = baseColor;
        targetScale.localScale = baseScale;
        anim = null;
    }

    public void OnPointerEnter(PointerEventData _) { hovered = true;  StartAnim(true);  }
    public void OnPointerExit (PointerEventData _) { hovered = false; StartAnim(false); }

    public void OnPointerDown(PointerEventData _)
    {
        // tiny press compress while hovered
        targetScale.localScale = baseScale * (hovered ? hoverScale * pressedScaleMultiplier
                                                      : pressedScaleMultiplier);
    }

    public void OnPointerUp(PointerEventData _) { StartAnim(hovered); }

    void StartAnim(bool toHover)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Animate(toHover));
    }

    IEnumerator Animate(bool toHover)
    {
        float dur = toHover ? easeInTime : easeOutTime;
        if (dur <= 0f) { ApplyInstant(toHover); yield break; }

        Color fromC = image ? image.color : Color.white;
        Color toC = baseColor;
        if (toHover && image)
            toC = new Color(baseColor.r * darkenMultiplier,
                            baseColor.g * darkenMultiplier,
                            baseColor.b * darkenMultiplier,
                            baseColor.a);

        Vector3 fromS = targetScale.localScale;
        Vector3 toS   = baseScale * (toHover ? hoverScale : 1f);

        AnimationCurve curve = toHover ? easeIn : easeOut;

        float t = 0f;
        while (t < dur)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float u = Mathf.Clamp01(t / dur);
            float k = curve.Evaluate(u);

            if (image) image.color = Color.LerpUnclamped(fromC, toC, k);
            targetScale.localScale = Vector3.LerpUnclamped(fromS, toS, k);
            yield return null;
        }
        ApplyInstant(toHover);
    }

    void ApplyInstant(bool hover)
    {
        if (image)
            image.color = hover
                ? new Color(baseColor.r * darkenMultiplier,
                            baseColor.g * darkenMultiplier,
                            baseColor.b * darkenMultiplier,
                            baseColor.a)
                : baseColor;

        targetScale.localScale = baseScale * (hover ? hoverScale : 1f);
        anim = null;
    }
}
