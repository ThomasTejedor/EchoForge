using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Head Bob Settings")]
    public float bobFrequency = 1.5f;  // How fast the bobbing occurs
    public float bobHorizontalAmplitude = 0.05f;  // Horizontal bob amplitude
    public float bobVerticalAmplitude = 0.05f;  // Vertical bob amplitude

    private float defaultPosY = 0.0f;  // Store the original camera position
    private float timer = 0.0f;  // Time tracker for bobbing

    private CharacterController controller;  // Reference to character controller

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<CharacterController>();  // Get character controller from the parent
        defaultPosY = transform.localPosition.y;  // Store the original Y position of the camera
    }

    // Update is called once per frame
    void Update()
    {
        // Only bob when the player is moving (not standing still)
        if (controller.velocity.magnitude > 0.1f && controller.isGrounded)
        {
            timer += Time.deltaTime * bobFrequency;  // Increase timer based on bob frequency

            // Calculate new X and Y positions for head bobbing
            float newX = Mathf.Sin(timer) * bobHorizontalAmplitude;
            float newY = defaultPosY + Mathf.Sin(timer * 2) * bobVerticalAmplitude;

            // Apply the new positions to the camera
            transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
        }
        else
        {
            // Reset timer and camera position when not moving
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY, transform.localPosition.z);
        }
    }
}