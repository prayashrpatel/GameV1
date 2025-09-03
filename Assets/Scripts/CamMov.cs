using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMov : MonoBehaviour
{
    public Vector3 Offset;
    public float speed = 60;
    public float smoothTime = 0.3f; // The time it takes to smooth to the target position

    private Vector3 velocity = Vector3.zero; // Reference velocity for the SmoothDamp function

    // Start is called before the first frame update
    void Start()
    {
        Offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the target position based on the mouse position
        Vector3 targetPosition = Offset + new Vector3(
            (Input.mousePosition.x - (Screen.width / 2)) / speed,
            (Input.mousePosition.y - (Screen.height / 2)) / speed,
            -10
        );

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}