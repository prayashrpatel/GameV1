using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime = 0.3f; // The time it takes to smooth to the target position
    private Vector3 velocity = Vector3.zero; // Reference velocity for the SmoothDamp function

    void Start()
    {
        
    }

    void Update()
    {
        // Calculate the target position based on the mouse position
        Vector3 targetPosition = new Vector3(
            (Input.mousePosition.x - (Screen.width / 2)) / 300f,
            (Input.mousePosition.y - (Screen.height / 2)) / 300f,
            -10
        );

        // Smoothly move the camera to the target position
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, smoothTime);
    }
}