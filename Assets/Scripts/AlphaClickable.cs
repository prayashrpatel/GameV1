using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaClickable : MonoBehaviour {
    [Range(0f,1f)] public float threshold = 0.2f;
    void Awake() {
        var img = GetComponent<Image>();
        img.raycastTarget = true;
        img.useSpriteMesh = true;               // tighter to the cloud outline
        img.alphaHitTestMinimumThreshold = threshold;
    }
}
