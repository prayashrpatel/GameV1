using UnityEngine;

public class BalloonBehavior : MonoBehaviour
{
    public float maxScale = 0.5f;
    public float growSpeed = 5f;

    public float floatDistance = 2f;
    public float floatDuration = 3f;
    public AnimationCurve floatCurve;

    public float idleBobAmplitude = 0.15f;
    public float idleBobSpeed = 2f;
    public float swayAmplitude = 0.15f;
    public float swaySpeed = 1f;

    private Vector3 basePosition;
    private float floatTimer = 0f;
    private bool isIdle = false;
    private float idleTimer = 0f;
    private Vector3 finalFloatPosition;
    private bool grown = false;

    void Start()
    {
        basePosition = transform.position;
        transform.localScale = Vector3.zero; // Start tiny
    }

    void Update()
    {
        // GROWING EFFECT
        if (!grown)
        {
            float current = transform.localScale.x;
            float next = Mathf.MoveTowards(current, maxScale, growSpeed * Time.deltaTime);
            transform.localScale = new Vector3(next, next, next);

            if (Mathf.Abs(next - maxScale) < 0.001f)
                grown = true;

            // If you want floating motion while growing, comment out the line below:
            // return;
        }

        // SIMPLE FLOAT UP & IDLE BOB
        if (!isIdle)
        {
            floatTimer += Time.deltaTime;
            float t = Mathf.Clamp01(floatTimer / floatDuration);
            float vertical = basePosition.y + floatDistance * (floatCurve != null ? floatCurve.Evaluate(t) : t);
            float horizontal = basePosition.x; // No tilt/sway during float up

            transform.position = new Vector3(horizontal, vertical, basePosition.z);

            if (t >= 1f)
            {
                isIdle = true;
                finalFloatPosition = transform.position;
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer += Time.deltaTime;
            float yOffset = Mathf.Sin(idleTimer * idleBobSpeed) * idleBobAmplitude;
            float xOffset = Mathf.Sin(idleTimer * swaySpeed) * swayAmplitude;
            transform.position = finalFloatPosition + new Vector3(xOffset, yOffset, 0f);
        }
    }
}
