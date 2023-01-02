using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    void Update()
    {
        //Rotate the object 60 degrees along the y-axis with respect to time
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }
}
