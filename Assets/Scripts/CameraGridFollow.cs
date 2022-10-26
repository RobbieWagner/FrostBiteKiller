using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script should be attached to player to force the camera to follow them as they move around
public class CameraGridFollow : MonoBehaviour
{
    bool delayedCameraMove;

    void Start() 
    {
        delayedCameraMove = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(Camera.current != null)
        Camera.current.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -10);
        else delayedCameraMove = true;
    }
}
