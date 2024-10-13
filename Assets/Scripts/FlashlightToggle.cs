using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject flashlight;
    private Light flashlightLight;

    public AudioSource turnOn;
    public AudioSource turnOff;

    public bool on;
    public bool off;

    void Start()
    {
        off = true;
        
        flashlightLight = flashlight.GetComponentInChildren<Light>();

        if (flashlightLight != null)
        {
            flashlightLight.enabled = false; // Ensure the light is off at the start
        }
        else
        {
            Debug.LogError("No Light component found on the flashlight object or its children.");
        }
    }

    void Update()
    {
        ToggleFlashlight();
    }

    void ToggleFlashlight()
    {
        if (off && Input.GetButtonDown("F"))
        {
            if (flashlightLight != null)
            {
                flashlightLight.enabled = true; // Turn on the light
            }

            turnOn.Play();
            off = false;
            on = true;

            Debug.Log("Turned on flashlight");
        } 
        else if(on && Input.GetButtonDown("F"))
        {
            if (flashlightLight != null)
            {
                flashlightLight.enabled = false; // Turn on the light
            }

            turnOff.Play();
            off = true;
            on = false;

            Debug.Log("Turned off flashlight");
        }
    }
}
