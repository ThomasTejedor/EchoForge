using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float crouchSpeed = 5.0f;
    public float proneSpeed = 2.5f;
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

    bool isCrouching = false;
    bool isProne = false;
    bool isSprinting = false;
    float originalHeight;
    public float crouchHeight = 1.0f;
    public float proneHeight = 0.5f;
    public float transitionSpeed = 5.0f; // Speed for smooth crouch/prone transitions

    void Start()
    {
        // Lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        character = GetComponent<CharacterController>();
        originalHeight = character.height;

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
        HandleCrouchingProne();
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
        if (isProne)
        {
            return proneSpeed;
        }
        else if (isCrouching)
        {
            return crouchSpeed;
        }
        else if (isSprinting)
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
            if (Input.GetButtonDown("Jump") && !isCrouching && !isProne) // Prevent jumping while crouching or prone
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity when in the air
        }
    }

    void HandleCrouchingProne()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Toggle crouch
        {
            isCrouching = !isCrouching;
            isProne = false; // Disable prone if crouching
            StopAllCoroutines(); // Stop any previous transitions
            StartCoroutine(AdjustHeight(isCrouching ? crouchHeight : originalHeight));
        }
        else if (Input.GetKeyDown(KeyCode.Z)) // Toggle prone
        {
            isProne = !isProne;
            isCrouching = false; // Disable crouch if prone
            StopAllCoroutines(); // Stop any previous transitions
            StartCoroutine(AdjustHeight(isProne ? proneHeight : originalHeight));
        }
    }

    // Smooth transition for height adjustment
    IEnumerator AdjustHeight(float targetHeight)
    {
        float currentHeight = character.height;
        while (Mathf.Abs(currentHeight - targetHeight) > 0.01f)
        {
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);
            character.height = currentHeight;
            character.center = new Vector3(0, currentHeight / 2, 0); // Adjust center based on height
            yield return null;
        }
        character.height = targetHeight;
        character.center = new Vector3(0, targetHeight / 2, 0);
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isProne) // Sprint only if not crouching or prone
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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit rotation between -90 and 90 degrees

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