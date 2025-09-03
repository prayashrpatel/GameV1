using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingBalloon : MonoBehaviour
{
    public float floatSpeed = 20f;

    void Update()
    {
        // Remove RectTransform usage — use transform.position instead!
        transform.position += new Vector3(0f, floatSpeed * Time.deltaTime, 0f);
    }

    void OnMouseDown()
    {
        if (Level0Manager.instance != null)
        {
            Level0Manager.instance.BalloonPopped(transform.position);
        }

        Destroy(gameObject);
    }
}
