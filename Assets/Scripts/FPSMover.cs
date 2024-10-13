using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintSpeed = 15.0f; // Sprint speed
    public float jumpForce = 8.0f;
    public float gravity = -9.8f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;

    CharacterController character;
    public GameObject cam;

    float moveFB, moveLR;
    float rotX, rotY;
    float xRotation = 0f;
    float verticalVelocity = 0f;
    public bool webGLRightClickRotation = true;

    bool isSprinting = false;

    void Start()
    {
        // Lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        character = GetComponent<CharacterController>();

        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
    }

    void Update()
    {
        moveFB = Input.GetAxis("Vertical") * GetSpeed();
        moveLR = Input.GetAxis("Horizontal") * GetSpeed();

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        CheckForWaterHeight();
        HandleJumping();
        HandleSprinting();

        Vector3 movement = new Vector3(moveLR, verticalVelocity, moveFB);
        movement = transform.rotation * movement;

        character.Move(movement * Time.deltaTime);

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);
            }
        }
        else
        {
            CameraRotation(cam, rotX, rotY);
        }
    }

    float GetSpeed()
    {
        if (isSprinting)
        {
            return sprintSpeed; // Use sprint speed if sprinting
        }
        return speed;
    }

    void HandleJumping()
    {
        if (character.isGrounded)
        {
            verticalVelocity = gravity * Time.deltaTime; // Apply gravity when grounded
            if (Input.GetButtonDown("Jump")) // Jump when the jump button is pressed (usually spacebar)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity when in the air
        }
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // Sprint
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        // Horizontal rotation (Y axis)
        transform.Rotate(0, rotX * Time.deltaTime, 0);

        // Vertical rotation (X axis) with clamping
        xRotation -= rotY * Time.deltaTime; // Subtract to invert vertical look
        xRotation = Mathf.Clamp(xRotation, -70f, 50f); // Limit rotation between -90 and 90 degrees

        // Apply clamped rotation to the camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }
}