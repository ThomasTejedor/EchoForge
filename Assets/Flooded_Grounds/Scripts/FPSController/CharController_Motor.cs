using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR;
    float rotX, rotY;
    float xRotation = 0f; // Variable to store the clamped vertical rotation
    public bool webGLRightClickRotation = true;
    float gravity = -9.8f;

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

    void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;
        moveLR = Input.GetAxis("Vertical") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        CheckForWaterHeight();

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(cam, rotX, rotY);
        }

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        // Horizontal rotation (Y axis)
        transform.Rotate(0, rotX * Time.deltaTime, 0);

        // Vertical rotation (X axis) with clamping
        xRotation -= rotY * Time.deltaTime; // Subtract to invert vertical look
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit rotation between -90 and 90 degrees

        // Apply clamped rotation to the camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}