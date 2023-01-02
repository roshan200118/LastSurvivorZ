using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to manage the Car's camera controller
public class CarCameraController : MonoBehaviour
{
    //Reference the camera anchor GO attached to the player
    //This is where the camera is located
    public Transform camAnchor;

    //Control the mouse movement sensitivity
    public float lookSensitivity = 2.5f;

    bool paused = false;

    //Variable to store the current x rotation
    private float _currentXRotation;

    //Gets called at the end of the frame
    //Want to rotate camera after all other updates
    void LateUpdate()
    {
        if (!paused)
        {
            //Store the camera's anchor's rotation
            Vector3 clampedAngle = camAnchor.eulerAngles;

            //Set the x rotation
            clampedAngle.x = _currentXRotation;

            //Set the camera anchor rotation
            camAnchor.eulerAngles = clampedAngle;
        }
    }

    public void togglePause()
    {
        paused = !paused;
    }
}
