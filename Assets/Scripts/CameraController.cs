using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to handle player's camera
public class CameraController : MonoBehaviour
{
    //Reference the camera anchor GO attached to the player
    //This is where the camera is located
    public Transform camAnchor;

    //Control the mouse movement sensitivity
    public float lookSensitivity = 2.5f;

    //Check if game is paused
    bool paused = false;

    //Max and min height the player can look at
    float _minXLook = -60f;
    float _maxXLook = 60f;

    //Variable to store the current x rotation
    private float _currentXRotation;

    //Gets called at the end of the frame
    //Want to rotate camera after all other updates
    void LateUpdate()
    {
        if (!paused)
        {
            ////Variables to store the mouse's movements
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            //Reference the player's transform rotation
            //Rotates the player along the y-axis, which is the horizontal axis
            transform.eulerAngles += Vector3.up * x * lookSensitivity;

            //Store the current x rotation by how far the player moved the mouse times the sensitivity 
            _currentXRotation -= y * lookSensitivity;

            //The current x rotation can't go below the minXLook value or above the maxXLook value
            _currentXRotation = Mathf.Clamp(_currentXRotation, _minXLook, _maxXLook);

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
