using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour 
{

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float mouseSensitivity = 100.0f;  // Control how sensitive the mouse movement is

    float xRotation = 0.0f;
    
    // Start is called before the first frame update
    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() 
    {
        MovePlayer();
        RotatePlayer();
    }

    void PrintInstructions()
    {
        Debug.Log("This is an instruction to the console!");
        Debug.Log("Hello!");
        Debug.Log("Move the player you kov!");
    }

    void MovePlayer()
    {
        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        Vector3 move = transform.right * xValue + transform.forward * zValue;
        transform.Translate(move, Space.World);
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;  // Horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  // Vertical mouse movement (optional for looking up/down)

        xRotation -= mouseY;  // Invert Y-axis for correct "looking up/down"
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limit vertical rotation to avoid full flips

        // Rotate the camera (or player's head) up/down
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player's body left/right based on mouse X movement
        transform.Rotate(Vector3.up * mouseX);
    }
}