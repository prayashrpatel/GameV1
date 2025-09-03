using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections; // Ensure this line is added
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
    public int normalSize = 14;
    public int hoverSize = 18;
    public float darkenAmount = 0.5f; // Amount to darken the text color (0 to 1)
    public float transitionDuration = 0.3f;
    private Coroutine currentTransition;
    private int currentSize;
    private Color startColor;

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogError("TextMeshProUGUI component not found on the GameObject. Please assign it manually.");
            }
        }

        // Set initial size and color
        currentSize = normalSize;
        text.fontSize = normalSize;
        startColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        currentTransition = StartCoroutine(ChangeProperties(currentSize, hoverSize, startColor, GetDarkenedColor(startColor), transitionDuration));
        currentSize = hoverSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        currentTransition = StartCoroutine(ChangeProperties(currentSize, normalSize, startColor, startColor, transitionDuration));
        currentSize = normalSize;
    }

    private IEnumerator ChangeProperties(int startSize, int targetSize, Color startCol, Color targetCol, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float progress = timer / duration;
            text.fontSize = Mathf.RoundToInt(Mathf.Lerp(startSize, targetSize, progress));
            text.color = Color.Lerp(startCol, targetCol, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        text.fontSize = targetSize; // Ensure final size is exactly the target size
        text.color = targetCol; // Ensure final color is exactly the target color
    }

    private Color GetDarkenedColor(Color originalColor)
    {
        // Calculate a darker shade of the original color
        float h, s, v;
        Color.RGBToHSV(originalColor, out h, out s, out v);
        Color darkerColor = Color.HSVToRGB(h, s, v * (1 - darkenAmount));
        darkerColor.a = originalColor.a; // Maintain original alpha
        return darkerColor;
    }
}